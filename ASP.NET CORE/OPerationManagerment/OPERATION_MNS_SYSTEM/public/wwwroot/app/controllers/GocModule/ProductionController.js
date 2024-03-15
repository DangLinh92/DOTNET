var ProductionController = function () {
    this.initialize = function () {
        registerEvents();
        initSelectGetPlanIds();
        initSelectGetSites();
    }

    function registerEvents() {
        $('#btnLoadDataSCP').on('click', function () {

            let masterId = $('#cboMasterID').val();
            let planId = $('#cboPlanID').val();
            let siteId = $('#cboSiteId').val();

            if (masterId.length < 3 || planId.length < 3 || siteId.length < 3) {
                hrms.notify('error: Thông tin masterId,plan Id,Site Id không phù hợp', 'error', 'alert', function () { });
            }
            else {
                $.ajax({
                    url: '/OpeationMns/GOCModule/GetSCPData',
                    type: 'POST',
                    data: {
                        masterID: masterId,
                        planID: planId,
                        siteID: siteId
                    },
                    beforeSend: function () {
                        hrms.run_waitMe($('#productionContent'));
                    },
                    success: function (data) {
                        hrms.hide_waitMe($('#productionContent'));

                        if (data) {
                            hrms.notify("Load data success!", 'Success', 'alert', function () {
                            });
                        }
                        else {
                            hrms.notify("Load data error!", 'error', 'alert', function () {
                            });
                        }

                    },
                    error: function (status) {
                        hrms.notify('error: Load error!' + status.responseText, 'error', 'alert', function () { });
                        hrms.hide_waitMe($('#productionContent'));
                    }
                });
            }


        });
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

                        $.ajax({
                            type: "POST",
                            url: "/OpeationMns/GOCModule/ExportExcelProdPlan",
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


        $('#gocDailyShortageDataTable').DataTable({
            scrollY: 600,
            scrollX: true,
            scrollCollapse: true,
            paging: false,
            select: true,
            "searching": true,
            /*     "order": [3, 'asc'],*/
            fixedColumns: {
                left: 2
            },
            fixedHeader: true,
            dom: 'Bfrtip',
            buttons: [
                'excel'
            ]
        });

        $('#gocDailyShortageDataTable input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="gocDailyShortageDataTable_length"]').removeClass('form-control-sm');
    }
}