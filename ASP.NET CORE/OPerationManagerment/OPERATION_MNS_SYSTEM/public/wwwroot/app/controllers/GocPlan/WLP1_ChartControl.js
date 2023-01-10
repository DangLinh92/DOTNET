var WLP1_ChartControl = function () {
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


    var arrLeadTimeChart = [];

    this.DrawChart = function () {

        for (d of arrLeadTimeChart) {
            d.destroy();
        }

        arrLeadTimeChart = [];

        // ve bieu do GOC by day
        var arrColor = ['#DC0000', '#205295', "#205295", "#DC0000", "#CFF5E7", "#F07DEA"];
        let dataLabel = [];

        for (d of chartData) {
            dataLabel.push(d.LOT_ID);
        }

        let ctx6 = document.getElementById('chartsLine_control').getContext('2d');

        let dic = new Object();
        var data_Usl = [];
        var data_UCL = [];
        var data_LCL = [];
        var data_LSL = [];
        var data_Target = [];
        var data_Avg = [];

        for (pl of chartData)
        {
            if (pl.MAIN_TARGET_USL != 0)
            {
                data_Usl.push(pl.MAIN_TARGET_USL);
            }

            if (pl.MAIN_TARGET_UCL != 0) {
                data_UCL.push(pl.MAIN_TARGET_UCL);
            }

            if (pl.MAIN_TARGET_LCL != 0) {
                data_LCL.push(pl.MAIN_TARGET_LCL);
            }
            
            if (pl.MAIN_TARGET_LSL != 0) {
                data_LSL.push(pl.MAIN_TARGET_LSL);
            }

            if (pl.MAIN_TARGET != 0) {
                data_Target.push(pl.MAIN_TARGET);
            }
          
            data_Avg.push(pl.MAIN_AVG_VALUE);
        }

        dic['USL'] = data_Usl;
        dic['UCL'] = data_UCL;
        dic['LCL'] = data_LCL;
        dic['LSL'] = data_LSL;
        dic['Target'] = data_Target;
        dic['Avg'] = data_Avg;

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
                type: 'line' 
            }

            if (obj.label == "Avg" || obj.label == "Target") {
                obj.fill = false;
                obj.borderDash = [5, 5]
            }
            else {
                obj.pointStyle = 'line';
            }

            if (obj.data.length > 0)
            {
                objDatasets.push(obj);
            }
           
            i += 1;
        }

        let ControlChart = new Chart(ctx6, {
            type: 'line',
            data: {
                labels: dataLabel,
                datasets: objDatasets
            },
            plugins: [ChartDataLabels, legenMargin],
            options: {
                /* indexAxis: 'y',*/
                responsive: true,
                plugins: {
                    // Change options for ALL labels of THIS CHART
                    datalabels: {
                        formatter: function (value, context) {
                            return value.toLocaleString("en-US");
                        },
                        display: false
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
                interaction: {
                    mode: 'index',
                    intersect: false,
                },
                scales: {
                    x: {
                        stacked: false,
                        ticks: {
                            fontSize: 11
                        },
                        grid: {
                            display: false
                        },
                        display: false,
                        title: {
                            display: true,
                            text: 'LOT ID'
                        },
                        min: 0,
                        max:100
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
                        title: {
                            display: true,
                            text: 'AVERAGE'
                        },
                    }
                }
            }
        });

        arrLeadTimeChart.push(ControlChart);

        // barchart
        let dataLabel_2 = [];
        let dataSetChar = [];

        function AvggroupBy(array, f) {
            var groups = {};
            array.forEach(function (o) {
                var group = JSON.stringify(f(o));
                groups[group] = groups[group] || [];
                groups[group].push(o);
            });
            return Object.keys(groups).map(function (group) {
                return groups[group];
            })
        };

        var resultData = AvggroupBy(chartData, function (item) {
            return [item.MAIN_AVG_VALUE];
        });
      
        resultData.sort((x, y) => { return x[0].MAIN_AVG_VALUE - y[0].MAIN_AVG_VALUE });
        console.log(resultData);

        for (arr of resultData) {

            dataLabel_2.push(arr[0].MAIN_AVG_VALUE);
            dataSetChar.push(arr.length);
        }

        console.log(dataLabel_2);
        console.log(dataSetChar);

        var ctx1 = document.getElementById('chartsColumn_control').getContext('2d');
        var ControlColumnChart = new Chart(ctx1, {
            type: 'bar',
            data: {
                labels: dataLabel_2,
                datasets: [{
                    label: '측정값 산포',
                    data: dataSetChar,
                    backgroundColor: '#44c4fa'
                }]
            },
            plugins: [ChartDataLabels, legenMargin],
            options: {
                plugins: {
                    datalabels: {
                        formatter: function (value, context) {
                            return value.toLocaleString("en-US");
                        },
                        anchor: 'end',
                        align: 'end',
                        display: 'true',
                        color: '#205295',
                        offset: 4,
                        font: {
                            weight: 'bold'
                        }
                    }
                },
                maintainAspectRatio: false,
                responsive: true,
                legend: {
                    display: false,
                    labels: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        barPercentage: 0.6,
                        ticks: {
                            beginAtZero: true,
                            fontSize: 11
                        },
                        grid: {
                            display: false
                        },
                        title: {
                            display: true,
                            text: 'MATERIAL_ID 개'
                        },
                    },
                    x: {
                        barPercentage: 0.6,
                        ticks: {
                            beginAtZero: true,
                            fontSize: 11
                        },
                        grid: {
                            display: false
                        },
                        title: {
                            display: false,
                            text: '측정값 산포'
                        },
                    }
                }
            }
        });

        arrLeadTimeChart.push(ControlColumnChart);

        function scroller(scroll,chart) {
            console.log(scroll);

            const dataLenght = chart.data.labels.length;
            console.log(dataLenght);
            console.log(chart.config.options.scales.x);

            if (scroll.deltaY > 0) {
                console.log('next');
                if (chart.config.options.scales.x.max >= dataLenght) {
                    chart.config.options.scales.x.min = dataLenght - 101;
                    chart.config.options.scales.x.max = dataLenght;
                }
                else {
                    chart.config.options.scales.x.min += 100;
                    chart.config.options.scales.x.max += 100;
                }
            }
            else if (scroll.deltaY < 0) {
                console.log('pre');
                if (chart.config.options.scales.x.min <= 0) {
                    chart.config.options.scales.x.min = 0;
                    chart.config.options.scales.x.max = 100;
                }
                else {
                    chart.config.options.scales.x.min -= 100;
                    chart.config.options.scales.x.max -= 100;
                 
                }
               
            }
            chart.update();
        }

        //ControlChart.canvas.addEventListener('wheel', (e) => {
        //    scroller(e, ControlChart);
        //});

        $('#btnNext').on('click', function () {
            const dataLenght = ControlChart.data.labels.length;
            if (ControlChart.config.options.scales.x.max >= dataLenght) {
                ControlChart.config.options.scales.x.min = dataLenght - 101;
                ControlChart.config.options.scales.x.max = dataLenght;
            }
            else {
                ControlChart.config.options.scales.x.min += 30;
                ControlChart.config.options.scales.x.max += 30;

                if (ControlChart.config.options.scales.x.max > dataLenght) {
                    ControlChart.config.options.scales.x.max = dataLenght;
                }
            }
            console.log(ControlChart.config.options.scales.x.min, ControlChart.config.options.scales.x.max);
            $('#txtItems').text("Items: " + ControlChart.config.options.scales.x.max + "/" + dataLenght);
            ControlChart.update();
        });

        $('#btnPre').on('click', function () {

            if (ControlChart.config.options.scales.x.min <= 0) {
                ControlChart.config.options.scales.x.min = 0;
                ControlChart.config.options.scales.x.max = 100;
            }
            else {

                ControlChart.config.options.scales.x.min -= 30;
                ControlChart.config.options.scales.x.max -= 30;

                if (ControlChart.config.options.scales.x.min < 0) {
                    ControlChart.config.options.scales.x.min = 0;
                }
            }
            console.log(ControlChart.config.options.scales.x.min, ControlChart.config.options.scales.x.max);
            const dataLenght = ControlChart.data.labels.length;
            $('#txtItems').text("Items: " + ControlChart.config.options.scales.x.min + "/" + dataLenght);
            ControlChart.update();
        });

        function disableScroll() {
            // Get the current page scroll position
            scrollTop =
                window.pageYOffset || document.documentElement.scrollTop;
            scrollLeft =
                window.pageXOffset || document.documentElement.scrollLeft,

                // if any scroll is attempted,
                // set this to the previous value
                window.onscroll = function () {
                    window.scrollTo(scrollLeft, scrollTop);
                };
        }

        function enableScroll() {
            window.onscroll = function () { };
        }
    }

}