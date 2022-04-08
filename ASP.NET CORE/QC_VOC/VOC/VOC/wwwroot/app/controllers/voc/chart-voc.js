/** chart.js **/
$(function () {
    /** STACKED BAR CHART **/

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

    // ve bieu do VOC theo thang
    var arrColor = ['#44c4fa', '#ffab00']
    for (voc of chartdataInit) {

        let ctx6 = document.getElementById('chartstacked' + voc.DivisionLst).getContext('2d');
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

        new Chart(ctx6, {
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

        new Chart(ctx6, {
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
    }

    // ve bieu do line PPM
    /* LINE CHART */

    let lineColor = ['#CB4335', '#F1C40F'];
    let ctx7 = document.getElementById('chartLine_All');
    let _mlabels = ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'];
    let datasetall = [];
    let _index = 0;
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

    new Chart(ctx7, {
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
                    text: 'K1 고객 불량 (월별 목표 대비 실적) - Unit [PPM]'
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


    for (ppm of chardataPPM.dataChartsItem) {

        for (m of ppm) {

            let ctx8 = document.getElementById('chartLine_' + m.Customer + m.Module);
            let mlabels = ['Total 년', '1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월']
            new Chart(ctx8, {
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
                            text: m.Customer + ' Customer GMES Data Trend ' + m.Year + '(' + m.Module + ') - Unit [PPM]'
                        },
                        subtitle: {
                            display: true,
                            text: 'Unit[PPM]'
                        },
                        legend: {
                            display: true,
                            align:'start',
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
        }

    }

});