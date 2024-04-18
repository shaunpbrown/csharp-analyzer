import { select, tree, hierarchy, linkHorizontal, zoom, zoomIdentity } from 'https://cdn.jsdelivr.net/npm/d3@7/+esm';

let graphOffset = 0;

const clearGraph = () => {
    // Select the SVG container and clear any existing content
    const svg = select('svg');
    svg.selectAll('*').remove();

    // Define the zoom behavior and apply it to the SVG
    const zoomGroup = svg.append('g').attr('id', 'zoom-group');
    const zoomBehavior = zoom().on('zoom', (event) => {
        zoomGroup.attr('transform', event.transform);
    });
    svg.call(zoomBehavior);
    svg.call(zoomBehavior.transform, zoomIdentity);

    // Reset graph offset
    graphOffset = 0;
};

const createGraph = (data, fileName = "") => {

    const root = hierarchy(JSON.parse(data));
    const maxDepth = root.height;

    // Specify the charts’ dimensions. The height is variable, depending on the layout.
    const width = 928;
    const marginTop = 10;
    const marginRight = 10;
    const marginBottom = 10;
    const marginLeft = 40;

    // Rows are separated by dx pixels, columns by dy pixels. These names can be counter-intuitive
    // (dx is a height, and dy a width). This because the tree must be viewed with the root at the
    // “bottom”, in the data domain. The width of a column is based on the tree’s height.
    const dx = 15;
    const dy = (width - marginRight - marginLeft) / (1 + root.height) + 20;

    const maxBreadth = (root) => {
        let maxBreadth = 0;
        let level = 0; // Start at root level
        let queue = [{ node: root, level: 0 }];

        while (queue.length > 0) {
            let levelNodes = 0; // Nodes at the current level
            const nextQueue = [];

            queue.forEach(({ node, level: nodeLevel }) => {
                if (nodeLevel > level) {
                    maxBreadth = Math.max(maxBreadth, levelNodes);
                    levelNodes = 0;
                    level = nodeLevel;
                }
                levelNodes++;
                (node.children || []).forEach(child => nextQueue.push({ node: child, level: nodeLevel + 1 }));
            });

            maxBreadth = Math.max(maxBreadth, levelNodes);
            queue = nextQueue;
        }

        return maxBreadth;
    };

    const graphHeightSize = maxBreadth(root) * 50;
    const graphWidthSize = maxDepth * 150;

    // Define the tree layout and the shape for links.
    const treeLayout = tree().size([graphHeightSize, graphWidthSize]);
    const diagonal = linkHorizontal().x(d => d.y).y(d => d.x);

    const zoomGroup = select('#zoom-group')
        .attr("width", width)
        .attr("height", dx)
        .attr("viewBox", [-marginLeft, -marginTop, width, dx])
        .attr("style", "max-width: 100%; height: auto; font: 10px sans-serif; user-select: none;");

    const contentGroup = zoomGroup.append('g')
        .attr('transform', `translate(0, ${graphOffset})`);

    const gLink = contentGroup.append("g")
        .attr("fill", "none")
        .attr("stroke", "#555")
        .attr("stroke-opacity", 0.4)
        .attr("stroke-width", 1.5);

    const gNode = contentGroup.append("g")
        .attr("cursor", "pointer")
        .attr("pointer-events", "all");

    function update(event, source) {
        const duration = event?.altKey ? 2500 : 250; // hold the alt key to slow down the transition
        const nodes = root.descendants().reverse();
        const links = root.links();

        // Compute the new tree layout.
        treeLayout(root);

        let left = root;
        let right = root;
        root.eachBefore(node => {
            if (node.x < left.x) left = node;
            if (node.x > right.x) right = node;
        });

        const height = right.x - left.x + marginTop + marginBottom;

        const transition = contentGroup.transition()
            .duration(duration)
            .attr("height", height)
            .attr("viewBox", [-marginLeft, left.x - marginTop, width, height])
            .tween("resize", window.ResizeObserver ? null : () => () => contentGroup.dispatch("toggle"));

        // Update the nodes…
        const node = gNode.selectAll("g")
            .data(nodes, d => d.id);

        // Enter any new nodes at the parent's previous position.
        const nodeEnter = node.enter().append("g")
            .attr("transform", d => `translate(${source.y0},${source.x0})`)
            .attr("fill-opacity", 0)
            .attr("stroke-opacity", 0)
            .on("click", (event, d) => {
                d.children = d.children ? null : d._children;
                update(event, d);
            });

        nodeEnter.append("circle")
            .attr("r", 2.5)
            .attr("fill", d => d._children ? "#555" : "#999")
            .attr("stroke-width", 10);

        nodeEnter.append("text")
            .attr('class', 'displayName')
            .attr("dy", "0.31em")
            .attr("x", d => d._children ? -6 : 6)
            .attr("text-anchor", "end")
            .text(d => d.data.syntaxData.displayName)
            .attr("stroke-linejoin", "round")
            .attr("stroke-width", 3)
            .attr("stroke", "white")
            .attr("paint-order", "stroke");

        nodeEnter.append("text")
            .attr('class', 'tokenKind')
            .attr("dy", "1.5em")
            .attr("text-anchor", "end")
            .attr('fill', 'rgba(0, 0, 0, 0.4)')
            .style('font-size', '.7em')
            .text(d => d.data.syntaxData.tokenKind);

        // Transition nodes to their new position.
        const nodeUpdate = node.merge(nodeEnter).transition(transition)
            .attr("transform", d => `translate(${d.y},${d.x})`)
            .attr("fill-opacity", 1)
            .attr("stroke-opacity", 1);

        // Transition exiting nodes to the parent's new position.
        const nodeExit = node.exit().transition(transition).remove()
            .attr("transform", d => `translate(${source.y},${source.x})`)
            .attr("fill-opacity", 0)
            .attr("stroke-opacity", 0);

        // Update the links…
        const link = gLink.selectAll("path")
            .data(links, d => d.target.id);

        // Enter any new links at the parent's previous position.
        const linkEnter = link.enter().append("path")
            .attr("d", d => {
                const o = { x: source.x0, y: source.y0 };
                return diagonal({ source: o, target: o });
            });

        // Transition links to their new position.
        link.merge(linkEnter).transition(transition)
            .attr("d", diagonal);

        // Transition exiting nodes to the parent's new position.
        link.exit().transition(transition).remove()
            .attr("d", d => {
                const o = { x: source.x, y: source.y };
                return diagonal({ source: o, target: o });
            });

        // Stash the old positions for transition.
        root.eachBefore(d => {
            d.x0 = d.x;
            d.y0 = d.y;
        });
    }

    // Do the first update to the initial configuration of the tree — where a number of nodes
    // are open (arbitrarily selected as the root, plus nodes with 7 letters).
    root.x0 = dy / 2;
    root.y0 = 0;
    root.descendants().forEach((d, i) => {
        d.id = i;
        d._children = d.children;
        if (d.depth && d.data.syntaxData.displayName.length === 7) d.children = null;
    });

    update(null, root);

    // Draw the file box
    if (fileName !== "") {
        const widthPadding = 100;
        const textLeftPadding = 30;

        contentGroup.append('text')
            .attr('x', -widthPadding + textLeftPadding)
            .attr('y', graphHeightSize - 10)
            .text(fileName)
            .attr('font-size', '30px')
            .attr('fill', 'rgba(255, 0, 0, 0.2)');

        contentGroup.append('rect')
            .attr('x', -widthPadding)
            .attr('y', 0)
            .attr('width', graphWidthSize + (widthPadding * 2))
            .attr('height', graphHeightSize)
            .attr('fill', 'none')
            .attr('stroke', 'rgba(255, 0, 0, 0.2)')
            .attr('stroke-width', 2);
    }

    // Update graph offset
    graphOffset += graphHeightSize + 10;
};

