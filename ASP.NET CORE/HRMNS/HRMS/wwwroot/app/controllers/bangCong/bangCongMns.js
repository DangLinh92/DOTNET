var bangCongController = function () {
    this.initialize = function () {
        registerEvents();
        initSelectOptionBoPhan();
    }

    function registerEvents() {
        $('#btnExportData').on('click', function () {

            $.ajax({
                type: "POST",
                url: "/Admin/BangCong/OutPutExcel",
                success: function (response) {
                    window.location.href = response;
                },
                error: function (status) {
                    hrms.notify(status.responseText, 'error', 'alert', function () { });
                }
            });
        });
    }


    function initSelectOptionBoPhan() {
        $.ajax({
            url: '/Admin/BoPhan/GetAll',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {

                if (deparment != '' && deparment != 'SP') {
                    var render = "<option value='" + deparment + "'>" + deparment + "</option >";
                    $('#cboDepartment').html(render);
                }
                else
                {
                    var render = "<option value=''>--All--</option>";
                    $.each(response, function (i, item)
                    {
                        render += "<option value='" + item.Id + "'>" + item.TenBoPhan + "</option >"
                    });
                    $('#cboDepartment').html(render);
                }
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading department data', 'error', 'alert', function () { });
            }
        });
    }

    this.beginSearch = function () {
        hrms.run_waitMe($('#bangCongDataTable'));
    }

    this.doAftersearch = function () {
        InitDataTable();
        hrms.hide_waitMe($('#bangCongDataTable'));
    }

    function InitDataTable() {
        $('#bangCongDataTable').DataTable({
            scrollY: 600,
            scrollX: true,
            scrollCollapse: true,
            paging: false,
            select: true,
            "searching": true,
            fixedColumns: {
                left: 5
            },
            fixedHeader: true
        });

        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="bangCongDataTable_length"]').removeClass('form-control-sm');
    }
}