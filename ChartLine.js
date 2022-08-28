
function test(datas,xaxisname,yaxisname) {
    LINECHART = document.getElementById('LineChart');
    var example = JSON.parse(datas);
    var layout = {
        showlegend: true,
        legend: { "orientation": "h" },
        xaxis: {
            title: {
                text: xaxisname,
                font: {
                    size: 18,
                    color: '#7f7f7f',
                    
                    
                },
               
            },
        },
        yaxis: {
            title: {
                text: yaxisname,
                font: { 
                    size: 18,
                    color: '#7f7f7f'
                }
            }
        }
    };
    Plotly.newPlot(LINECHART, example, layout);
};

