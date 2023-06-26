var SampleLeadTimeChart = function () {
    const legenMargin = {
        id: 'legenMargin',
        beforeInit: function (chart, legend, options) {
            const fitValue = chart.legend.fit;
            chart.legend.fit = function fit() {
                fitValue.bind(chart.legend)();
                return this.height += 10;
            }
        }
    };

    const topLabels1 = {
        id: 'topLabels1',
        beforeInit: function (chart, legend, options) {
            const fitValue = chart.legend.fit;
            chart.legend.fit = function fit() {
                fitValue.bind(chart.legend)();
                return this.height += 15;
            }
        },
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

        //        console.log(index);
        //        console.log(chart.getDatasetMeta(1));

        //        ctx.font = 'bold 11px sans-serif';
        //        ctx.fillStyle = '#004e89';
        //        ctx.textAlign = 'center';
        //        ctx.fillText(sum.toFixed(1), x.getPixelForValue(index), y.getPixelForValue(index));

        //    })
        //}
    };

    var arrLeadTimeChart = [];

    this.DrawChart = function () {

        for (d of arrLeadTimeChart) {
            d.destroy();
        }

        arrLeadTimeChart = [];

        // ve bieu do leadtime theo thang
        var arrColor = ['#664dc9', '#44c4fa', '#38cb89', '#3e80eb', '#17594A', '#ffab00', '#ef4b4b'];
        var arrColorGap = ['#44c4fa', '#ef4b4b'];
        let monthLabel = ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'];

        let gapsLabel = [];
        for (g of Gaps) {
            gapsLabel.push(g);
        }

        // Lead time theo tháng (Đơn vị:  ngày)

        let ctx_wlp_month = document.getElementById('chart_sample_leadtime_month').getContext('2d');

        let monthLabel2 = [];
        for (w of Sample_LeadTimeByMonth) {

            if (!monthLabel2.includes(Number(w.Label_x))) {
                monthLabel2.push(Number(w.Label_x));
            }
        }

        if (monthLabel2.length == 0) {
            monthLabel2 = monthLabel;
        }

        let dic_wlpMonth = new Object();
        var dataValue_P_wlpMonth = [];
        var dataValue_H_wlpMonth = [];
        var dataValue_R_wlpMonth = [];
        var dataValue_Z_wlpMonth = [];
        var dataValue_M_wlpMonth = [];
        var dataValue_R_Target = [];
        var dataValue_P_Target = [];

        let _mIndex = 0;
        for (var _i = 0; _i < monthLabel2.length; _i++) {

            _mIndex = 0;
            for (pl of Sample_LeadTimeByMonth) {

                if (monthLabel2[_i] == Number(pl.Label_x)) {

                    if (pl.Legend == 'P') {
                        dataValue_P_wlpMonth.push(pl.Code_P);
                    }

                    if (pl.Legend == 'H') {
                        dataValue_H_wlpMonth.push(pl.Code_H);
                    }

                    if (pl.Legend == 'R') {
                        dataValue_R_wlpMonth.push(pl.Code_R);
                    }

                    if (pl.Legend == 'Z') {
                        dataValue_Z_wlpMonth.push(pl.Code_Z);
                    }

                    if (pl.Legend == 'M') {
                        dataValue_M_wlpMonth.push(pl.Code_M);
                    }
                }

                if (_mIndex == 0) {
                    dataValue_R_Target.push(pl.Target_R);
                    dataValue_P_Target.push(pl.Target_P);
                }

                _mIndex += 1;
            }

            if (dataValue_P_wlpMonth.length < _i+1) {
                dataValue_P_wlpMonth.push(0);
            }
            if (dataValue_H_wlpMonth.length < _i + 1) {
                dataValue_H_wlpMonth.push(0);
            }
            if (dataValue_R_wlpMonth.length < _i + 1) {
                dataValue_R_wlpMonth.push(0);
            }
            if (dataValue_Z_wlpMonth.length < _i + 1) {
                dataValue_Z_wlpMonth.push(0);
            }
            if (dataValue_M_wlpMonth.length < _i + 1) {
                dataValue_M_wlpMonth.push(0);
            }
        }

        dic_wlpMonth['P'] = dataValue_P_wlpMonth;
        dic_wlpMonth['H'] = dataValue_H_wlpMonth;
        dic_wlpMonth['R'] = dataValue_R_wlpMonth;
        dic_wlpMonth['Z'] = dataValue_Z_wlpMonth;
        dic_wlpMonth['M'] = dataValue_M_wlpMonth;
        dic_wlpMonth['Target R'] = dataValue_R_Target;
        dic_wlpMonth['Target P'] = dataValue_P_Target;

        let objDatasets_wlpMonth = [];
        let k = 0;
        for (let key in dic_wlpMonth) {
            let obj = {
                label: key,
                data: dic_wlpMonth[key],
                backgroundColor: arrColor[k],
                borderColor: arrColor[k],
                borderDash: key.includes('Target') ? [10, 5] : [],
                borderWidth: 0.8,
                /*fill: true,*/
                type: key.includes('Target') ? 'line' : 'bar'
            }

            if (!obj.label.includes('Target')) {
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
                labels: monthLabel2,
                datasets: objDatasets_wlpMonth
            },
            plugins: [ChartDataLabels, legenMargin],
            options: {
                /* indexAxis: 'y',*/
                plugins: {
                    // Change options for ALL labels of THIS CHART
                    datalabels: {
                        anchor: 'start',
                        align: 'top',
                        color: '#ffff',
                        formatter: function (value, index, values) {
                            if (value > 0) {
                                return value;
                            } else {
                                value = "";
                                return value;
                            }
                        }
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
                    }
                }
            }
        });
        arrLeadTimeChart.push(monthChart_wlp);

        // Lead time tổng theo tuần

        let ctx_wlp_week = document.getElementById('chart_sample_leadtime_week').getContext('2d');

        let weeksLabel = [];
        for (w of Sample_LeadTimeByWeek) {
            if (!weeksLabel.includes(Number(w.Label_x))) {
                weeksLabel.push(Number(w.Label_x));
            }
        }

        let dic_wlpWeek = new Object();
        var dataValue_P_wlpWeek = [];
        var dataValue_H_wlpWeek = [];
        var dataValue_R_wlpWeek = [];
        var dataValue_Z_wlpWeek = [];
        var dataValue_M_wlpWeek = [];
        var dataValue_R_Target_Week = [];
        var dataValue_P_Target_Week = [];

        let _wIndex = 0;
        for (var _i = 0; _i < weeksLabel.length; _i++) {

            _wIndex = 0;
            for (pl of Sample_LeadTimeByWeek) {

                if (weeksLabel[_i] == Number(pl.Label_x)) {

                    if (pl.Legend == 'P') {
                        dataValue_P_wlpWeek.push(pl.Code_P);
                    }

                    if (pl.Legend == 'H') {
                        dataValue_H_wlpWeek.push(pl.Code_H);
                    }

                    if (pl.Legend == 'R') {
                        dataValue_R_wlpWeek.push(pl.Code_R);
                    }

                    if (pl.Legend == 'Z') {
                        dataValue_Z_wlpWeek.push(pl.Code_Z);
                    }

                    if (pl.Legend == 'M') {
                        dataValue_M_wlpWeek.push(pl.Code_M);
                    }
                }

                if (_wIndex == 0) {
                    dataValue_R_Target_Week.push(pl.Target_R);
                    dataValue_P_Target_Week.push(pl.Target_P);
                }

                _wIndex += 1;
            }

            if (dataValue_P_wlpWeek.length < _i + 1) {
                dataValue_P_wlpWeek.push(0);
            }
            if (dataValue_H_wlpWeek.length < _i + 1) {
                dataValue_H_wlpWeek.push(0);
            }
            if (dataValue_R_wlpWeek.length < _i + 1) {
                dataValue_R_wlpWeek.push(0);
            }
            if (dataValue_Z_wlpWeek.length < _i + 1) {
                dataValue_Z_wlpWeek.push(0);
            }
            if (dataValue_M_wlpWeek.length < _i + 1) {
                dataValue_M_wlpWeek.push(0);
            }
        }

        dic_wlpWeek['P'] = dataValue_P_wlpWeek;
        dic_wlpWeek['H'] = dataValue_H_wlpWeek;
        dic_wlpWeek['R'] = dataValue_R_wlpWeek;
        dic_wlpWeek['Z'] = dataValue_Z_wlpWeek;
        dic_wlpWeek['M'] = dataValue_M_wlpWeek;
        dic_wlpWeek['Target R'] = dataValue_R_Target_Week;
        dic_wlpWeek['Target P'] = dataValue_P_Target_Week;

        let objDatasets_wlpWeek = [];
        let h = 0;
        for (let key in dic_wlpWeek) {
            let obj = {
                label: key,
                data: dic_wlpWeek[key],
                backgroundColor: arrColor[h],
                borderColor: arrColor[h],
                borderDash: key.includes('Target') ? [10, 5] : [],
                borderWidth: 0.8,
                /*fill: true,*/
                type: key.includes('Target') ? 'line' : 'bar'
            }

            if (!obj.label.includes('Target')) {
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
                    datalabels: {
                        anchor: 'start',
                        align: 'top',
                        color: '#ffff',
                        formatter: function (value, index, values) {
                            if (value > 0) {
                                return value;
                            } else {
                                value = "";
                                return value;
                            }
                        }
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
                    }
                }
            }
        });
        arrLeadTimeChart.push(weekChart_wlp);

        // tong wafer by month
        let ctx_wafer_month = document.getElementById('chart_sample_wafer_month').getContext('2d');

        let dic_wafer_month = new Object();
        var dataValue_P_wafer_month = [];
        var dataValue_H_wafer_month = [];
        var dataValue_R_wafer_month = [];
        var dataValue_Z_wafer_month = [];
        var dataValue_M_wafer_month = [];
        var dataValue_Total_wafer_month = [];

        for (pl of Sample_WaferByMonth) {

            if (pl.Legend == 'P') {
                dataValue_P_wafer_month.push(pl.Code_P);
            }

            if (pl.Legend == 'H') {
                dataValue_H_wafer_month.push(pl.Code_H);
            }

            if (pl.Legend == 'R') {
                dataValue_R_wafer_month.push(pl.Code_R);
            }

            if (pl.Legend == 'Z') {
                dataValue_Z_wafer_month.push(pl.Code_Z);
            }

            if (pl.Legend == 'M') {
                dataValue_M_wafer_month.push(pl.Code_M);
            }
        }

        dic_wafer_month['P'] = dataValue_P_wafer_month;
        dic_wafer_month['H'] = dataValue_H_wafer_month;
        dic_wafer_month['R'] = dataValue_R_wafer_month;
        dic_wafer_month['Z'] = dataValue_Z_wafer_month;
        dic_wafer_month['M'] = dataValue_M_wafer_month;

        for (var i = 0; i < dataValue_P_wafer_month.length; i++) {
            dataValue_Total_wafer_month.push(dataValue_P_wafer_month[i] + dataValue_H_wafer_month[i] + dataValue_R_wafer_month[i] + dataValue_Z_wafer_month[i] + dataValue_M_wafer_month[i]);
        }

        dic_wafer_month['Total'] = dataValue_Total_wafer_month;

        let objDatasets_wafer_month = [];
        let z = 0;
        for (let key in dic_wafer_month) {
            let obj = {
                label: key,
                data: dic_wafer_month[key],
                backgroundColor: key != 'Total' ? arrColor[z] :'#ffff',
                type: 'bar'
            }

            if (key != 'Total') {
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
                    color: '#004e89',
                    display: true
                }
            }
            

            objDatasets_wafer_month.push(obj);
            z += 1;
        }

        let monthLabel3 = [];

        for (w of Sample_WaferByMonth) {

            if (!monthLabel3.includes(Number(w.Label_x))) {
                monthLabel3.push(Number(w.Label_x));
            }
        }

        if (monthLabel3.length == 0) {
            monthLabel3 = monthLabel;
        }

        let chart_wafer_month = new Chart(ctx_wafer_month, {
            type: 'bar',
            data: {
                labels: monthLabel3,
                datasets: objDatasets_wafer_month
            },
            plugins: [ChartDataLabels, legenMargin],
            options: {
                /* indexAxis: 'y',*/
                plugins: {
                    //tooltip: {
                    //    callbacks: {
                    //        footer: function (items) {
                    //            return 'Total: ' + items.reduce((a, b) => a + b.parsed.y, 0)
                    //        }
                    //    }
                    //},
                    // Change options for ALL labels of THIS CHART
                    datalabels: {
                        anchor: 'start',
                        align: 'top',
                        color: '#ffff',
                        formatter: function (value, index, values) {
                            if (value > 0) {
                                return value;
                            } else {
                                value = "";
                                return value;
                            }
                        }
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
                        beginAtZero: true,
                        ticks: {
                            fontSize: 11
                        },
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
        arrLeadTimeChart.push(chart_wafer_month);

        // tong wafer by week
        let ctx_wafer_week = document.getElementById('chart_sample_wafer_week').getContext('2d');

        let dic_wafer_week = new Object();
        var dataValue_P_wafer_week = [];
        var dataValue_H_wafer_week = [];
        var dataValue_R_wafer_week = [];
        var dataValue_Z_wafer_week = [];
        var dataValue_M_wafer_week = [];
        var dataValue_Total_wafer_week = [];

        for (pl of Sample_WaferByWeek) {

            if (pl.Legend == 'P') {
                dataValue_P_wafer_week.push(pl.Code_P);
            }

            if (pl.Legend == 'H') {
                dataValue_H_wafer_week.push(pl.Code_H);
            }

            if (pl.Legend == 'R') {
                dataValue_R_wafer_week.push(pl.Code_R);
            }

            if (pl.Legend == 'Z') {
                dataValue_Z_wafer_week.push(pl.Code_Z);
            }

            if (pl.Legend == 'M') {
                dataValue_M_wafer_week.push(pl.Code_M);
            }
        }


        let weeksLabel2 = [];
        for (w of Sample_WaferByWeek) {
            if (!weeksLabel2.includes(Number(w.Label_x))) {
                weeksLabel2.push(Number(w.Label_x));
            }
        }

        dic_wafer_week['P'] = dataValue_P_wafer_week;
        dic_wafer_week['H'] = dataValue_H_wafer_week;
        dic_wafer_week['R'] = dataValue_R_wafer_week;
        dic_wafer_week['Z'] = dataValue_Z_wafer_week;
        dic_wafer_week['M'] = dataValue_M_wafer_week;

        for (var i = 0; i < dataValue_P_wafer_week.length; i++) {
            dataValue_Total_wafer_week.push(dataValue_P_wafer_week[i] + dataValue_H_wafer_week[i] + dataValue_R_wafer_week[i] + dataValue_Z_wafer_week[i] + dataValue_M_wafer_week[i]);
        }

        dic_wafer_week['Total'] = dataValue_Total_wafer_week;

        let objDatasets_wafer_week = [];
        let m = 0;
        for (let key in dic_wafer_week) {
            let obj = {
                label: key,
                data: dic_wafer_week[key],
                backgroundColor: key != 'Total' ? arrColor[m] : '#ffff',
                borderColor: key != 'Total' ? arrColor[m] : '#ffff',
                type: 'bar'
            }

            if (key != 'Total') {
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
                    color: '#004e89',
                    size:12,
                    display: true
                }
            }
            

            objDatasets_wafer_week.push(obj);
            m += 1;
        }

       

        console.log(weeksLabel2);
        console.log(objDatasets_wafer_week);

        let chart_wafer_week = new Chart(ctx_wafer_week, {
            type: 'bar',
            data: {
                labels: weeksLabel2,
                datasets: objDatasets_wafer_week
            },
            plugins: [ChartDataLabels, legenMargin],
            options: {
                /* indexAxis: 'y',*/
                plugins: {
                    //tooltip: {
                    //    callbacks: {
                    //        footer: function (items) {
                    //            return 'Total: ' + items.reduce((a, b) => a + b.parsed.y, 0)
                    //        }
                    //    }
                    //},
                    // Change options for ALL labels of THIS CHART
                    datalabels: {
                        //anchor: 'start',
                        //align: 'top',
                        //color: '#ffff',
                         formatter: function (value, index, values) {
                            if (value > 0) {
                                return value;
                            } else {
                                value = "";
                                return value;
                            }
                        }
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
        arrLeadTimeChart.push(chart_wafer_week);

        // gap leadtime 
        let ctx_gap_leadtime = document.getElementById('chart_leadtime_gap').getContext('2d');

        let dic_gap_leadtime = new Object();
        var dataValue_gap = [];
        var dataValue_gap_rate = [];


        for (pl of SampleGapLeadtime) {
            dataValue_gap.push(pl.Value);
            dataValue_gap_rate.push(pl.Rate);
        }

        dic_gap_leadtime['Gap'] = dataValue_gap;
        dic_gap_leadtime['Rate(%)'] = dataValue_gap_rate;

        let objDatasets_gap_leadtime = [];
        let x = 0;
        for (let key in dic_gap_leadtime) {
            let obj = {
                label: key,
                data: dic_gap_leadtime[key],
                backgroundColor: arrColorGap[x],
                borderColor: arrColorGap[x],
                borderDash: key == 'Rate(%)' ? [10, 5] : [],
                /* borderWidth: 0.5,*/
                /*fill: true,*/
                type: key == 'Rate(%)' ? 'line' : 'bar',
                yAxisID: key == 'Rate(%)' ? 'y1' : 'y',
                order: key == 'Rate(%)' ? 0 : 1,
            }

            if (key != 'Rate(%)') {
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
                    color: '#B31312',
                    offset: 10,
                    display: true
                }
            }

            objDatasets_gap_leadtime.push(obj);
            x += 1;
        }

        let chart_gap_leadtime = new Chart(ctx_gap_leadtime, {
            type: 'bar',
            data: {
                labels: gapsLabel,
                datasets: objDatasets_gap_leadtime
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
        arrLeadTimeChart.push(chart_gap_leadtime);
    }

}