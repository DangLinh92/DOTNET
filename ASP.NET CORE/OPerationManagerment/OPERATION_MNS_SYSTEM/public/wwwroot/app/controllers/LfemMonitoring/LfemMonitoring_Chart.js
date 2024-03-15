var LfemMonitoring_Chart = function () {
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

        let sizeLabel = [];
        let operationLabel = [];
        let modelLabel = [];

        for (d of StayDayLfemItems) {
            operationLabel.push(d.OperationName);
        }

        for (d of StayDayLfemItems1) {
            modelLabel.push(d.Model);
        }

        for (w of StayDayLfemItems2) {
            sizeLabel.push(w.Size);
        }

        // 공정별 정체 재공현황 stay day by operation
        let ctx6 = document.getElementById('chartStayDayByOperation').getContext('2d');

        let dic = new Object();
        var dataValue_LotID = [];
        var dataValue_StayDay = [];

        for (pl of StayDayLfemItems) {
            dataValue_LotID.push(pl.LotIDCount.toFixed().toLocaleString("en-US"));
            dataValue_StayDay.push(pl.StayDay.toFixed().toLocaleString("en-US"));
        }

        dic['Lot ID'] = dataValue_LotID;
        dic['Stay Day'] = dataValue_StayDay;

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
                type: (key == 'Stay Day') ? 'line' : 'bar',
                yAxisID: (key == 'Stay Day') ? 'y1' : 'y'
            }

            if (obj.label != 'Stay Day') {
                obj.datalabels =
                {
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: false,

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

            objDatasets.push(obj);
            i += 1;
        }

        let byOperationChart = new Chart(ctx6, {
            type: 'bar',
            data: {
                labels: operationLabel,
                datasets: objDatasets
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

                        ticks: {
                            beginAtZero: true,
                            fontSize: 11
                        },
                        grid: {
                            display: false
                        },
                        position: 'left'
                    },
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

        arrLeadTimeChart.push(byOperationChart);

        // Stay day by model
        let ctx_lfem_model = document.getElementById('chartStayDayByModel').getContext('2d');

        let dic_model = new Object();
        var _dataValue_stayday = [];
        var _dataValue_lotID = [];

        for (pl of StayDayLfemItems1) {
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

        // stay day by size
        let ctx_size = document.getElementById('chartStayDayBySize').getContext('2d');

        let dic_size = new Object();
        var dataValue_lotID_size = [];
        var dataValue_stayDay_size = [];

        for (pl of StayDayLfemItems2) {
            dataValue_lotID_size.push(pl.LotIDCount.toFixed().toLocaleString("en-US"));
            dataValue_stayDay_size.push(pl.StayDay.toFixed().toLocaleString("en-US"));
        }

        dic_size['Lot ID'] = dataValue_lotID_size;
        dic_size['Stay Day'] = dataValue_stayDay_size;

        let objDatasets_size = [];
        let u = 0;
        for (let key in dic_size) {
            let obj = {
                label: key,
                data: dic_size[key],
                backgroundColor: arrColor[u],
                borderColor: arrColor[u],
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

            objDatasets_size.push(obj);
            u += 1;
        }

        let sizeChart = new Chart(ctx_size, {
            type: 'bar',
            data: {
                labels: sizeLabel,
                datasets: objDatasets_size
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
        arrLeadTimeChart.push(sizeChart);
    }
}