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
});