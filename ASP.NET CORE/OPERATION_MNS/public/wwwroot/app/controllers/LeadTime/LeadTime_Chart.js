var LeadTimeChart = function () {
    const legenMargin = {
        id: 'legendMargin',
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
                ctx.fillText(sum.toFixed(1),x.getPixelForValue(index),chart.getDatasetMeta(1).data[index].y -  13);

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
        let dataLabel = ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'];
        let weeksLabel = [];
        let dayLabel1 = [];
        let dayLabel2 = [];

        for (d of Days1) {
            dayLabel1.push(d);
        }

        for (d of Days2) {
            dayLabel2.push(d);
        }

        for (w of WeeksLabel) {
            weeksLabel.push(w);
        }

        // WLP1_ Lead time theo tháng
        let ctx6 = document.getElementById('chartstacked_wlp1_leadtimeby_Month').getContext('2d');

        let dic = new Object();
        var dataValue_runtime = [];
        var dataValue_waittime = [];
        var dataValue_target = [];

        for (pl of WLP1_LeadTimeByMonth)
        {
            dataValue_runtime.push(pl.Value_runtime);
            dataValue_waittime.push(pl.Value_waittime);
            dataValue_target.push(pl.Value_target);
        }

        dic['Wait Time'] = dataValue_waittime;
        dic['Run Time'] = dataValue_runtime;
        dic['Target'] = dataValue_target;

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
                type: (key == 'Target') ? 'line' : 'bar'
            }

            if (obj.label != 'Target') {
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


        // WLP2_ Lead time theo tháng
        let ctx_wlp2 = document.getElementById('chartstacked_wlp2_leadtimeby_Month').getContext('2d');

        let dic1 = new Object();
        var dataValue_runtime1 = [];
        var dataValue_waittime1 = [];
        var dataValue_target1 = [];

        for (pl of WLP2_LeadTimeByMonth) {
            dataValue_runtime1.push(pl.Value_runtime);
            dataValue_waittime1.push(pl.Value_waittime);
            dataValue_target1.push(pl.Value_target);
        }

        dic1['Wait Time'] = dataValue_waittime1;
        dic1['Run Time'] = dataValue_runtime1;
        dic1['Target'] = dataValue_target1;

        let objDatasets1 = [];
        let j = 0;
        for (let key in dic1)
        {
            let obj = {
                label: key,
                data: dic1[key],
                backgroundColor: arrColor[j],
                borderColor: arrColor[j],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: key == 'Target' ? 'line' : 'bar'
            }

            if (obj.label != 'Target') {
                obj.datalabels =
                {
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: true
                }
            }
            else
            {
                obj.datalabels =
                {
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: false
                }
            }

            objDatasets1.push(obj);
            j += 1;
        }

        let monthChart1 = new Chart(ctx_wlp2, {
            type: 'bar',
            data: {
                labels: dataLabel,
                datasets: objDatasets1
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
        arrLeadTimeChart.push(monthChart1);

        // WLP_Lead time theo năm ( Đơn vị: ngày)

        let ctx_wlp = document.getElementById('chartstacked_wlp_byYear').getContext('2d');

        let objDatasets2 = [];
        if (WLP_LeadTimeByYear.length > 0) {
            var objData = WLP_LeadTimeByYear[0];
            let _obj = {
                label: 'Wait Time',
                data: [objData.Value_waittime],
                backgroundColor: arrColor[0],
                borderColor: arrColor[0],
                datalabels: {
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: true
                },
                type: 'bar'
            }

            let _obj1 = {
                label: 'Run Time',
                data: [objData.Value_runtime],
                backgroundColor: arrColor[1],
                borderColor: arrColor[1],
                datalabels: {
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: true
                },
                type: 'bar'
            }

            let _obj2 = {
                label: 'Target',
                data: [objData.Value_target],
                backgroundColor: arrColor[2],
                borderColor: arrColor[2],
                datalabels: {
                    anchor: 'center',
                    align: 'center',
                    color: '#ffff',
                    display: false
                },
                type: 'line'
            }
            objDatasets2.push(_obj);
            objDatasets2.push(_obj1);
            objDatasets2.push(_obj2);

            let yearChart = new Chart(ctx_wlp, {
                type: 'bar',
                data: {
                    labels: [objData.Label_x],
                    datasets: objDatasets2
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
            arrLeadTimeChart.push(yearChart);
        }

        // WLP_ Lead time tổng theo tháng (Đơn vị:  ngày)

        let ctx_wlp_month = document.getElementById('chartstacked_WLP_LeadTimeByMonth').getContext('2d');

        let dic_wlpMonth = new Object();
        var dataValue_runtime_wlpMonth = [];
        var dataValue_waittime_wlpMonth = [];
        var dataValue_target_wlpMonth = [];

        for (pl of WLP_LeadTimeByMonth)
        {
            dataValue_runtime_wlpMonth.push(pl.Value_runtime);
            dataValue_waittime_wlpMonth.push(pl.Value_waittime);
            dataValue_target_wlpMonth.push(pl.Value_target);
        }

        dic_wlpMonth['Wait Time'] = dataValue_waittime_wlpMonth;
        dic_wlpMonth['Run Time'] = dataValue_runtime_wlpMonth;
        dic_wlpMonth['Target'] = dataValue_target_wlpMonth;

        let objDatasets_wlpMonth = [];
        let k = 0;
        for (let key in dic_wlpMonth) {
            let obj = {
                label: key,
                data: dic_wlpMonth[key],
                backgroundColor: arrColor[k],
                borderColor: arrColor[k],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: key == 'Target' ? 'line' : 'bar'
            }

            if (obj.label != 'Target') {
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

            objDatasets_wlpMonth.push(obj);
            k += 1;
        }

        let monthChart_wlp = new Chart(ctx_wlp_month, {
            type: 'bar',
            data: {
                labels: dataLabel,
                datasets: objDatasets_wlpMonth
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
        arrLeadTimeChart.push(monthChart_wlp);

        // WLP_ Lead time tổng theo tuần ( Đơn vị: ngày)

        let ctx_wlp_week = document.getElementById('chartstacked_WLP_LeadTimeByWeek').getContext('2d');

        let dic_wlpWeek = new Object();
        var dataValue_runtime_wlpWeek = [];
        var dataValue_waittime_wlpWeek = [];
        var dataValue_target_wlpWeek = [];

        for (pl of WLP_LeadTimeByWeek) {
            dataValue_runtime_wlpWeek.push(pl.Value_runtime);
            dataValue_waittime_wlpWeek.push(pl.Value_waittime);
            dataValue_target_wlpWeek.push(pl.Value_target);
        }

        dic_wlpWeek['Wait Time'] = dataValue_waittime_wlpWeek;
        dic_wlpWeek['Run Time'] = dataValue_runtime_wlpWeek;
        dic_wlpWeek['Target'] = dataValue_target_wlpWeek;

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
                type: key == 'Target' ? 'line' : 'bar'
            }

            if (obj.label != 'Target') {
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

        let weekChart_wlp = new Chart(ctx_wlp_week, {
            type: 'bar',
            data: {
                labels: weeksLabel,
                datasets: objDatasets_wlpWeek
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


        // WLP1_ Lead time theo tuần ( Đơn vị: ngày)

        let ctx_wlp1_week = document.getElementById('chartstacked_WLP1_LeadTimeByWeek').getContext('2d');

        let dic_wlp1Week = new Object();
        var dataValue_runtime_wlp1Week = [];
        var dataValue_waittime_wlp1Week = [];
        var dataValue_target_wlp1Week = [];

        for (pl of WLP1_LeadTimeByWeek) {
            dataValue_runtime_wlp1Week.push(pl.Value_runtime);
            dataValue_waittime_wlp1Week.push(pl.Value_waittime);
            dataValue_target_wlp1Week.push(pl.Value_target);
        }

        dic_wlp1Week['Wait Time'] = dataValue_waittime_wlp1Week;
        dic_wlp1Week['Run Time'] = dataValue_runtime_wlp1Week;
        dic_wlp1Week['Target'] = dataValue_target_wlp1Week;

        let objDatasets_wlp1Week = [];
        let m = 0;
        for (let key in dic_wlp1Week) {
            let obj = {
                label: key,
                data: dic_wlp1Week[key],
                backgroundColor: arrColor[m],
                borderColor: arrColor[m],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: key == 'Target' ? 'line' : 'bar'
            }

            if (obj.label != 'Target') {
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

            objDatasets_wlp1Week.push(obj);
            m += 1;
        }

        let weekChart_wlp1 = new Chart(ctx_wlp1_week, {
            type: 'bar',
            data: {
                labels: weeksLabel,
                datasets: objDatasets_wlp1Week
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
        arrLeadTimeChart.push(weekChart_wlp1);

        // WLP2_ Lead time theo tuần ( Đơn vị: ngày)
        let ctx_wlp2_week = document.getElementById('chartstacked_WLP2_LeadTimeByWeek').getContext('2d');

        let dic_wlp2Week = new Object();
        var dataValue_runtime_wlp2Week = [];
        var dataValue_waittime_wlp2Week = [];
        var dataValue_target_wlp2Week = [];

        for (pl of WLP2_LeadTimeByWeek) {
            dataValue_runtime_wlp2Week.push(pl.Value_runtime);
            dataValue_waittime_wlp2Week.push(pl.Value_waittime);
            dataValue_target_wlp2Week.push(pl.Value_target);
        }

        dic_wlp2Week['Wait Time'] = dataValue_waittime_wlp2Week;
        dic_wlp2Week['Run Time'] = dataValue_runtime_wlp2Week;
        dic_wlp2Week['Target'] = dataValue_target_wlp2Week;

        let objDatasets_wlp2Week = [];
        let n = 0;
        for (let key in dic_wlp2Week) {
            let obj = {
                label: key,
                data: dic_wlp2Week[key],
                backgroundColor: arrColor[n],
                borderColor: arrColor[n],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: key == 'Target' ? 'line' : 'bar'
            }

            if (obj.label != 'Target') {
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

            objDatasets_wlp2Week.push(obj);
            n += 1;
        }

        let weekChart_wlp2 = new Chart(ctx_wlp2_week, {
            type: 'bar',
            data: {
                labels: weeksLabel,
                datasets: objDatasets_wlp2Week
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
        arrLeadTimeChart.push(weekChart_wlp2);



        // WLP1_ Lead time theo ngày ( Đơn vị: ngày)
        let ctx_wlp1_day = document.getElementById('chartstacked_WLP1_LeadTimeByday').getContext('2d');

        let dic_wlp1day = new Object();
        var dataValue_runtime_wlp1day = [];
        var dataValue_waittime_wlp1day = [];
        var dataValue_target_wlp1day = [];

        for (pl of WLP1_LeadTimeByDay) {
            dataValue_runtime_wlp1day.push(pl.Value_runtime);
            dataValue_waittime_wlp1day.push(pl.Value_waittime);
            dataValue_target_wlp1day.push(pl.Value_target);
        }

        dic_wlp1day['Wait Time'] = dataValue_waittime_wlp1day;
        dic_wlp1day['Run Time'] = dataValue_runtime_wlp1day;
        dic_wlp1day['Target'] = dataValue_target_wlp1day;

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
                type: key == 'Target' ? 'line' : 'bar'
            }

            if (obj.label != 'Target') {
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


        // WLP2_ Lead time theo ngày ( Đơn vị: ngày)
        let ctx_wlp2_day = document.getElementById('chartstacked_WLP2_LeadTimeByday').getContext('2d');

        let dic_wlp2day = new Object();
        var dataValue_runtime_wlp2day = [];
        var dataValue_waittime_wlp2day = [];
        var dataValue_target_wlp2day = [];

        for (pl of WLP2_LeadTimeByDay) {
            dataValue_runtime_wlp2day.push(pl.Value_runtime);
            dataValue_waittime_wlp2day.push(pl.Value_waittime);
            dataValue_target_wlp2day.push(pl.Value_target);
        }

        dic_wlp2day['Wait Time'] = dataValue_waittime_wlp2day;
        dic_wlp2day['Run Time'] = dataValue_runtime_wlp2day;
        dic_wlp2day['Target'] = dataValue_target_wlp2day;

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
                type: key == 'Target' ? 'line' : 'bar'
            }

            if (obj.label != 'Target') {
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

            objDatasets_wlp2day.push(obj);
            v += 1;
        }

        let dayChart_wlp2 = new Chart(ctx_wlp2_day, {
            type: 'bar',
            data: {
                labels: dayLabel2,
                datasets: objDatasets_wlp2day
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
        arrLeadTimeChart.push(dayChart_wlp2);
    }

}