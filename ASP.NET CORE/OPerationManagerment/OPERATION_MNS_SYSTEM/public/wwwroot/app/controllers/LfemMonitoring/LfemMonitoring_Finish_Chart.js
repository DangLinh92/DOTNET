var LfemMonitoring_Finish_Chart = function () {
    const legenMargin = {
        id: 'legenMargin',
        beforeInit: function (chart, legend, options) {
            const fitValue = chart.legend.fit;
            chart.legend.fit = function fit() {
                fitValue.bind(chart.legend)();
                return this.height += 15;
            }
        },
        afterDatasetsDraw: function (chart, args, options) {
            const { ctx, scales: { x, y } } = chart;

            chart.data.datasets[0].data.forEach((datapoint, index) => {

                const datasetArray = [];
                chart.data.datasets.forEach((dataset) => {

                    if (dataset.type == 'bar') {
                        datasetArray.push(dataset.data[index]);
                    }
                })

                function totalSum(total, values) {
                    return total + values;
                };

                let sum = datasetArray.reduce(totalSum, 0);

                ctx.font = 'bold 11px sans-serif';
                ctx.fillStyle = '#004e89';
                ctx.textAlign = 'center';
                ctx.fillText(sum.toFixed(1), x.getPixelForValue(index), chart.getDatasetMeta(0).data[index].y - 32);

            })
        }
    };

    const legenMargin2 = {
        id: 'legenMargin2',
        beforeInit: function (chart, legend, options) {
            const fitValue = chart.legend.fit;
            chart.legend.fit = function fit() {
                fitValue.bind(chart.legend)();
                return this.height += 15;
            }
        },
        afterDatasetsDraw: function (chart, args, options) {
            const { ctx, scales: { x, y } } = chart;

            chart.data.datasets[0].data.forEach((datapoint, index) => {

                const datasetArray = [];
                chart.data.datasets.forEach((dataset) => {

                    if (dataset.type == 'bar') {
                        datasetArray.push(dataset.data[index]);
                    }
                })

                function totalSum(total, values) {
                    return total + Number(values);
                };

                let sum = datasetArray.reduce(totalSum, 0);

                ctx.font = 'bold 11px sans-serif';
                ctx.fillStyle = '#004e89';
                ctx.textAlign = 'center';

                ctx.fillText(sum.toLocaleString("en-US"), x.getPixelForValue(index), chart.getDatasetMeta(0).data[index].y - 20);
            })
        }
    };

    const legenMargin3 = {
        id: 'legenMargin3',
        beforeInit: function (chart, legend, options) {
            const fitValue = chart.legend.fit;
            chart.legend.fit = function fit() {
                fitValue.bind(chart.legend)();
                return this.height += 15;
            }
        },
        afterDatasetsDraw: function (chart, args, options) {
            const { ctx, scales: { x, y } } = chart;

            chart.data.datasets[0].data.forEach((datapoint, index) => {

                const datasetArray = [];
                chart.data.datasets.forEach((dataset) => {

                    if (dataset.type == 'bar') {
                        datasetArray.push(dataset.data[index]);
                    }
                })

                function totalSum(total, values) {
                    return total + values;
                };

                let sum = datasetArray.reduce(totalSum, 0);

                ctx.font = 'bold 11px sans-serif';
                ctx.fillStyle = '#004e89';
                ctx.textAlign = 'center';

                let _y = chart.getDatasetMeta(0).data[index].y > chart.getDatasetMeta(1).data[index].y ? chart.getDatasetMeta(1).data[index].y : chart.getDatasetMeta(0).data[index].y;

                ctx.fillText(sum.toFixed(1), x.getPixelForValue(index), _y - 20);

            })
        }
    };

    var arrLeadTimeChart = [];

    this.DrawChart = function () {

        for (d of arrLeadTimeChart) {
            d.destroy();
        }

        arrLeadTimeChart = [];

        // ve bieu do VOC theo thang
        var arrColor = ['#44c4fa', "#CB4335"];
        let modelLabel = [];

        for (d of StayDayLfemItems) {
            modelLabel.push(d.Model);
        }

        // Stay day by model
        let ctx_lfem_model = document.getElementById('chartStayDayByModel').getContext('2d');

        let dic_model = new Object();
        var _dataValue_stayday = [];
        var _dataValue_lotID = [];

        for (pl of StayDayLfemItems) {
            _dataValue_stayday.push(pl.StayDay.toFixed().toLocaleString("en-US"));
            _dataValue_lotID.push(pl.LotIDCount.toFixed().toLocaleString("en-US"));
        }

        dic_model['Lot ID'] = _dataValue_lotID;
        dic_model['Stay Day'] = _dataValue_stayday;

        let objDatasets_model = [];
        let h = 0;
        for (let key in dic_model) {
            let obj = {
                label: key,
                data: dic_model[key],
                backgroundColor: arrColor[h],
                borderColor: arrColor[h],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: key == 'Stay Day' ? 'line' : 'bar',
                yAxisID: (key == 'Stay Day') ? 'y1' : 'y'
            }

            if (obj.label != 'Stay Day') {
                obj.datalabels =
                {
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: false
                }
            }
            else {
                obj.datalabels =
                {
                    anchor: 'end',
                    align: 'top',
                    color: '#CB4335',
                    display: true,
                    Offset: 40
                }
            }

            objDatasets_model.push(obj);
            h += 1;
        }

        let modelChart_lfem = new Chart(ctx_lfem_model, {
            type: 'bar',
            data: {
                labels: modelLabel,
                datasets: objDatasets_model
            },
            plugins: [ChartDataLabels, legenMargin2],
            options: {
                /* indexAxis: 'y',*/
                plugins: {
                    // Change options for ALL labels of THIS CHART
                    //datalabels: {
                    //    anchor: 'start',
                    //    align: 'top',
                    //    color: '#ffff'
                    //},
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
                        position: 'left',
                        ticks: {
                            beginAtZero: true,
                            fontSize: 11
                        },
                        grid: {
                            display: false
                        }
                    }
                    ,
                    y1: {
                        stacked: false,
                        position: 'right',
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
        arrLeadTimeChart.push(modelChart_lfem);
    }
}