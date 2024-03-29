import { select, tree, hierarchy, linkHorizontal, zoom, zoomIdentity } from 'https://cdn.jsdelivr.net/npm/d3@7/+esm';

const createGraph = (data) => {
    // Select the SVG container and clear any existing content
    const svg = select('svg');
    svg.selectAll('*').remove();

    // Set the dimensions of the graph
    const width = document.body.clientWidth;
    const height = document.body.clientHeight;

    // Create a group element (`g`) that will contain the tree
    // This group will also handle the zoom and pan features
    const zoomG = svg.attr('width', width)
        .attr('height', height)
        .append('g');

    // Create another group inside `zoomG` to apply the transformations
    const contentGroup = zoomG.append('g');

    // Define the zoom behavior and apply it to the SVG
    const zoomBehavior = zoom().on('zoom', (event) => {
        contentGroup.attr('transform', event.transform);
    });
    svg.call(zoomBehavior);

    // Reset the zoom and pan to the initial state
    svg.call(zoomBehavior.transform, zoomIdentity);

    // Parse the data to create a hierarchy and then compute the layout
    const root = hierarchy(JSON.parse(data));

    const maxDepth = (node) => {
        if (!node.children || node.children.length === 0) {
            return 0;
        }
        return 1 + Math.max(...node.children.map(maxDepth));
    };

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
    const treeLayout = tree().size([maxBreadth(root) * 50, maxDepth(root) * 150]);
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

    // Draw the nodes as text elements
    contentGroup.selectAll('text')
        .data(root.descendants())
        .enter().append('text')
        .attr('x', node => node.y)
        .attr('y', node => node.x)
        .attr('dy', '0.32em')
        .attr('text-anchor', 'middle')
        .text(node => node.data.data.id);
};

const handleTestButtonClicked = () => {
    webui.call('GetBirdSyntaxTree')
        .then(res => {
            console.log(res);
            createGraph(res);
        });
}

const handleFileSelectClicked = () => {
    webui.call('LoadSyntaxTreeFromFile')
        .then(res => {
            console.log(res);
            createGraph(res);
        });
}

const handleDirectorySelectClicked = () => {
    webui.call('LoadSyntaxTreeFromDirectory')
        .then(res => {
            console.log(res);
            createGraph(res);
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