var DailyUsingController = function () {
    this.initialize = function () {
        registerEvents();
        initSelectGetPlanIds();
        initSelectGetSites();
    }

    function registerEvents() {

    }

    function initSelectGetPlanIds() {
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

    function initSelectGetSites() {
        $.ajax({
            url: '/OpeationMns/SCPMaster/GetSites',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {

                var render = "<option value=' '> - </option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.code + "'>" + item.name + "</option >"
                });
                $('#cboSiteId').html(render);
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading data', 'error', 'alert', function () { });
            }
        });
    }

    this.beginSearch = function () {
        hrms.run_waitMe($('#BomContent'));
    }

    this.doAftersearch = function () {
        InitDataTable();
        hrms.hide_waitMe($('#BomContent'));
    }

    function InitDataTable() {


        $('#dailyUsingBomDataTable').DataTable({
            scrollY: 550,
            scrollX: true,
            scrollCollapse: true,
            paging: true,
            select: true,
            "searching": true,
       /*     "order": [3, 'asc'],*/
            fixedColumns: {
                left: 1
            },
            fixedHeader: true,
            dom: 'Bfrtip',
            buttons: [
                'excel'
            ]
        });

        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="dailyUsingBomDataTable_length"]').removeClass('form-control-sm');
    }
}