var Goc_Chart = function () {
    const legenMargin = {
        id: 'legendMargin',
        beforeInit: function (chart, legend, options) {
            const fitValue = chart.legend.fit;
            chart.legend.fit = function fit() {
                fitValue.bind(chart.legend)();
                return this.height += 15;
            }
        }
    };


    var arrLeadTimeChart = [];

    this.DrawChart = function () {

        for (d of arrLeadTimeChart) {
            d.destroy();
        }

        arrLeadTimeChart = [];

        // ve bieu do GOC by day
        var arrColor = ['#CB4335', '#004e89',"#9D3C72"];
        let dataLabel = [];

        for (let i = 1; i <= DayOfMonth; i++) {
            dataLabel.push(i +'일');
        }

        let ctx6 = document.getElementById('chartsLine_wlp_byDay').getContext('2d');

        let dic = new Object();
        var dataValue_PlanYtd = [];
        var dataValue_ActualYtd = [];
        var dataValue_PercenYtd = [];

        for (pl of QuantityByDays)
        {
            dataValue_PlanYtd.push(Math.round(pl.QtyPlan_Ytd/1000));
            dataValue_ActualYtd.push(Math.round(pl.QtyActual_Ytd / 1000));
            dataValue_PercenYtd.push(pl.Qty_Percen_Ytd);
        }

        dic['Prod. Plan YTD'] = dataValue_PlanYtd;
        dic['Prod. Actual YTD'] = dataValue_ActualYtd;
       /* dic['% YTD'] = dataValue_PercenYtd;*/

        let objDatasets = [];
        let i = 0;
        for (let key in dic) {
            let obj = {
                label: key,
                data: dic[key],
                backgroundColor: arrColor[i],
                borderColor: arrColor[i],
               /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: 'line' 
            }

            if (i == 0) {
                obj.datalabels =
                {
                    anchor: 'end',
                    align: 'start',
                    display: 'auto',
                    color: 'black',
                    offset: 6
                }
            }
            else {
                obj.datalabels =
                {
                    anchor: 'start',
                    align: 'end',
                    display: 'auto',
                    color: 'black',
                   offset:8
                }
            }
            obj.yAxisID = 'y';

            //if (obj.label == "% YTD") {
            //    obj.yAxisID = 'y1';
            //}
            //else {
            //    obj.yAxisID = 'y';
            //}

            objDatasets.push(obj);
            i += 1;
        }

        let monthChart = new Chart(ctx6, {
            type: 'line',
            data: {
                labels: dataLabel,
                datasets: objDatasets
            },
            plugins: [ChartDataLabels, legenMargin],
            options: {
                /* indexAxis: 'y',*/
                plugins: {
                    // Change options for ALL labels of THIS CHART
                    datalabels: {
                        formatter: function (value, context) {
                            return value.toLocaleString("en-US");
                        }
                    },
                    legend: {
                        display: true,
                        position: 'top',
                        align: 'start'
                    },
                    title: {
                        display: true,
                        position: 'top',
                        align: 'end',
                        text: 'Yield : ' + dataValue_PercenYtd.slice(-1) + '%',
                        font: {
                            size: 14
                        },
                        color: '#CB4335'
                    }
                },
                maintainAspectRatio: false,
                //legend: {
                //    display: true,
                //    labels: {
                //        display: true
                //    }
                //},
                responsive: true,
                interaction: {
                    mode: 'index',
                    intersect: false,
                },
                scales: {
                    x: {
                        stacked: false,
                        ticks: {
                            beginAtZero: true,
                            fontSize: 11
                        },
                        grid: {
                            display: false
                        }
                    },
                    y: {
                        stacked: false,
                        ticks: {
                            beginAtZero: true,
                            fontSize: 11
                        },
                        grid: {
                            display: false
                        },
                        position: 'left',
                    },
                    //y1: {
                    //    position: 'right',
                    //    ticks: {
                    //        beginAtZero: true,
                    //        fontSize: 11
                    //    },
                    //    grid: {
                    //        display: false
                    //    },
                    //    title: {
                    //        display: true,
                    //        text: '% YTD'
                    //    },
                    //}
                }
            }
        });

        arrLeadTimeChart.push(monthChart);
    }

}