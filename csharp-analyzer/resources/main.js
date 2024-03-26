import { select, tree, hierarchy, linkHorizontal, zoom } from 'https://cdn.jsdelivr.net/npm/d3@7/+esm';

const createGraph = (data) => {
    document.getElementById('graph')?.remove();
    document.body.insertAdjacentHTML('beforeend', '<svg id="graph"></svg>');
    const svg = select('svg');
    const width = document.body.clientWidth;
    const height = document.body.clientHeight;
    const margin = { top: 0, right: 50, bottom: 0, left: 0 };
    const innerWidth = width - margin.left - margin.right;
    const innerHeight = height - margin.top - margin.bottom;

    const worldTree = tree()
        .size([innerHeight, innerWidth]);

    const zoomG = svg.attr('width', width)
        .attr('height', height)
        .append('g')

    const g = zoomG.append('g')
        .attr('transform', `translate(${margin.left},${margin.top})`);

    svg.call(zoom().on('zoom', (e) => {
        zoomG.attr('transform', e.transform);
    }));

    const root = hierarchy(JSON.parse(data));
    const links = worldTree(root).links();
    const linkPathGenerator = linkHorizontal()
        .x(d => d.y)
        .y(d => d.x);

    g.selectAll('path').data(links)
        .enter().append('path')
        .attr('d', linkPathGenerator)

    g.selectAll('text').data(root.descendants())
        .enter().append('text')
        .attr('x', d => d.y)
        .attr('y', d => d.x)
        .attr('dy', '0.32em')
        .attr('text-anchor', 'middle')
        .text(d => d.data.data.id);
}

const handleTestButtonClicked = () => {
    webui.call('GetBirdSyntaxTree')
        .then(res => {
            console.log(res);
            createGraph(res);
        });
}

document.getElementById('test').addEventListener('click', () => handleTestButtonClicked());