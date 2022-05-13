
function test(datas) {
    LINECHART = document.getElementById('LineChart');
    var example = JSON.parse(datas);
    Plotly.newPlot(LINECHART, example);
};

