var chartjsPPM_Controller = function () {

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

    var arrPPMChart = [];
    this.DrawChart = function (_module) {

        for (c of arrPPMChart) {
            c.destroy();
        }
        arrPPMChart = [];

        // ve bieu do line PPM
        /* LINE CHART */

        let lineColor = ['#CB4335', '#F1C40F'];
        let ctx7 = document.getElementById('chartLine_All');
        let _mlabels = ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'];
        let datasetall = [];
        let _index = 0;

        if (chardataPPM == undefined) {
            return;
        }

        for (m of chardataPPM.dataChartsAll) {

            if (_index == 0) {
                _mlabels.splice(0, 0, m.Year.toString().substring(2, 4) + '년');
                _mlabels.splice(0, 0, (m.Year - 1).toString().substring(2, 4) + '년');
                _mlabels.splice(0, 0, (m.Year - 2).toString().substring(2, 4) + '년');
                _mlabels.splice(0, 0, (m.Year - 3).toString().substring(2, 4) + '년');
            }

            let obj = {
                label: m.Module + ' 실적',
                data: m.lstData,
                borderWidth: 1.2,
                fill: false,
                borderColor: lineColor[_index],
                datalabels: {
                    display: 'auto',
                    anchor: 'end',
                    align: 'top',
                    offset: 5,
                    color: '#566573',
                    labels: {
                        title: {
                            font: {
                                weight: 'bold'
                            }
                        }
                    }
                }
            };

            let targetObj = {
                label: m.Module + ' target',
                data: m.dataTargetAll,
                borderWidth: 1,
                fill: false,
                borderColor: '#44c4fa',
                datalabels: {
                    display: 'auto',
                    anchor: 'end',
                    align: 'top',
                    offset: 5,
                    color: '#2E86C1'
                }
            };

            datasetall.push(obj);
            datasetall.push(targetObj);
            _index += 1;
        }

        let ctx7Chart = new Chart(ctx7, {
            type: 'line',
            data: {
                labels: _mlabels,
                datasets: datasetall
            },
            plugins: [ChartDataLabels, legenMargin],
            options: {
                plugins: {
                    title: {
                        display: true,
                        text: 'K1 고객 불량 (월별 목표 대비 실적)'
                    },
                    subtitle: {
                        display: true,
                        text: 'Unit[PPM]'
                    },
                    legend: {
                        display: true,
                        align: 'start',
                        labels: {
                            display: true,
                            usePointStyle: true,
                            pointStyle: 'line'
                        }
                    },
                },
                maintainAspectRatio: false,

                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true,
                            fontSize: 10,
                        }
                    }],
                    xAxes: [{
                        ticks: {
                            beginAtZero: true,
                            fontSize: 11
                        }
                    }]
                }
            }
        });

        arrPPMChart.push(ctx7Chart);

        var _i = 0;
        for (ppm of chardataPPM.dataChartsItem) {

            for (m of ppm) {

                if (m.Module == _module) {
                    _i = _i + 1;
                    let elId = 'chartLine_' + _i;
                    if (_module == 'LFEM') {
                        elId = 'chartLine_lfem_' + _i;
                    }
                    let ctx8 = document.getElementById(elId);
                    let mlabels = ['Total 년', '1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월']
                    let ctx8Chart = new Chart(ctx8, {
                        type: 'line',
                        data: {
                            labels: mlabels,
                            datasets: [{
                                label: m.Module + ' 실적',
                                data: m.lstData,
                                borderColor: '#EC7063',
                                borderWidth: 1.2,
                                fill: false,
                                datalabels: {
                                    display: 'auto',
                                    anchor: 'end',
                                    align: 'top',
                                    offset: 5,
                                    color: '#566573',
                                    labels: {
                                        title: {
                                            font: {
                                                weight: 'bold'
                                            }
                                        }
                                    }
                                }
                            },
                            {
                                label: m.Year.toString().substring(2, 4) + '년target',
                                data: m.dataTargetAll,
                                borderColor: '#44c4fa',
                                borderWidth: 1,
                                fill: false,
                                datalabels: {
                                    display: 'auto',
                                    anchor: 'end',
                                    align: 'top',
                                    offset: 5,
                                    color: '#2E86C1'
                                }
                            }]
                        },
                        plugins: [ChartDataLabels, legenMargin],
                        options: {
                            plugins: {
                                title: {
                                    display: true,
                                    text: m.Customer + ' Customer GMES Data Trend ' + m.Year + '(' + m.Module + ')'
                                },
                                subtitle: {
                                    display: true,
                                    text: 'Unit[PPM]'
                                },
                                legend: {
                                    display: true,
                                    align: 'start',
                                    labels: {
                                        display: true,
                                        usePointStyle: true,
                                        pointStyle: 'line',
                                    }
                                }
                            },
                            maintainAspectRatio: false,

                            scales: {
                                yAxes: [{
                                    ticks: {
                                        beginAtZero: true,
                                        fontSize: 10,
                                    }
                                }],
                                xAxes: [{
                                    ticks: {
                                        beginAtZero: true,
                                        fontSize: 11
                                    }
                                }]
                            }
                        }
                    });

                    arrPPMChart.push(ctx8Chart);
                }
            }

        }

    }
}