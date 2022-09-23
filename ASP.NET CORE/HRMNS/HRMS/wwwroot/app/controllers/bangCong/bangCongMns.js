var bangCongController = function () {
    this.initialize = function () {
        registerEvents();
        initSelectOptionBoPhan();
    }

    function registerEvents() {

        $('#btn-baocaolamthemgio').on('click', function () {
            $('#_txtBeginTimeBC').val('');
            $('#_txtEndTimeBC').val('');
            $('#baocaolamthemgioModel').modal('show');
        });

        $('#btnExportBaocaoLamThemGio').on('click', function (e) {
            if ($('#frmBaocaolamthemgio').valid()) {
                e.preventDefault();

                let department = $('#cboDepartmentBaoCao').val();
                let fromTime = $('#_txtBeginTimeBC').val();
                let endTime = $('#_txtEndTimeBC').val();

                hrms.run_waitMe($('#frmBaocaolamthemgio'));

                $.ajax({
                    url: '/Admin/BangCong/ExportBaoCaoOT',
                    type: 'POST',
                    data:
                    {
                        bophan: department,
                        fromTime: fromTime,
                        endTime: endTime
                    },
                    success: function (response) {
                        window.location.href = response;
                        $('#baocaolamthemgioModel').modal('hide');
                        hrms.hide_waitMe($('#frmBaocaolamthemgio'));
                    },
                    error: function (status) {
                        console.log(status.responseText);
                        hrms.notify('error:' + status.responseText, 'error', 'alert', function () {
                            hrms.hide_waitMe($('#frmBaocaolamthemgio'));
                        });
                    }
                })
            }
        });

        $('#btn-denghilamthemgio').on('click', function () {
            $('#_txtBeginTimeOT').val('');
            $('#_txtEndTimeOT').val('');
            $('#denghilamthemgioModel').modal('show');
        });

        $('#btnExportDenghiLamThemGio').on('click', function (e) {
            if ($('#frmDenghilamthemgio').valid()) {
                e.preventDefault();

                let department = $('#cboDepartmentDenghi').val();
                let fromTime = $('#_txtBeginTimeOT').val();
                let endTime = $('#_txtEndTimeOT').val();

                hrms.run_waitMe($('#frmDenghilamthemgio'));

                $.ajax({
                    url: '/Admin/BangCong/ExportDenghiOT',
                    type: 'POST',
                    data:
                    {
                        bophan: department,
                        fromTime: fromTime,
                        endTime: endTime
                    },
                    success: function (response) {
                        window.location.href = response;
                        $('#denghilamthemgioModel').modal('hide');
                        hrms.hide_waitMe($('#frmDenghilamthemgio'));
                    },
                    error: function (status) {
                        console.log(status.responseText);
                        hrms.notify('error:' + status.responseText, 'error', 'alert', function () {
                            hrms.hide_waitMe($('#frmDenghilamthemgio'));
                        });
                    }
                })
            }
        });

        $('#btnExportData').on('click', function () {
            hrms.run_waitMe($('#bangCongDataTable'));

            let _time = $('#searchToTime').val();
            let _dept = $('#cboDepartment').val();
            let _status = '';
            if ($("#chStatus").is(":checked")) {
                _status = 'InActive';
            }
            else {
                _status = 'Active';
            }
            let _timeEndUser = $('#searchOutToTime').val();

            if (!_time || !_timeEndUser) {
                hrms.notify('Hãy nhập ngày tháng!', 'error', 'alert', function () { });
                return;
            }

            $.ajax({
                type: "POST",
                url: "/Admin/BangCong/OutPutExcel",
                data:
                {
                    timeEndUser: _timeEndUser,
                    status: _status,
                    department: _dept,
                    timeTo: _time
                },
                success: function (response) {
                    window.location.href = response;
                    hrms.hide_waitMe($('#bangCongDataTable'));
                },
                error: function (status) {
                    hrms.notify(status.responseText, 'error', 'alert', function () { });
                    hrms.hide_waitMe($('#bangCongDataTable'));
                }
            });
        });

        $('#btnExportData2').on('click', function () {

            let _time = $('#searchToTime').val();
            let _dept = $('#cboDepartment').val();

            if (!_time) {
                hrms.notify('Hãy nhập tháng!', 'error', 'alert', function () { });
                return;
            }

            hrms.run_waitMe($('#bangCongDataTable'));

            $.ajax({
                type: "POST",
                url: "/Admin/BangCong/TongHopNhanSuDaily",
                data:
                {
                    time: _time,
                    dept: _dept
                },
                success: function (response) {
                    hrms.hide_waitMe($('#bangCongDataTable'));
                    window.open("/Admin/Spreadsheet/Export?fileName=" + response, '_blank');
                },
                error: function (status) {
                    hrms.notify(status.responseText, 'error', 'alert', function () { });
                    hrms.hide_waitMe($('#bangCongDataTable'));
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
                    $('#cboDepartmentDenghi').html(render);
                    $('#cboDepartmentBaoCao').html(render);
                }
                else {
                    var render = "<option value=''>--All--</option>";
                    $.each(response, function (i, item) {
                        render += "<option value='" + item.Id + "'>" + item.TenBoPhan + "</option >"
                    });
                    $('#cboDepartment').html(render);
                    $('#cboDepartmentDenghi').html(render);
                    $('#cboDepartmentBaoCao').html(render);
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