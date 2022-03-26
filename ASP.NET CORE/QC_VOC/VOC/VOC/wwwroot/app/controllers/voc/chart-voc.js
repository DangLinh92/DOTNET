$(function () {
    /** STACKED BAR CHART **/

    var arrColor = ['#664dc9', '#44c4fa', '#38cb89', '#3e80eb', '#ffab00', '#ef4b4b']
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
            options: {
                maintainAspectRatio: true,
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