var CheckDemandController = function () {
    this.initialize = function () {
        initSelectGetPlanIds();
        initSelectGetSites();
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
        hrms.run_waitMe($('#productionContent'));
    }

    this.doAftersearch = function () {
        hrms.hide_waitMe($('#productionContent'));
        InitDataTable();
    }

    function InitDataTable() {

        $('#gocProdPlanDataTable').DataTable().destroy();
        $('#gocProdPlanDataTable').DataTable({
            scrollY: 600,
            scrollX: true,
            scrollCollapse: true,
            paging: false,
            select: true,
            "searching": true,
            fixedColumns: {
                left: 2
            },
            fixedHeader: true,
        });

        $('#gocProdPlanDataTable input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="gocProdPlanDataTable_length"]').removeClass('form-control-sm');
    }
}