/** chart.js **/
$(function () {
    /** STACKED BAR CHART **/

    // ve bieu do VOC theo thang
    var arrColor = ['#44c4fa', '#ffab00']
    for (voc of chartdataInit) {

        let ctx6 = document.getElementById('chartstacked' + voc.DivisionLst);
        let dataLabel = voc.TimeHeader;

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
                borderWidth: 1,
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
            plugins: [ChartDataLabels],
            options: {
                /* indexAxis: 'y',*/
                plugins: {
                    // Change options for ALL labels of THIS CHART
                    datalabels: {
                        color: '#ffff'
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
                    yAxes: [{
                        stacked: true,
                        ticks: {
                            beginAtZero: true,
                            fontSize: 11
                        }
                    }],
                    xAxes: [{
                        barPercentage: 0.5,
                        stacked: true,
                        ticks: {
                            fontSize: 11
                        }
                    }]
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
            plugins: [ChartDataLabels],
            options: {
                indexAxis: 'y',
                plugins: {
                    // Change options for ALL labels of THIS CHART
                    datalabels: {
                        color: '#ffff'
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
                    yAxes: [{
                        stacked: true,
                        ticks: {
                            beginAtZero: true,
                            fontSize: 11
                        }
                    }],
                    xAxes: [{
                        barPercentage: 0.5,
                        stacked: true,
                        ticks: {
                            fontSize: 11
                        }
                    }]
                }
            }
        });
    }

    // ve bieu do line PPM
    /* LINE CHART */

    let lineColor = ['#664dc9', '#44c4fa', '#38cb89', '#3e80eb', '#ffab00', '#ef4b4b'];
    let ctx7 = document.getElementById('chartLine_All');
    let _mlabels = ['22年', '1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'];
    let datasetall = [];
    let _index = 0;
    for (m of chardataPPM.dataChartsAll) {

        let obj = {
            label: m.Module + ' 실적',
            data: m.lstData,
            borderWidth: 1.2,
            fill: false,
            borderColor: lineColor[_index],
        };

        let targetObj = {
            label: m.Module + ' 목표',
            data: [5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5],
            borderWidth: 1,
            fill: false,
            borderColor: lineColor[_index+2],
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
        plugins: [ChartDataLabels],
        options: {
            maintainAspectRatio: false,
            legend: {
                display: true,
                labels: {
                    display: true
                }
            },
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        fontSize: 10,
                        max: 80
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
                        label: 'Wisol',
                        data: m.lstData,
                        borderColor: '#EC7063',
                        borderWidth: 1.2,
                        fill: false,
                        datalabels: {
                            color: '#17202A',
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
                        label: 'Target',
                        data: [5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5],
                        borderColor: '#44c4fa',
                        borderWidth: 1,
                        fill: false,
                        datalabels: {
                            color: '#0000'
                        }
                    }]
                },
                plugins: [ChartDataLabels],
                options: {
                    maintainAspectRatio: false,
                    legend: {
                        display: true,
                        labels: {
                            display: true
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                fontSize: 10,
                                max: 80
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