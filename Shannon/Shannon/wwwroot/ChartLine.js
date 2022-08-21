
function test(datas) {
    LINECHART = document.getElementById('LineChart');
    var example = JSON.parse(datas);
    var layout = {
        showlegend: true,
        legend: { "orientation": "h" }
    };
    Plotly.newPlot(LINECHART, example, layout);
};

