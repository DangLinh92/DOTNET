var apexChartTotalYearController = function () {

    var charApex = new Object();

    this.DrawChart = function () {

        if (typeof charApex.destroy === "function") {
            charApex.destroy();
            charApex = new Object();
        }

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
            colors: ['#44c4fa', '#ffab00'],
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
                position: 'top',
                horizontalAlign: 'left',
                offsetY: 0
            },
            grid: {
                show: false,
            },
            fill: {
                opacity: 1
            },
        }

         charApex = new ApexCharts(
            document.querySelector("#s-col-stacked"),
            sColStacked
        );

        charApex.render();

        console.log(typeof (charApex));
    }
}