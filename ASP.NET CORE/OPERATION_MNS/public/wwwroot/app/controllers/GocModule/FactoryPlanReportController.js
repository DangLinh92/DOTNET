var FactoryPlanReportController = function () {
    this.initialize = function () {
        initSelectGetPlanIds();
    }

    function initSelectGetPlanIds() {
        let planID = $('#cboPlanID').val();
        $.ajax({
            url: '/OpeationMns/SCPMaster/GetPlanIds',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {

                var render = "";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.code + "'>" + item.name + "</option >"
                });
                $('#cboPlanID').html(render);
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading data', 'error', 'alert', function () { });
            }
        });
    }

    this.beginSearch = function () {
        hrms.run_waitMe($('#ReportContent'));
    }

    this.doAftersearch = function () {

        console.log('end');
        $("#gridContainer").dxDataGrid('instance').refresh();
        /*$("#gridContainer").dxDataGrid("getDataSource").reload();*/
        InitDataTable();
        hrms.hide_waitMe($('#ReportContent'));
    }

    function InitDataTable() {
        $('#gocFactoryPlanDataTable').DataTable({
            scrollY: 600,
            scrollX: true,
            scrollCollapse: true,
            paging: false,
            select: true,
            "searching": true,
            fixedColumns: {
                left: 2
            },
            fixedHeader: true
        });

        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="gocFactoryPlanDataTable_length"]').removeClass('form-control-sm');
    }
}