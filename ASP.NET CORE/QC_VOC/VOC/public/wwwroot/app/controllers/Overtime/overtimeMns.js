var overtimeController = function () {
    this.initialize = function () {
        registerEvents();
        initSelectOptionBoPhan();
    }

    function registerEvents() {

        // Open popup add overtime
        $('#btnCreate').on('click', function () {

            resetModelAdd();
            initSelectOptionNhanVien();

            $('#hd_overtime').val('Add');
            $('#_txtMaNV').attr('disabled', false);
            $('#addEditOvertimeModel').modal('show');
        });

        // Click Edit overtime
        $('body').on('click', '.edit-nv-overtime', function (e) {

            e.preventDefault();
            resetModelAdd();
            initSelectOptionNhanVien();

            var that = $(this).data('id');

            $.ajax({
                type: "GET",
                url: "/Admin/DangKyOT/GetById",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (overtime) {
                    if (overtime) {
                        $('#_txtMaNV').val(overtime.MaNV);
                        $('#_txtMaNV').trigger('change');
                        $('#_txtOvertime').val(overtime.NgayOT);
                        $('#_txtId').val(that);
                    }
                    else {
                        hrms.notify('error: Not found overtime!', 'error', 'alert', function () { });
                    }
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });

            $('#hd_overtime').val('Edit');
            $('#_txtMaNV').attr('disabled', true);
            $('#addEditOvertimeModel').modal('show');
        });

        function resetModelAdd() {

            $('#_txtId').val(0);
            $('#_txtMaNV').val('');
            $('#_txtOvertime').val('');
            $('#_txtMaNV').trigger('change');
        }

        // Init data nhan vien
        function initSelectOptionNhanVien() {
            $.ajax({
                url: '/Admin/NhanVien/GetAll',
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (response) {

                    console.log(response);

                    var groupEmp = response.reduce(function (result, current) {
                        result[current.MaBoPhan] = result[current.MaBoPhan] || [];
                        result[current.MaBoPhan].push(current);
                        return result;
                    }, {})

                    console.log(groupEmp);

                    var render = "<option value='' selected='selected'>Select option...</option>";
                    $.each(groupEmp, function (gr, item) {
                        render += "<optgroup label='" + gr + "'>";
                        $.each(item, function (j, sub) {
                            render += "<option value='" + sub.Id + "'>" + sub.Id + "-" + sub.TenNV + "</option>"
                        });
                        render += "</optgroup>"
                    });
                    $('#_txtMaNV').html(render);
                },
                error: function (status) {
                    console.log(status);
                    hrms.notify('Cannot loading nhan vien', 'error', 'alert', function () { });
                }
            });
        }

        // save overtime
        $('#btnSave_Overtime').on('click', function (e) {

            if ($('#frmOvertimeAddEdit').valid()) {
                e.preventDefault();

                var maNv = $('#_txtMaNV').val();
                var ngayOT = $('#_txtOvertime').val();
                var action = $('#hd_overtime').val();
                var code = $('#_txtId').val();

                $.ajax({
                    url: '/Admin/DangKyOT/RegisterOvertime?action=' + action,
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        Id: code,
                        MaNV: maNv,
                        NgayOT: ngayOT
                    },
                    success: function (response) {

                        $('#addEditOvertimeModel').modal('hide');
                        hrms.notify("Update success!", 'Success', 'alert', function () {
                            location.reload();
                        });
                    },
                    error: function (status) {
                        console.log(status.responseText);
                        hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                    }
                });
            }
        });

        // Click delete overtime
        $('body').on('click', '.delete-nv-overtime', function (e) {
            e.preventDefault();

            $('#txtHiddenId').val($(this).data('id'));
            $('#delete_overtime').modal('show');
        });

        // Delete overtime
        $('#btnDeleteOvertime').on('click', function (e) {

            e.preventDefault();

            var code = $('#txtHiddenId').val();

            $.ajax({
                url: '/Admin/DangKyOT/DeleteOvertime',
                type: 'POST',
                dataType: 'json',
                data: {
                    Id: code
                },
                success: function (response) {

                    $('#delete_overtime').modal('hide');
                    hrms.notify("Delete success!", 'Success', 'alert', function () {
                        location.reload();
                    });
                },
                error: function (status) {
                    console.log(status.responseText);
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // open form approve
        $('body').on('click', '.approve-nv-overtime', function (e) {

            e.preventDefault();
            $('#hiIdApprove').val($(this).data('id'));
            $('#approve_overtime').modal('show');
        });

        // open form unapprove
        $('body').on('click', '.unapprove-nv-overtime', function (e) {

            e.preventDefault();
            $('#hiIdUnApprove').val($(this).data('id'));
            $('#unapprove_overtime').modal('show');
        });

        // Approve
        $('#btn-approve_overtime').on('click', function (e) {

            e.preventDefault();

            var _dept = $('#cboDepartment').val();
            var _status = $('#searchStatus').val();
            var code = $('#hiIdApprove').val();

            $.ajax({
                url: '/Admin/DangKyOT/Approve',
                type: 'POST',
                dataType: 'json',
                data: {
                    Id: code,
                    dept: _dept,
                    status: _status,
                    action:'Y'
                },
                success: function (response) {

                    $('#approve_overtime').modal('hide');
                    hrms.notify("Update success!", 'Success', 'alert', function () {
                        location.reload();
                    });
                },
                error: function (status) {
                    console.log(status.responseText);
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // Approve
        $('#btn-Unapprove_overtime').on('click', function (e) {

            e.preventDefault();

            var _dept = $('#cboDepartment').val();
            var _status = $('#searchStatus').val();
            var code = $('#hiIdUnApprove').val();

            $.ajax({
                url: '/Admin/DangKyOT/Approve',
                type: 'POST',
                dataType: 'json',
                data: {
                    Id: code,
                    dept: _dept,
                    status: _status,
                    action: 'N'
                },
                success: function (response) {

                    $('#unapprove_overtime').modal('hide');
                    hrms.notify("Update success!", 'Success', 'alert', function () {
                        location.reload();
                    });
                },
                error: function (status) {
                    console.log(status.responseText);
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // Approve
        $('#btnApprove').on('click', function (e) {

            e.preventDefault();
            $('#hiIdApprove').val(0);

            if ($('#cboDepartment').val() == '' || $('#searchStatus').val() != 'N') {
                hrms.notify("Duyệt theo phòng ban có status = 'Not approved yet'", 'error', 'alert', function () { });
            }
            else
            {
                $('#approve_overtime').modal('show');
            }
        });

        // UnApprove
        $('#btnUnApprove').on('click', function (e) {

            e.preventDefault();
            $('#hiIdUnApprove').val(0);

            if ($('#cboDepartment').val() == '' || $('#searchStatus').val() != 'Y') {
                hrms.notify("Bỏ duyệt theo phòng ban có status = 'Approved'", 'error', 'alert', function () { });
            }
            else
            {
                $('#unapprove_overtime').modal('show');
            }
        });

        // Import excel
        // 1. import overtime
        $('#btn-importOvertime').on('click', function () {
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val('');
            $('#import_OvertimeModel').modal('show');
        });

        // Close import
        $('#btnCloseImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                $("#fileInputExcel").val(null);
                $('#hd-ImportType').val('');
                $('#import_OvertimeModel').modal('hide');
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
                $('#import_OvertimeModel').modal('hide');
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
                url: '/Admin/DangKyOT/ImportExcel?param=' + type,
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    $('#import_OvertimeModel').modal('hide');
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
    }

    function initSelectOptionBoPhan() {
        $.ajax({
            url: '/Admin/BoPhan/GetAll',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var render = "<option value=''>--Select department--</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.TenBoPhan + "</option>"
                });
                $('#cboDepartment').html(render);
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading department data', 'error', 'alert', function () { });
            }
        });
    }

    this.doAftersearch = function () {
        InitDataTable();
    }

    function InitDataTable() {
        var table = $('#overtimeDataTable');
        if (table) {
            table.DataTable().destroy();
        }

        $('#overtimeDataTable').DataTable({
            "order": [6, 'asc']
        });
        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="overtimeDataTable_length"]').removeClass('form-control-sm');
    }
}