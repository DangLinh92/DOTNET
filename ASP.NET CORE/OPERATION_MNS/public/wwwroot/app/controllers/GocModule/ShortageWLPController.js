var ShortageWLPController = function () {
    this.initialize = function () {
        initSelectGetPlanIds();
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

    this.beginSearch = function () {
        hrms.run_waitMe($('#productionContent'));
    }

    this.doAftersearch = function () {
        InitDataTable();
        hrms.hide_waitMe($('#productionContent'));
    }

    function InitDataTable() {
        $('#shortageWLPDataTable').DataTable({
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
            fixedHeader: true
        });

        $('#shortageWLPDataTable input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="shortageWLPDataTable_length"]').removeClass('form-control-sm');
    }
}