var Wlp2_ActualPlan_Chart = function () {
    const legenMargin = {
        id: 'legendMargin',
        beforeInit: function (chart, legend, options) {
            const fitValue = chart.legend.fit;
            chart.legend.fit = function fit() {
                fitValue.bind(chart.legend)();
                return this.height += 15;
            }
        }
        //,
        //afterDatasetsDraw: function (chart, args, options) {
        //    const { ctx, scales: { x, y } } = chart;

        //    chart.data.datasets[0].data.forEach((datapoint, index) => {

        //        const datasetArray = [];
        //        chart.data.datasets.forEach((dataset) => {

        //            if (dataset.type == 'bar') {
        //                datasetArray.push(dataset.data[index]);
        //            }
        //        })

        //        function totalSum(total, values) {
        //            return total + values;
        //        };

        //        let sum = datasetArray.reduce(totalSum, 0);

        //        ctx.font = 'bold 11px sans-serif';
        //        ctx.fillStyle = '#004e89';
        //        ctx.textAlign = 'center';
        //        ctx.fillText(sum.toFixed(1), x.getPixelForValue(index), chart.getDatasetMeta(1).data[index].y - 13);

        //    })
        //}
    };


    var arrActualPlan_Chart = [];

    this.DrawChart = function (dataRaw) {

        for (d of arrActualPlan_Chart) {
            d.destroy();
        }

        arrActualPlan_Chart = [];

        var arrColor = ['#44c4fa', '#ffab00', "#CB4335", "#004E7C"];
        let dataLabel_Input = [];

        let dataPlan_Input = [];
        let dataActual_Input = [];
        let dataGap_Input = [];

        for (d of dataRaw) {
            if (d.QuantityPlan > 0 || d.QuantityActual > 0) {
                dataLabel_Input.push(d.Model);

                dataPlan_Input.push((d.QuantityPlan / 1000).toFixed(1));
                dataActual_Input.push((d.QuantityActual / 1000).toFixed(1));
                dataGap_Input.push((d.QuantityGap / 1000).toFixed(1));
            }
        }

        // NHAP_KHO
        let ctx6 = document.getElementById('chartstacked_wlp_Input').getContext('2d');

        let dic_Input = new Object();

        dic_Input['Plan'] = dataPlan_Input;
        dic_Input['Actual'] = dataActual_Input;
        dic_Input['Gap'] = dataGap_Input;

        let objDatasets_Input = [];
        let i = 0;
        for (let key in dic_Input) {
            let obj = {
                label: key,
                data: dic_Input[key],
                backgroundColor: arrColor[i],
                borderColor: arrColor[i],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: (key == 'Gap') ? 'line' : 'bar'
            }

            if (obj.label != 'Gap') {
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
                    anchor: 'start',
                    align: 'start',
                    color: '#004e89',
                    display: true
                }
            }

            objDatasets_Input.push(obj);
            i += 1;
        }
        console.log(objDatasets_Input);

        let wlp_InputChart = new Chart(ctx6, {
            type: 'bar',
            data: {
                labels: dataLabel_Input,
                datasets: objDatasets_Input
            },
            plugins: [ChartDataLabels, legenMargin],
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
                        ticks:
                        {
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
                        }
                    }
                }
            }
        });

        arrActualPlan_Chart.push(wlp_InputChart);

        //const containerBodyInput = document.querySelector('.containerBody');
        //const totalLabels = wlp_InputChart.data.labels.length;

        //if (totalLabels > 10) {
        //    const newWidth = 500 + ((totalLabels-10) * 20);
        //    containerBodyInput.style.width = `${newWidth}px`;
        //}


        // WLP2_ San Xuat
        //let ctx_wlp2 = document.getElementById('chartstacked_wlp_Production').getContext('2d');

        //let dic_Production = new Object();

        //dic_Production['Plan'] = dataPlan_Production;
        //dic_Production['Actual'] = dataActual_Production;
        //dic_Production['Gap'] = dataGap_Production;

        //let objDatasets1 = [];
        //let j = 0;
        //for (let key in dic_Production) {
        //    let obj = {
        //        label: key,
        //        data: dic_Production[key],
        //        backgroundColor: arrColor[j],
        //        borderColor: arrColor[j],
        //        /* borderWidth: 0.5,*/
        //        /*fill: true,*/
        //        type: key == 'Gap' ? 'line' : 'bar'
        //    }

        //    if (obj.label != 'Gap') {
        //        obj.datalabels =
        //        {
        //            anchor: 'center',
        //            align: 'center',
        //            color: '#ffff',
        //            display: false
        //        }
        //    }
        //    else {
        //        obj.datalabels =
        //        {
        //            anchor: 'start',
        //            align: 'start',
        //            color: '#004e89',
        //            display: true
        //        }
        //    }

        //    objDatasets1.push(obj);
        //    j += 1;
        //}

        //let wlp_ProductionChart = new Chart(ctx_wlp2, {
        //    type: 'bar',
        //    data: {
        //        labels: dataLabel_Production,
        //        datasets: objDatasets1
        //    },
        //    plugins: [ChartDataLabels, legenMargin],
        //    options: {
        //        /* indexAxis: 'y',*/
        //        plugins: {
        //            // Change options for ALL labels of THIS CHART
        //            //datalabels: {
        //            //    anchor: 'start',
        //            //    align: 'top',
        //            //    color: '#ffff'
        //            //},
        //            legend: {
        //                display: true,
        //                position: 'top',
        //                align: 'start'
        //            }
        //        },
        //        maintainAspectRatio: false,
        //        //legend: {
        //        //    display: true,
        //        //    labels: {
        //        //        display: true
        //        //    }
        //        //},
        //        responsive: true,
        //        scales: {
        //            x: {
        //                stacked: false,
        //                ticks: {
        //                    fontSize: 11
        //                },
        //                grid: {
        //                    display: false
        //                }
        //            },
        //            y: {
        //                stacked: false,
        //                ticks: {
        //                    beginAtZero: true,
        //                    fontSize: 11
        //                },
        //                grid: {
        //                    display: false
        //                }
        //            }
        //        }
        //    }
        //});
        //arrActualPlan_Chart.push(wlp_ProductionChart);

        //// WLP Delivery

        //let ctx_wlp_month = document.getElementById('chartstacked_wlp_delivery').getContext('2d');

        //let dic_Delivery = new Object();

        //dic_Delivery['Plan'] = dataPlan_Delivery;
        //dic_Delivery['Actual'] = dataActual_Delivery;
        //dic_Delivery['Gap'] = dataGap_Delivery;

        //let objDatasets_Delivery = [];
        //let k = 0;
        //for (let key in dic_Delivery) {
        //    let obj = {
        //        label: key,
        //        data: dic_Delivery[key],
        //        backgroundColor: arrColor[k],
        //        borderColor: arrColor[k],
        //        /* borderWidth: 0.5,*/
        //        /*fill: true,*/
        //        type: key == 'Gap' ? 'line' : 'bar'
        //    }

        //    if (obj.label != 'Gap') {
        //        obj.datalabels =
        //        {
        //            anchor: 'center',
        //            align: 'center',
        //            color: '#ffff',
        //            display: false
        //        }
        //    }
        //    else {
        //        obj.datalabels =
        //        {
        //            anchor: 'start',
        //            align: 'start',
        //            color: '#004e89',
        //            display: true
        //        }
        //    }

        //    objDatasets_Delivery.push(obj);
        //    k += 1;
        //}

        //let monthChart_Delivery = new Chart(ctx_wlp_month, {
        //    type: 'bar',
        //    data: {
        //        labels: dataLabel_Delivery,
        //        datasets: objDatasets_Delivery
        //    },
        //    plugins: [ChartDataLabels, legenMargin],
        //    options: {
        //        /* indexAxis: 'y',*/
        //        plugins: {
        //            // Change options for ALL labels of THIS CHART
        //            //datalabels: {
        //            //    anchor: 'start',
        //            //    align: 'top',
        //            //    color: '#ffff'
        //            //},
        //            legend: {
        //                display: true,
        //                position: 'top',
        //                align: 'start'
        //            }
        //        },
        //        maintainAspectRatio: false,
        //        //legend: {
        //        //    display: true,
        //        //    labels: {
        //        //        display: true
        //        //    }
        //        //},
        //        responsive: true,
        //        scales: {
        //            x: {
        //                stacked: false,
        //                ticks: {
        //                    fontSize: 11
        //                },
        //                grid: {
        //                    display: false
        //                }
        //            },
        //            y: {
        //                stacked: false,
        //                ticks: {
        //                    beginAtZero: true,
        //                    fontSize: 11
        //                },
        //                grid: {
        //                    display: false
        //                }
        //            }
        //        }
        //    }
        //});
        //arrActualPlan_Chart.push(monthChart_Delivery);

    }

}