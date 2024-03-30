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
    // Select the SVG container 
    const zoomGroup = select('#zoom-group');
    const contentGroup = zoomGroup.append('g')
        .attr('transform', `translate(0, ${graphOffset})`);

    // Parse the data to create a hierarchy and then compute the layout
    const root = hierarchy(JSON.parse(data));
    const maxDepth = root.height;

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

    const treeLayout = tree().size([graphHeightSize, graphWidthSize]);
    treeLayout(root);

    // Define a generator for the links (lines between nodes)
    const linkPathGenerator = linkHorizontal()
        .x(node => node.y)
        .y(node => node.x);

    // Draw the links (paths) between nodes
    contentGroup.selectAll('path')
        .data(root.links())
        .enter().append('path')
        .attr('d', linkPathGenerator);

    // Add text element for token kind
    contentGroup.selectAll('text.tokenKind')
        .data(root.descendants())
        .enter().append('text')
        .attr('class', 'tokenKind')
        .attr('x', node => node.y)
        .attr('y', node => node.x)
        .attr('dy', '1.3em')
        .attr('text-anchor', 'middle')
        .attr('fill', 'rgba(0, 0, 0, 0.4)')
        .style('font-size', '.7em')
        .text(node => node.data.syntaxData.tokenKind);

    // Draw the nodes display name as text elements
    contentGroup.selectAll('text.displayName')
        .data(root.descendants())
        .enter().append('text')
        .attr('class', 'displayName')
        .attr('x', node => node.y)
        .attr('y', node => node.x)
        .attr('dy', '0.32em')
        .attr('text-anchor', 'middle')
        .text(node => node.data.syntaxData.displayName);


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
            createGraph(res);
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

const handleResizeEvent = () => {
    const svg = document.select('svg');
    svg.width = document.body.clientWidth;
    svg.height = document.body.clientHeight;
}

document.getElementById('test').addEventListener('click', () => handleTestButtonClicked());
document.getElementById('select-file').addEventListener('click', (e) => handleFileSelectClicked());
document.getElementById('select-directory').addEventListener('click', (e) => handleDirectorySelectClicked());
window.addEventListener('resize', () => handleResizeEvent());
window.addEventListener('keydown', function (event) { if (event.key === 'Escape') { window.close(); } });