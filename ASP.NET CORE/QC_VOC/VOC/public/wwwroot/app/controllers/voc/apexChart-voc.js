$(document).ready(function () {

    // draw chart for year
    var dataSeries = [];
    for (cls of chartdataTotalInit["PartsClassification"]) {
        let obj = new Object();
        let dataItem = [];
        for (voc of chartdataTotalInit["totalVOCSiteModelItems"]) {
            if (cls == voc["Classification"]) {
                dataItem.push(voc.Qty);
            }
        }
        obj["name"] = cls;
        obj["data"] = dataItem;
        dataSeries.push(obj);
    }

    var sColStacked = {
        chart: {
            height: 250,
            type: 'bar',
            stacked: true,
            toolbar: {
                show: false,
            }
        },
        // colors: ['#4361ee', '#888ea8', '#e3e4eb', '#d3d3d3'],
        responsive: [{
            breakpoint: 200,
            options: {
                legend: {
                    position: 'bottom',
                    offsetX: -10,
                    offsetY: 0
                }
            }
        }],
        plotOptions: {
            bar: {
                horizontal: false,
            },
        },
        series: dataSeries,
        xaxis: {
            /*            type: 'datetime',*/
            categories: chartdataTotalInit["Divisions"],
        },
        legend: {
            position: 'right',
            offsetY: 20
        },
        fill: {
            opacity: 1
        },
    }

    var chart = new ApexCharts(
        document.querySelector("#s-col-stacked"),
        sColStacked
    );

    chart.render();

    //-------------------------------------------

    // draw char for voc count finished
    for (fVoc of chartdataVocCountFinished) {

        let fdataSeries = [];
        for (cls of fVoc["PartsClassifications"]) {
            let obj = new Object();
            let dataItem = [];
            for (voc of fVoc["vOCSiteModelByTimes"]) {
                if (cls == voc["Classification"]) {
                    dataItem.push(voc.Qty);
                }
            }
            obj["name"] = cls;
            obj["data"] = dataItem;
            fdataSeries.push(obj);
        }

        let fsColStacked = {
            chart: {
                height: 250,
                type: 'bar',
                stacked: true,
                toolbar: {
                    show: false,
                }
            },
            colors: ['#2ECC71', '#FF7F50'],
            responsive: [{
                breakpoint: 200,
                options: {
                    legend: {
                        position: 'bottom',
                        offsetX: -10,
                        offsetY: 0
                    }
                }
            }],
            plotOptions: {
                bar: {
                    horizontal: false,
                },
            },
            series: fdataSeries,
            xaxis: {
                /*            type: 'datetime',*/
                categories: fVoc["TimeHeader"],
            },
            legend: {
                position: 'right',
                offsetY: 20
            },
            fill: {
                opacity: 1
            },
        }

        let fchart = new ApexCharts(
            document.querySelector("#s-col-stacked-Count_WHC"),
            fsColStacked
        );

        fchart.render();
    }
});