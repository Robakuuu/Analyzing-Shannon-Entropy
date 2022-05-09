
function test(chart1, chart2) {
    LINECHART = document.getElementById('LineChart');
    var layout = {
        title: 'Line and Scatter Plot'
    };
    var data = [chart1,chart2];
    Plotly.newPlot(LINECHART, data, layout);
};

