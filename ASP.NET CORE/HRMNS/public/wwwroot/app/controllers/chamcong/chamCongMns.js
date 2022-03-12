var chamCongController = function () {
    this.initialize = function () {
        registerEvents();
        initSelectOptionBoPhan();
    }


    function registerEvents() {

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

            $('#lblTitle').text($(this).data('name'));

            $.ajax({
                url: '/Admin/ChamCong/GetLogByUserId',
                type: 'POST',
                dataType: 'json',
                data: {
                    userId: _userId,
                    time: _time
                },
                success: function (data) {

                    var maxLeng = data.length;
                    var dateMin = '';
                    var dateMax = '';
                    var render = "";
                    $.each(data, function (index, value) {
                        if (index == 0) {
                            $('#lblNgayChamCong').text(_time);
                            $('#lblBeginTime').text(value.sLogTime);
                            dateMin = value.sLogTime;
                        }
                        else if (index == maxLeng - 1) {
                            $('#lblEndTime').text(value.sLogTime);
                            dateMax = value.sLogTime;
                        }

                        render += "<li><p class='mb-0'>" + value.sName + "</p><p class='res-activity-time'><i class='fa fa-clock-o'></i>" + (new Date(value.sLogTime)).toLocaleTimeString() + "</p></li>"
                    });

                    var dateTimeMin = new Date(dateMin);
                    var dateTimeMax = new Date(dateMax);
                    var Difference_In_Time = dateTimeMax.getTime() - dateTimeMin.getTime();
                    var Difference_In_Days = Difference_In_Time / (1000 * 3600); // hour in day
                    $('#lblTotalTimeInDay').text(Difference_In_Days.toFixed(2) + ' hrs');
                    $('#slActivity').html(render);
                  
                },
                error: function (status) {
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                }
            });

            $('#attendance_info').modal('show');
        });


    }


    function initSelectOptionBoPhan() {
        $.ajax({
            url: '/Admin/ChamCong/GetDepartment',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var render = "<option value=''>--Select department--</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.sName + "'>" + item.sName + "</option >"
                });
                $('#cboBoPhan').html(render);
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading department data', 'error', 'alert', function () { });
            }
        });
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

        $('#chamCongLogDataTable').DataTable({
            "order": [10, 'asc']
        });
        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="chamCongLogDataTable_length"]').removeClass('form-control-sm');
    }
}