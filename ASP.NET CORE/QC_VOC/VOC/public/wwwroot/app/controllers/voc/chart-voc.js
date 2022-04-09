var chartVoc = function () {
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

    var arrMonthChart = [];
    var arrDefectChart = [];

    this.DrawChart = function () {

        for (c of arrMonthChart) {
            c.destroy();
        }

        for (d of arrDefectChart) {
            d.destroy();
        }

        arrMonthChart = [];
        arrDefectChart = [];

        // ve bieu do VOC theo thang
        var arrColor = ['#44c4fa', '#ffab00']
        for (voc of chartdataInit) {

            let ctx6 = document.getElementById('chartstacked_ByYear').getContext('2d');
            let dataLabel = [voc.TimeHeader[0], '1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'];

            let dic = new Object();
            for (pl of voc.PartsClassifications) {

                let dataValue = [];
                for (item of voc.vOCSiteModelByTimes) {
                    if (item.Classification == pl) {
                        dataValue.push(item.Qty);
                    }
                }
                dic[pl] = dataValue;
            }

            let objDatasets = [];
            let i = 0;
            for (let key in dic) {
                let obj = {
                    label: key,
                    data: dic[key],
                    backgroundColor: arrColor[i],
                    borderWidth: 0.5,
                    fill: true
                }
                objDatasets.push(obj);
                i += 1;
            }

            let monthChart = new Chart(ctx6, {
                type: 'bar',
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
                            anchor: 'start',
                            align: 'top',
                            color: '#ffff'
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
                            stacked: true,
                            ticks: {
                                fontSize: 11
                            },
                            grid: {
                                display: false
                            }
                        },
                        y: {
                            stacked: true,
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

            arrMonthChart.push(monthChart);
        }

        // ----------------------------------------------

        // ve bieu do pareto theo defect
        var indexSaw = 0;
        var bgColor;
        for (voc of chartdataByDefectName) {

            let chartId = indexSaw == 0 ? 'chartBar_Defect_SAW' : 'chartBar_Defect_Module';
            bgColor = indexSaw++ == 0 ? ['#44c4fa'] : ['#ffab00']
            let ctx6 = document.getElementById(chartId).getContext('2d');;

            let dataLabel = voc.PartsClassification;
            let dataDefect = [];

            for (d of voc.totalVOCSiteModelItems) {
                dataDefect.push(d.Qty);
            }

            let objDatasets = [];
            let obj = {
                label: 'Defect name (Analysis result)',
                data: dataDefect,
                backgroundColor: bgColor,
                borderWidth: 1,
                fill: true
            }
            objDatasets.push(obj);

            let defectChart = new Chart(ctx6, {
                type: 'bar',
                data: {
                    labels: dataLabel,
                    datasets: objDatasets
                },
                plugins: [ChartDataLabels, legenMargin],
                options: {
                    indexAxis: 'x',
                    plugins: {
                        // Change options for ALL labels of THIS CHART
                        datalabels: {
                            color: '#ffff'
                        },
                        legend: {
                            display: true,
                            position: 'top',
                            align: 'start'
                        }
                    },
                    maintainAspectRatio: false,
                    legend: {
                        display: true,
                        labels: {
                            display: true
                        }
                    },
                    scales: {
                        y: {
                            stacked: true,
                            ticks: {
                                beginAtZero: true,
                                fontSize: 11
                            },
                            grid: {
                                display: false
                            }
                        },
                        x: {
                            stacked: true,
                            ticks: {
                                fontSize: 11
                            },
                            grid: {
                                display: false
                            }
                        }
                    }
                }
            });

            arrDefectChart.push(defectChart);
        }
    }
}