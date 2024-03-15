var LeadTimeLFEMChart = function () {
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
                ctx.fillText(sum.toFixed(1),x.getPixelForValue(index),chart.getDatasetMeta(0).data[index].y -  32);

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
                    return total + values;
                };

                let sum = datasetArray.reduce(totalSum, 0);

                ctx.font = 'bold 11px sans-serif';
                ctx.fillStyle = '#004e89';
                ctx.textAlign = 'center';

               
                
                ctx.fillText(sum.toFixed(1), x.getPixelForValue(index), chart.getDatasetMeta(0).data[index].y - 20);

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

                ctx.fillText(sum.toFixed(1), x.getPixelForValue(index), _y-20);

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
        var arrColor = ['#44c4fa', '#ffab00', "#CB4335","#004E7C"];
        let dataLabel_Total = ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'];
        let dataLabel = [];
        let weeksLabel = [];
        let dayLabel1 = [];
        let operationLabel1 = [];
        let operationLabel2 = [];

        for (d of Days1) {
            dayLabel1.push(d);
        }

        for (d of Operation1) {
            operationLabel1.push(d);
        }

        for (d of Operation2) {
            operationLabel2.push(d);
        }

        for (w of WeeksLabel) {
            weeksLabel.push(w);
        }

        const dateNow = new Date();
        let year = dateNow.getFullYear();
        let month = dateNow.getMonth();
        if (Year == year) {
            for (let i = 0; i <= month; i++) {
                dataLabel.push(dataLabel_Total[i]);
            }
        }
        else {
            dataLabel = dataLabel_Total;
        }

        // LFEM_ Lead time theo tháng
        let ctx6 = document.getElementById('chartstacked_LFEM_LeadTimeByMonth').getContext('2d');

        let dic = new Object();
        var dataValue_runtime = [];
        var dataValue_waittime = [];
        var dataValue_leadtime = [];

        for (pl of LFEM_LeadTimeByMonth)
        {
            dataValue_runtime.push(pl.Value_runtime);
            dataValue_waittime.push(pl.Value_waittime);
            dataValue_leadtime.push(pl.Value_leadtime);
        }

        dic['Wait Time'] = dataValue_waittime;
        dic['Run Time'] = dataValue_runtime;
        dic['Lead Time'] = dataValue_leadtime;

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
                type: (key == 'Lead Time') ? 'line' : 'bar'
            }

            if (obj.label != 'Lead Time') {
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
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: false
                }
            }

            objDatasets.push(obj);
            i += 1;
        }
        console.log(objDatasets);
        let monthChart = new Chart(ctx6, {
            type: 'bar',
            data: {
                labels: dataLabel,
                datasets: objDatasets
            },
            plugins: [ChartDataLabels, legenMargin3],
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
                        stacked: true,
                        ticks: {
                            fontSize: 11
                        },
                        grid: {
                            display: false
                        }
                    },
                    y: {
                        stacked: true,
                       
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

        arrLeadTimeChart.push(monthChart);

        // Lead time tổng theo tuần ( Đơn vị: ngày)

        let ctx_lfem_week = document.getElementById('chartstacked_LFEM_LeadTimeByWeek').getContext('2d');

        let dic_wlpWeek = new Object();
        var dataValue_runtime_wlpWeek = [];
        var dataValue_waittime_wlpWeek = [];
        var dataValue_leadtime_wlpWeek = [];

        for (pl of LFEM_LeadTimeByWeek) {
            dataValue_runtime_wlpWeek.push(pl.Value_runtime);
            dataValue_waittime_wlpWeek.push(pl.Value_waittime);
            dataValue_leadtime_wlpWeek.push(pl.Value_leadtime);
        }

        dic_wlpWeek['Wait Time'] = dataValue_waittime_wlpWeek;
        dic_wlpWeek['Run Time'] = dataValue_runtime_wlpWeek;
        dic_wlpWeek['Lead Time'] = dataValue_leadtime_wlpWeek;

        let objDatasets_wlpWeek = [];
        let h = 0;
        for (let key in dic_wlpWeek) {
            let obj = {
                label: key,
                data: dic_wlpWeek[key],
                backgroundColor: arrColor[h],
                borderColor: arrColor[h],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: key == 'Lead Time' ? 'line' : 'bar'
            }

            if (obj.label != 'Lead Time') {
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
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: false
                }
            }

            objDatasets_wlpWeek.push(obj);
            h += 1;
        }

        let weekChart_wlp = new Chart(ctx_lfem_week, {
            type: 'bar',
            data: {
                labels: weeksLabel,
                datasets: objDatasets_wlpWeek
            },
            plugins: [ChartDataLabels, legenMargin3],
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
                        stacked: true,
                        ticks: {
                            fontSize: 11
                        },
                        grid: {
                            display: false
                        }
                    },
                    y: {
                        stacked: true,
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
        arrLeadTimeChart.push(weekChart_wlp);


        // Lead time theo ngày ( Đơn vị: ngày)
        let ctx_wlp1_day = document.getElementById('chartstacked_LFEM_leadtimeby_Day').getContext('2d');

        let dic_wlp1day = new Object();
        var dataValue_runtime_wlp1day = [];
        var dataValue_waittime_wlp1day = [];
        var dataValue_leadtime_wlp1day = [];

        for (pl of LFEM_LeadTimeByDay) {
            dataValue_runtime_wlp1day.push(pl.Value_runtime);
            dataValue_waittime_wlp1day.push(pl.Value_waittime);
            dataValue_leadtime_wlp1day.push(pl.Value_leadtime);
        }

        dic_wlp1day['Wait Time'] = dataValue_waittime_wlp1day;
        dic_wlp1day['Run Time'] = dataValue_runtime_wlp1day;
        dic_wlp1day['Lead Time'] = dataValue_leadtime_wlp1day;

        let objDatasets_wlp1day = [];
        let u = 0;
        for (let key in dic_wlp1day) {
            let obj = {
                label: key,
                data: dic_wlp1day[key],
                backgroundColor: arrColor[u],
                borderColor: arrColor[u],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: key == 'Lead Time' ? 'line' : 'bar'
            }

            if (obj.label != 'Lead Time') {
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
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: false
                }
            }

            objDatasets_wlp1day.push(obj);
            u += 1;
        }

        let dayChart_wlp1 = new Chart(ctx_wlp1_day, {
            type: 'bar',
            data: {
                labels: dayLabel1,
                datasets: objDatasets_wlp1day
            },
            plugins: [ChartDataLabels, legenMargin3],
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
                        stacked: true,
                        ticks: {
                            fontSize: 11
                        },
                        grid: {
                            display: false
                        }
                    },
                    y: {
                        stacked: true,
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
        arrLeadTimeChart.push(dayChart_wlp1);


        // Runtime theo operation
        let ctx_wlp2_day = document.getElementById('chartstacked_LFEM_Runtimeby_Day').getContext('2d');

        let dic_wlp2day = new Object();
        var dataValue_runtime= [];
        var dataValue_capa= [];

        for (pl of LFEM_RuntimeByOperation) {
            dataValue_runtime.push(pl.Value_runtime);
            dataValue_capa.push(pl.Value_target);
        }

        dic_wlp2day['Run Time'] = dataValue_runtime;
        dic_wlp2day['CAPA'] = dataValue_capa;

        let objDatasets_wlp2day = [];
        let v = 0;
        for (let key in dic_wlp2day) {
            let obj = {
                label: key,
                data: dic_wlp2day[key],
                backgroundColor: arrColor[v],
                borderColor: arrColor[v],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: key == 'CAPA' ? 'line' : 'bar',
                yAxisID: (key == 'CAPA') ? 'y1' : 'y'
            }

            if (obj.label != 'CAPA') {
                obj.datalabels =
                {
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: true
                }
            } else {
                obj.datalabels =
                {
                    anchor: 'end',
                    align: 'top',
                    color: '#CB4335',
                    display: true,
                    Offset: 40
                }
            }

            objDatasets_wlp2day.push(obj);
            v += 1;
        }

        let dayChart_wlp2 = new Chart(ctx_wlp2_day, {
            type: 'bar',
            data: {
                labels: operationLabel1,
                datasets: objDatasets_wlp2day
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
                        }
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
        arrLeadTimeChart.push(dayChart_wlp2);

        // run time (day)
        let ctx_lfem_day = document.getElementById('chartstacked_LFEM_Runtimeby_Day2').getContext('2d');

        let dic_lfemday = new Object();
        var dataValue_runtime2 = [];
        var dataValue_capa2 = [];

        for (pl of LFEM_RuntimeByOperation) {
            dataValue_runtime2.push(Math.round(pl.Value_runtime / 24 * 100) / 100);
            dataValue_capa2.push(pl.Value_target);
        }

        dic_lfemday['Run Time'] = dataValue_runtime2;
        dic_lfemday['CAPA'] = dataValue_capa2;

        let objDatasets_lfemday = [];
        let v1 = 0;
        for (let key in dic_lfemday) {
            let obj = {
                label: key,
                data: dic_lfemday[key],
                backgroundColor: arrColor[v1],
                borderColor: arrColor[v1],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: key == 'CAPA' ? 'line' : 'bar',
                yAxisID: (key == 'CAPA') ? 'y1' : 'y'
            }

            if (obj.label != 'CAPA') {
                obj.datalabels =
                {
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: true
                }
            } else {
                obj.datalabels =
                {
                    anchor: 'end',
                    align: 'top',
                    color: '#CB4335',
                    display: true,
                    Offset: 40
                }
            }

            objDatasets_lfemday.push(obj);
            v1 += 1;
        }

        let dayChart_runtimeday = new Chart(ctx_lfem_day, {
            type: 'bar',
            data: {
                labels: operationLabel1,
                datasets: objDatasets_lfemday
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
                        }
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
        arrLeadTimeChart.push(dayChart_runtimeday);


        // Wait time theo operation
        let ctx_waitime = document.getElementById('chartstacked_LFEM_Waittimeby_Day').getContext('2d');

        let dic_waitime = new Object();
        var dataValue_waitime = [];
        var dataValue_capa3 = [];

        for (pl of LFEM_WaitTimeByOperation) {
            dataValue_waitime.push(pl.Value_waittime);
            dataValue_capa3.push(pl.Value_target);
        }

        dic_waitime['Wait Time'] = dataValue_waitime;
        dic_waitime['CAPA'] = dataValue_capa3;

        let objDatasets_waitime = [];
        let vi = 0;
        for (let key in dic_waitime) {
            let obj = {
                label: key,
                data: dic_waitime[key],
                backgroundColor: arrColor[vi],
                borderColor: arrColor[vi],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: key == 'CAPA' ? 'line' : 'bar',
                yAxisID: (key == 'CAPA') ? 'y1' : 'y'
            }

            if (obj.label != 'CAPA') {
                obj.datalabels =
                {
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: true
                }
            } else {
                obj.datalabels =
                {
                    anchor: 'end',
                    align: 'top',
                    color: '#CB4335',
                    display: true,
                    Offset: 40
                }
            }

            objDatasets_waitime.push(obj);
            vi += 1;
        }

        let dayWaitime = new Chart(ctx_waitime, {
            type: 'bar',
            data: {
                labels: operationLabel2,
                datasets: objDatasets_waitime
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
                        }
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
        arrLeadTimeChart.push(dayWaitime);

        // waite time (day)
        let ctx_waitime2 = document.getElementById('chartstacked_LFEM_Waittimeby_Day2').getContext('2d');

        let dic_waitime2 = new Object();
        var dataValue_waitime2 = [];
        var dataValue_waitime4 = [];

        for (pl of LFEM_WaitTimeByOperation) {
            dataValue_waitime2.push(Math.round(pl.Value_waittime / 24 * 100) / 100);
            dataValue_waitime4.push(pl.Value_target);
        }

        dic_waitime2['Wait Time'] = dataValue_waitime2;
        dic_waitime2['CAPA'] = dataValue_waitime4;

        let objDatasets_waitime2 = [];
        let vi2 = 0;
        for (let key in dic_waitime2) {
            let obj = {
                label: key,
                data: dic_waitime2[key],
                backgroundColor: arrColor[vi2],
                borderColor: arrColor[vi2],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: key == 'CAPA' ? 'line' : 'bar',
                yAxisID: (key == 'CAPA') ? 'y1' : 'y'
            }

            if (obj.label != 'CAPA') {
                obj.datalabels =
                {
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: true
                }
            } else {
                obj.datalabels =
                {
                    anchor: 'end',
                    align: 'top',
                    color: '#CB4335',
                    display: true,
                    Offset: 40
                }
            }

            objDatasets_waitime2.push(obj);
            vi2 += 1;
        }

        let dayWaitime2 = new Chart(ctx_waitime2, {
            type: 'bar',
            data: {
                labels: operationLabel2,
                datasets: objDatasets_waitime2
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
                        }
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
        arrLeadTimeChart.push(dayWaitime2);
    }

}