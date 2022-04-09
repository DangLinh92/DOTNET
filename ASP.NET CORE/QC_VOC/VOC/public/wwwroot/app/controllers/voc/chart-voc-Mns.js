var chartVocMnsController = function () {
    this.initialize = function () {
        GetCustomer();
        registerEvents();
    }

    function registerEvents() {

        $('#txtYear').on('dp.change', function (e) {
            console.log(e.date.format('YYYY'));

            var year = e.date.format('YYYY');
            var customer = $('#cboCustomer').val();
            var side = $('#cboSide').val();

            SearchDataChart(year, customer, side);
        });

        $('#cboCustomer').on('change', function () {
            console.log('2');

            var year = $('#txtYear').val();
            var customer = $('#cboCustomer').val();
            var side = $('#cboSide').val();

            SearchDataChart(year, customer, side);
        });

        $('#cboSide').on('change', function () {
            console.log('3');

            var year = $('#txtYear').val();
            var customer = $('#cboCustomer').val();
            var side = $('#cboSide').val();
            SearchDataChart(year, customer, side);
        });

        function SearchDataChart(year, customer, side) {
            $.ajax({
                type: "GET",
                url: "/Admin/Voc/Search",
                data: {
                    year: year,
                    customer: customer,
                    side: side
                },
                success: function (response) {
                    if (chartdataInit) {
                        chartdataInit = response.vOCSiteModelByTimeLsts;
                        chartdataTotalInit = response.totalVOCSitesView;
                        chartdataByDefectName = response.paretoDataDefectName;

                        $('#tagReceiveCount').text(response.vocProgessInfo.ReceiveCount);
                        $('#tagCloseCount').text(response.vocProgessInfo.CloseCount);
                        $('#tagProgressCount').text(response.vocProgessInfo.ProgressCount);
                        $('#vocByMonthTitle').text(side + '진성 VOC 건수');
                        $('#vocYearTitle').text(year.toString().substring(2, 4) + '년 VOC 접수 건수');

                        chartVoc.DrawChart();
                        apexVoc.DrawChart();
                    }
                },
                error: function (status) {
                    hrms.notify(status.responseText, 'error', 'alert', function () { });
                }
            });
        }
    }

    function GetCustomer() {
        $.ajax({
            url: '/Admin/Voc/GetCustomer',
            type: 'GET',
            success: function (response) {
                var render = "<option value=''>All</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.trim() + "'>" + item + "</option >"
                });
                $('#cboCustomer').html(render);
            },
            error: function (status) {
                hrms.notify(status.responseText, 'error', 'alert', function () { });
            }
        });
    }
}