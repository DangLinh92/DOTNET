var chamCongController = function () {
    this.initialize = function () {
        registerEvents();
        initSelectOptionBoPhan();
    }

    function registerEvents() {

        $('#btnExportData').on('click', function () {

            $.ajax({
                type: "POST",
                url: "/Admin/ChamCong/ExportExcel",
                success: function (response) {
                    window.location.href = response;
                },
                error: function (status) {
                    hrms.notify(status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        $('#btnExportAbsenceData').on('click', function () {

            var dept = $('#cboBoPhan').val();
            var fromTime = $('#txtTimeFrom').val();
            var toTime = $('#txtTimeTo').val();

            $.ajax({
                type: "POST",
                url: "/Admin/ChamCong/GetChamCongAbsenceLog",
                data: {
                    fromTime: fromTime,
                    toTime: toTime,
                    dept: dept
                },
                success: function (response) {
                    window.location.href = response;
                },
                error: function (status) {
                    hrms.notify(status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // Import excel
        // 1. import even log
        $('#btn-importAttendance').on('click', function () {
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val('');
            $('#import_EvenLog').modal('show');
        });

        // Close import
        $('#btnCloseImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                $("#fileInputExcel").val(null);
                $('#hd-ImportType').val('');
                $('#import_EvenLog').modal('hide');
                location.reload();
            }
        });

        // Close import
        $('#btnCloseImport').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                $("#fileInputExcel").val(null);
                $('#hd-ImportType').val('');
                $('#import_EvenLog').modal('hide');
                location.reload();
            }
        });

        // Click import excel
        $('#btnImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            // Adding one more key to FormData object  
            // fileData.append('categoryId', $('#ddlCategoryIdImportExcel').combotree('getValue'));
            var type = $('#hd-ImportType').val();

            $.ajax({
                url: '/Admin/ChamCong/ImportExcel?param=' + type,
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    $('#import_EvenLog').modal('hide');
                    hrms.notify("Import success!", 'Success', 'alert', function () {
                        location.reload();
                    });
                },
                error: function (status) {
                    hrms.notify('error: Import error!', 'error', 'alert', function () { });
                }
            });
            return false;
        });

        // Get Log start
        $('#btnGetLog').on('click', function () {
            $.ajax({
                type: "GET",
                url: "/Admin/ChamCong/GetChamCongLog",
                beforeSend: function () {
                    hrms.run_waitMe($('#chamCongLogDataTable'));
                },
                success: function (response) {
                    hrms.hide_waitMe($('#chamCongLogDataTable'));
                    hrms.notify("Get Log success!", 'Success', 'alert', function () {
                        location.reload();
                    });
                },
                error: function () {
                    hrms.hide_waitMe($('#chamCongLogDataTable'));
                    hrms.notify('Has an error in progress!', 'error', 'alert', function () {
                    });
                }
            });
        });
        // Get Log end

        // open form show info
        $('body').on('click', '.attendance-info', function (e) {
            e.preventDefault();

            var _userId = $(this).data('id');
            var _time = $(this).data('date');
            var _inTime = $(this).data('timein');
            var _ouTime = $(this).data('timeout');

            $('#lblNgayChamCong').text(_time);
            $('#lblBeginTime').text(_inTime);
            $('#lblEndTime').text(_ouTime);
            $('#lblTitle').text($(this).data('name'));
            $('#lblTitleId').text('('+_userId+')');

            $.ajax({
                url: '/Admin/ChamCong/GetLogByUserId',
                type: 'POST',
                dataType: 'json',
                data: {
                    userId: _userId,
                    time: _time
                },
                success: function (data) {

                    var render = "";

                    $.each(data, function (index, value) {
                        render += "<li><p class='mb-0'>" + value.sName + "</p><p class='res-activity-time'><i class='fa fa-clock-o'></i>" + (new Date(value.sLogTime)).toLocaleTimeString() + "</p></li>"
                    });

                    var dateTimeMin = new Date(_time + ' ' + _inTime);
                    var dateTimeMax = new Date(_time + ' ' + _ouTime);

                    var Difference_In_Time = dateTimeMax.getTime() - dateTimeMin.getTime();
                    var Difference_In_Days = Difference_In_Time / (1000 * 3600); // hour in day

                    $('#lblTotalTimeInDay').text(Math.abs(Difference_In_Days.toFixed(2)) + ' hrs');
                    $('#slActivity').html(render);

                },
                error: function (status) {
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                }
            });

            $('#attendance_info').modal('show');
        });

        $('#btnSave_ChamCong').on('click', function (e) {
            e.preventDefault();

            var _userId = $('#_txtMaNV').val();
            var _time = $('#_txtdate').val();
            var _nameNV = $('#_txtTenNV').val();
            var _firstTime = $('#_txtfirstTime').val();
            var _lastTime = $('#_txtlastTime').val();

            $.ajax({
                url: '/Admin/ChamCong/UpdateTimeChamCong',
                type: 'POST',
                dataType: 'json',
                data:
                {
                    firstTime: _firstTime,
                    lastTime: _lastTime,
                    maNV: _userId,
                    ngayChamCong: _time,
                    tenNV: _nameNV
                },
                success: function (data) {
                    hrms.notify("Update success!", 'Success', 'alert', function () {
                        $('#btnSearch').submit();
                    });
                },
                error: function (status) {
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                }
            });

            $('#addEditChamCongModel').modal('hide');
        });

        $('body').on('click', '.edit-chamcong', function (e) {

            e.preventDefault();
            var _userId = $(this).data('id');
            var _time = $(this).data('date');
            var _nameNV = $(this).data('name');
            var _inTime = $(this).data('timein');
            var _ouTime = $(this).data('timeout');

            $('#_txtMaNV').val(_userId);
            $('#_txtTenNV').val(_nameNV);
            $('#_txtdate').val(_time);
            $('#_txtfirstTime').val(_inTime);
            $('#_txtlastTime').val(_ouTime);

            $('#addEditChamCongModel').modal('show');
        });
    }

    function initSelectOptionBoPhan() {

        if (deparment != '' && deparment != 'SP') {
            var render = "";
            if (deparment == "WLP2") {
                render = "<option value='WLP 2'>" + deparment + "</option >";
            }
            else if (deparment == "WLP1") {
                render = "<option value='WLP 1'>" + deparment + "</option >";
            }
            else {
                render = "<option value='" + deparment + "'>" + deparment + "</option >";
            }
            $('#cboBoPhan').html(render);
        }
        else {
            $.ajax({
                url: '/Admin/ChamCong/GetDepartment',
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (response) {

                    if (deparment != '' && deparment != 'SP') {
                        var render = "";
                        if (deparment == "WLP2") {
                            render = "<option value='WLP 2'>" + deparment + "</option >";
                        }
                        else if (deparment == "WLP1") {
                            render = "<option value='WLP 1'>" + deparment + "</option >";
                        }
                        else {
                            render = "<option value='" + deparment + "'>" + deparment + "</option >";
                        }
                        $('#cboBoPhan').html(render);
                    }
                    else {
                        var render = "<option value=''>--All--</option>";
                        $.each(response, function (i, item) {
                            render += "<option value='" + item.sName + "'>" + item.sName + "</option >"
                        });
                        $('#cboBoPhan').html(render);
                    }
                },
                error: function (status) {
                    console.log(status);
                    hrms.notify('Cannot loading department data', 'error', 'alert', function () { });
                }
            });
        }
    }

    this.beginSearch = function () {
        hrms.run_waitMe($('#chamCongLogDataTable'));
    }

    this.doAftersearch = function () {
        InitDataTable();
        hrms.hide_waitMe($('#chamCongLogDataTable'));
    }

    function InitDataTable() {
        var table = $('#chamCongLogDataTable');
        if (table) {
            table.DataTable().destroy();
        }

        var table = $('#chamCongLogDataTable').DataTable({
            select: true,
            columnDefs: [{
                targets: 0,
                className: 'dt-body-center',
                render: function (data, type, full, meta) {
                    return '<input type="checkbox" name="id[]" value="' + $('<div/>').text(data).html() + '">';
                }
            }],
            "order": [13, 'asc']
        });
        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="chamCongLogDataTable_length"]').removeClass('form-control-sm');

        // Handle click on "Select all" control
        $('#chamCongLogDataTable-select-all').on('click', function () {
            // Get all rows with search applied
            var rows = table.rows({ 'search': 'applied' }).nodes();
            // Check/uncheck checkboxes for all rows in the table
            $('input[type="checkbox"]', rows).prop('checked', this.checked);
        });

        // Handle click on checkbox to set state of "Select all" control
        $('#chamCongLogDataTable tbody').on('change', 'input[type="checkbox"]', function () {
            // If checkbox is not checked
            if (!this.checked) {
                var el = $('#chamCongLogDataTable-select-all').get(0);
                // If "Select all" control is checked and has 'indeterminate' property
                if (el && el.checked && ('indeterminate' in el)) {
                    // Set visual state of "Select all" control
                    // as 'indeterminate'
                    el.indeterminate = true;
                }
            }
        });
    }
}