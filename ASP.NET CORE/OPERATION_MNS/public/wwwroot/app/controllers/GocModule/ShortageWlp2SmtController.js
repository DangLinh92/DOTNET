var ShortageWlp2SmtController = function () {
    this.initialize = function () {
        registerEvents();
        initSelectGetPlanIds();
    }

    function registerEvents() {

    }

    function initSelectGetPlanIds() {
        let planID = $('#cboPlanID').val();
        console.log(planID);
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
        hrms.run_waitMe($('#salesContent'));
    }

    this.doAftersearch = function () {
        InitDataTable();
        hrms.hide_waitMe($('#salesContent'));
    }

    function InitDataTable() {
        $('#gocSalesPlanDataTable').DataTable({
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
            //dom: 'Bfrtip',
            //buttons: [
            //    {
            //        text: 'Excel',
            //        action: function (e, dt, node, config) {

            //            let _planId = $('#cboPlanID').val();
            //            console.log(_planId);

            //            $.ajax({
            //                type: "POST",
            //                url: "/OpeationMns/GOCModule/ExportExcelShortageWlp2Smt",
            //                data: {
            //                    planId: _planId
            //                },
            //                beforeSend: function () {
            //                    hrms.run_waitMe($('#gridGocPlan'));
            //                },
            //                success: function (response) {
            //                    window.location.href = response;
            //                    hrms.hide_waitMe($('#gridGocPlan'));
            //                },
            //                error: function () {
            //                    hrms.notify('Has an error in progress!', 'error', 'alert', function () { });
            //                    hrms.hide_waitMe($('#gridGocPlan'));
            //                }
            //            });
            //        }
            //    }
            //]
        });

        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="gocSalesPlanDataTable_length"]').removeClass('form-control-sm');
    }
}