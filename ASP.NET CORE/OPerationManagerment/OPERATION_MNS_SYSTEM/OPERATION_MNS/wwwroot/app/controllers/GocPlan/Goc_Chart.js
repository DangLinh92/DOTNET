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


        // wafer
        let ctx7 = document.getElementById('chartsLine_wlp_byDay_wafer').getContext('2d');

        let dic_wf = new Object();
        var dataValue_PlanYtd_wf = [];
        var dataValue_ActualYtd_wf = [];
        var dataValue_PercenYtd_wf = [];

        for (pl of QuantityByDays_WF) {
            dataValue_PlanYtd_wf.push(Math.round(pl.QtyPlan_Ytd));
            dataValue_ActualYtd_wf.push(Math.round(pl.QtyActual_Ytd));
            dataValue_PercenYtd_wf.push(pl.Qty_Percen_Ytd);
        }

        dic_wf['Prod. Plan YTD'] = dataValue_PlanYtd_wf;
        dic_wf['Prod. Actual YTD'] = dataValue_ActualYtd_wf;
        /*dic_wf['% YTD'] = dataValue_PercenYtd_wf;*/

        let objDatasets_wf = [];
        let j = 0;
        for (let key in dic_wf) {
            let obj = {
                label: key,
                data: dic_wf[key],
                backgroundColor: arrColor[j],
                borderColor: arrColor[j],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: 'line'
            }

            if (j== 0) {
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
                    offset: 8
                }
            }
            obj.yAxisID = 'y';
            //if (obj.label == "% YTD") {
            //    obj.yAxisID = 'y1';
            //}
            //else {
            //    obj.yAxisID = 'y';
            //}


            objDatasets_wf.push(obj);
            j += 1;
        }

        let monthChart_wf = new Chart(ctx7, {
            type: 'line',
            data: {
                labels: dataLabel,
                datasets: objDatasets_wf
            },
            plugins: [ChartDataLabels, legenMargin],
            options: {
                /* indexAxis: 'y',*/
                plugins: {
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
                        text: 'Yield : ' + dataValue_PercenYtd_wf.slice(-1) + '%',
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
                            fontSize: 11
                        },
                        grid: {
                            display: false
                        }
                    },
                    y: {
                        position: 'left',
                        stacked: false,
                        ticks: {
                            beginAtZero: true,
                            fontSize: 11
                        },
                        grid: {
                            display: false
                        }
                    }
                    ,
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

        arrLeadTimeChart.push(monthChart_wf);

        // FAB
        let ctx8 = document.getElementById('chartsLine_wlp_byDay_fab').getContext('2d');

        let dic_fab = new Object();
        var dataValue_PlanYtd_fab = [];
        var dataValue_ActualYtd_fab = [];

        for (pl of QuantityByDays_FAB) {
            dataValue_PlanYtd_fab.push(Math.round(pl.QtyPlan_Ytd));
            dataValue_ActualYtd_fab.push(Math.round(pl.QtyActual_Ytd));
        }

        dic_fab['FAB. Plan YTD'] = dataValue_PlanYtd_fab;
        dic_fab['FAB. Actual YTD'] = dataValue_ActualYtd_fab;

        let objDatasets_fab = [];
        let k = 0;
        for (let key in dic_fab) {
            let obj = {
                label: key,
                data: dic_fab[key],
                backgroundColor: arrColor[k],
                borderColor: arrColor[k],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: 'line'
            }

            if (k == 0) {
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
                    offset: 8
                }
            }


            objDatasets_fab.push(obj);
            k += 1;
        }

        let monthChart_FAB = new Chart(ctx8, {
            type: 'line',
            data: {
                labels: dataLabel,
                datasets: objDatasets_fab
            },
            plugins: [ChartDataLabels, legenMargin],
            options: {
                /* indexAxis: 'y',*/
                plugins: {
                    datalabels: {
                        formatter: function (value, context) {
                            return value.toLocaleString("en-US");
                        }
                    },
                    legend: {
                        display: true,
                        position: 'top',
                        align: 'start'
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
                scales: {
                    x: {
                        stacked: false,
                        ticks: {
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
                        }
                    }
                }
            }
        });

        arrLeadTimeChart.push(monthChart_FAB);
    }

}