const handleTestButtonClicked = () => {
    webui.call('GetBirdSyntaxTree')
        .then(res => {
            console.log('GetBirdSyntaxTree');
            console.log(res);
            clearGraph();
            console.log('Graph Cleared');
            createGraph(res);
            console.log('Graph Created');
        });
}

const handleFileSelectClicked = () => {
    webui.call('LoadSyntaxTreeFromFile')
        .then(res => {
            console.log('LoadSyntaxTreeFromFile');
            console.log(res);
            if (res === '') return;
            clearGraph();
            createGraph(res);
        });
}

const handleDirectorySelectClicked = () => {
    webui.call('LoadSyntaxTreeFromDirectory')
        .then(fileNameJson => {
            console.log('LoadSyntaxTreeFromDirectory');
            console.log(fileNameJson);
            if (fileNameJson === '') return;
            clearGraph();
            let fileNames = JSON.parse(fileNameJson);
            fileNames.forEach(fileName => {
                webui.call('GetSyntaxTreeWithFileName', fileName)
                    .then(syntaxTreeJson => {
                        console.log('GetSyntaxTreeWithFileName');
                        console.log(syntaxTreeJson);
                        createGraph(syntaxTreeJson, fileName);
                    });
            });
        });
}

const handleDirectorySelectTrimmedClicked = () => {
    webui.call('LoadSyntaxTreeFromDirectory')
        .then(fileNameJson => {
            console.log('LoadSyntaxTreeFromDirectory');
            console.log(fileNameJson);
            if (fileNameJson === '') return;
            clearGraph();
            let fileNames = JSON.parse(fileNameJson);
            fileNames.forEach(fileName => {
                webui.call('GetSyntaxTreeWithFileNameTrimmed', fileName)
                    .then(syntaxTreeJson => {
                        console.log('GetSyntaxTreeWithFileName');
                        console.log(syntaxTreeJson);
                        createGraph(syntaxTreeJson, fileName);
                    });
            });
        });
}

const handleResizeEvent = () => {
    const svg = document.select('svg');
    svg.width = document.body.clientWidth;
    svg.height = document.body.clientHeight;
}

document.getElementById('test').addEventListener('click', () => handleTestButtonClicked());
document.getElementById('select-file').addEventListener('click', (e) => handleFileSelectClicked());
document.getElementById('select-directory').addEventListener('click', (e) => handleDirectorySelectClicked());
document.getElementById('select-directory-trimmed').addEventListener('click', (e) => handleDirectorySelectTrimmedClicked());
window.addEventListener('resize', () => handleResizeEvent());
window.addEventListener('keydown', function (event) { if (event.key === 'Escape') { window.close(); } });