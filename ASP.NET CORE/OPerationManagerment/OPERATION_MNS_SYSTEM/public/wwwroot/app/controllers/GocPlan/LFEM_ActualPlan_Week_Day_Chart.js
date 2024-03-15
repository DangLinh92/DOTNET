var LFEM_ActualPlan_Week_Chart = function () {
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

    var arrActualPlan_Chart = [];
    this.DrawChart = function () {

        for (d of arrActualPlan_Chart) {
            d.destroy();
        }

        arrActualPlan_Chart = [];

        var arrColor = ['#44c4fa', "#CB4335", "#004E7C"];
        let dataLabel_Input = [];

        let dataPlan_Input = [];
        let dataActual_Input = [];

        for (d of QuantityByDays) {
            if (d.QtyPlan_Ytd > 0 || d.QtyActual_Ytd > 0) {

                dataLabel_Input.push(d.DatePlan);
                dataPlan_Input.push((d.QtyPlan_Ytd / 1000).toFixed(0));
                dataActual_Input.push((d.QtyActual_Ytd / 1000).toFixed(0));
            }
        }

        // NHAP_KHO
        let ctx6 = document.getElementById('chartsLine_wlp_byWeek').getContext('2d');

        let dic_Input = new Object();

        dic_Input['YTD Plan'] = dataPlan_Input;
        dic_Input['YTD Actual'] = dataActual_Input;

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
                type: (key == 'YTD Actual') ? 'line' : 'bar'
            }

            if (obj.label != 'YTD Actual') {
                obj.datalabels =
                {
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: true
                }
            }
            else {
                obj.datalabels =
                {
                    anchor: 'end',
                    align: 'top',
                    color: '#CB4335',
                    offset: 20,
                    display: true
                }
            }

            objDatasets_Input.push(obj);
            i += 1;
        }

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
    }

}

var LFEM_ActualPlan_Day_Chart = function () {
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

    var arrActualPlan_Chart = [];
    this.DrawChart = function () {

        for (d of arrActualPlan_Chart) {
            d.destroy();
        }

        arrActualPlan_Chart = [];

        var arrColor = ['#44c4fa', '#ffab00', "#CB4335", "#004E7C"];
        let dataLabel_Input = [];

        let dataPlan_Input = [];
        let dataActual_Input = [];
        let dataGap_Input = [];

        console.log(QuantityByDays);

        for (d of QuantityByDays) {
            if (d.QuantityPlan > 0 || d.QuantityActual > 0) {

                dataLabel_Input.push(d.SapCode);
                dataPlan_Input.push((d.QuantityPlan / 1000).toFixed(0));
                dataActual_Input.push((d.QuantityActual / 1000).toFixed(0));
                dataGap_Input.push((d.QuantityGap / 1000).toFixed(0));
            }
        }

        let totalPlan = 0;
        let totalActual = 0;
        let Rate = 0;
        for (p of QuantityByDays) {
            totalPlan += p.QuantityPlan;
            totalActual += p.QuantityActual;
        }

        if (totalPlan > 0)
            Rate = 100 * totalActual / totalPlan;

        // NHAP_KHO
        let ctx6 = document.getElementById('chartsLine_lfem_byDay').getContext('2d');

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
                    display: true
                }
            }
            else {
                obj.datalabels =
                {
                    anchor: 'end',
                    align: 'top',
                    color: '#CB4335',
                    offset: 10,
                    display: true
                }
            }

            objDatasets_Input.push(obj);
            i += 1;
        }

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
                    },
                    title: {
                        display: true,
                        position: 'top',
                        align: 'end',
                        text: 'Total [ Plan: ' + (totalPlan / 1000).toLocaleString("en-US") + ' \n Actual: ' + (totalActual / 1000).toLocaleString("en-US") + '\n Rate : ' + Rate.toFixed(0) + '% ]',
                        font: {
                            size: 14
                        },
                        color: '#CB4335'
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
    }

}