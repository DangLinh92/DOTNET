var ProductionWLPController = function () {
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
        hrms.run_waitMe($('#productionContent'));
    }

    this.doAftersearch = function () {
        InitDataTable();
        hrms.hide_waitMe($('#productionContent'));
    }

    function InitDataTable() {
        $('#gocProdPlanDataTable').DataTable({
            scrollY: 600,
            scrollX: true,
            scrollCollapse: true,
            paging: false,
            select: true,
            "searching": true,
            /*     "order": [3, 'asc'],*/
            fixedColumns: {
                left: 3
            },
            fixedHeader: true,
            dom: 'Bfrtip',
            buttons: [
                {
                    text: 'Excel',
                    action: function (e, dt, node, config) {

                        let _planId = $('#cboPlanID').val();
                        console.log(_planId);

                        let _url = "/OpeationMns/GOCModule/ExportExcelProdPlanWlp";

                        $.ajax({
                            type: "POST",
                            url: _url,
                            data: {
                                planId: _planId
                            },
                            beforeSend: function () {
                                hrms.run_waitMe($('#gridGocPlanSmt'));
                            },
                            success: function (response) {
                                window.location.href = response;
                                hrms.hide_waitMe($('#gridGocPlanSmt'));
                            },
                            error: function () {
                                hrms.notify('Has an error in progress!', 'error', 'alert', function () { });
                                hrms.hide_waitMe($('#gridGocPlanSmt'));
                            }
                        });
                    }
                }
            ]
        });

        $('#gocProdPlanDataTable input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="gocProdPlanDataTable_length"]').removeClass('form-control-sm');

    }
}