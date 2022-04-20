var chartPPMMnsController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {

        // export excel
        $('#btnExportData').on('click', function () {

            var year = $('#txtYear').val();

            if (!year) {
                year = new Date().getFullYear();
            }

            $.ajax({
                type: "POST",
                url: "/Admin/K1/ExportExcelK1",
                data: {
                    year: year
                },
                success: function (response) {
                    window.location.href = response;
                },
                error: function (status) {
                    hrms.notify(status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        //$('#txtYear').on('dp.change', function (e) {
        //    console.log(e.date.format('YYYY'));

        //    var year = e.date.format('YYYY');
        //    var side = $('#cboSite').val();
        //    console.log($('#cboSite').val());
        //    SearchDataChart(year, side);
        //});

        //$('#cboSite').on('change', function () {

        //    var year = $('#txtYear').val();
        //    var side = $('#cboSite').val();
        //    console.log($('#cboSite').val());
        //    SearchDataChart(year, side);
        //});

        //$('#btnSearch').on('click', function () {
        //    var year = $('#txtYear').val();
        //    var side = $('#cboSite').val();
        //    SearchDataChart(year, side);
        //});

        //function SearchDataChart(year, site) {
        //    $.ajax({
        //        type: "GET",
        //        url: "/Admin/K1/Search",
        //        data: {
        //            year: year,
        //            site: site
        //        },
        //        success: function (response) {
        //            if (chardataPPM) {
        //                chardataPPM = response.VocPPMView;
        //                chartjsPPM.DrawChart(site);
        //            }
        //        },
        //        error: function (status) {
        //            hrms.notify(status.responseText, 'error', 'alert', function () { });
        //        }
        //    });
        //}
    }
}