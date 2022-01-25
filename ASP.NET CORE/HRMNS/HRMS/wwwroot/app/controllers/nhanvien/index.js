var nhanVienController = function () {
    this.initialize = function () {
        // loadData();
    }

    function registerEvents() {
        // bindding event
    }

    // ADD EDIT START
    // Open popup Add employee
    $('#btnCreate').on('click', function () {
        $('#txtTitleAddEdit').text('Add Employee');
        $("#txtMaNV").prop('disabled', false);

        resetFormData();
        initSelectOptionBoPhan();
        initSelectOptionChucDanh();
        $('#add_employee').modal('show');
    });

    // Click Edit employee
    $('body').on('click', '.edit-employee', function (e) {
        e.preventDefault();

        $('#txtTitleAddEdit').text('Edit Employee');
        $("#txtMaNV").prop('disabled', true);

        resetFormData();
        initSelectOptionBoPhan();
        initSelectOptionChucDanh();
        $('#add_employee').modal('show');

        var that = $(this).data('id');

        $.ajax({
            type: "GET",
            url: "/Admin/NhanVien/GetById",
            dataType: "json",
            data: {
                Id: that
            },
            success: function (nhanvien) {
                if (nhanvien) {
                    $('#txtTenNV').val(nhanvien.TenNV);
                    $('#txtGioiTinh').val(nhanvien.GioiTinh);
                    $('#txtEmail').val(nhanvien.Email);
                    $('#txtSoDienThoai').val(nhanvien.SoDienThoai);
                    $('#txtMaNV').val(nhanvien.Id);
                    $('#txtNgayVao').val(nhanvien.NgayVao);
                    $('#txtBoPhan').val(nhanvien.MaBoPhan);
                    $('#txtBoPhan').trigger('change');
                    $('#txtChucDanh').val(nhanvien.MaChucDanh);
                    $('#txtChucDanh').trigger('change');
                }
                else {
                    hrms.notify('error: Not found employee!', 'error', 'alert', function () { });
                }
            },
            error: function (status) {
                hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
            }
        });
    });

    // Click save popup
    $('#btnSave').on('click', function (e) {

        if ($('#frmAddEditEmployee').valid()) {

            e.preventDefault();

            var tenNV = $('#txtTenNV').val();
            var gioiTinh = $('#txtGioiTinh').val();
            var email = $('#txtEmail').val();
            var phone = $('#txtSoDienThoai').val();
            var maNV = $('#txtMaNV').val();
            var joinDate = $('#txtNgayVao').val();
            var boPhan = $('#txtBoPhan').val();
            var chucDanh = $('#txtChucDanh').val();

            var action = '';
            var title = $('#txtTitleAddEdit').text();
            if (title == 'Edit Employee') {
                action = 'Edit';
            }
            else {
                action = 'Add';
            }

            $.ajax({
                url: '/Admin/NhanVien/SaveEmployee',
                type: 'POST',
                dataType: 'json',
                data: {
                    Action: action,
                    NhanVien: {
                        Id: maNV,
                        TenNV: tenNV,
                        MaChucDanh: chucDanh,
                        MaBoPhan: boPhan,
                        GioiTinh: gioiTinh,
                        Email: email,
                        SoDienThoai: phone,
                        NgayVao: joinDate
                    }
                },
                success: function (response) {
                    $('#add_employee').modal('hide');
                    hrms.notify("Update success!", 'Success', 'alert', function () {

                        // update grid data ,update datatable jquery
                        ReloadData();
                    });
                },
                error: function (status) {
                    console.log(status.responseText);
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                }
            });
        }
    });
    // ADD EDIT END

    // DELETE START
    $('#btnDelete').on('click', function (e) {

        e.preventDefault();
        var that = $('#txtId').val();
        $.ajax({
            type: "POST",
            url: "/Admin/NhanVien/Delete",
            dataType: "json",
            data: {
                Id: that
            },
            success: function (response) {
                $('#delete_employee').modal('hide');
                hrms.notify("Delete success!", 'Success', 'alert', function () {
                    ReloadData();
                });
            },
            error: function (status) {
                hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
            }
        });
    });

    $('body').on('click', '.delete-employee', function (e) {
        e.preventDefault();
        $('#delete_employee').modal('show');
        $('#txtId').val($(this).data('id'));
    });
    // DELETE END

    // IMPORT EXCEL START
    $('#btn-import').on('click', function () {
        $("#fileInputExcel").val(null);
        $('#import_employee').modal('show');
    });

    $('#btnCloseImportExcel').on('click', function () {
        var fileUpload = $("#fileInputExcel").get(0);
        var files = fileUpload.files;
        if (files.length > 0) {
            $("#fileInputExcel").val(null);
            $('#import_employee').modal('hide');
            location.reload();
        }
    });

    $('#btnCloseImport').on('click', function () {
        var fileUpload = $("#fileInputExcel").get(0);
        var files = fileUpload.files;
        if (files.length > 0) {
            $("#fileInputExcel").val(null);
            $('#import_employee').modal('hide');
            location.reload();
        }
    });

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

        $.ajax({
            url: '/Admin/NhanVien/ImportExcel',
            type: 'POST',
            data: fileData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            success: function (data) {
                $('#import_employee').modal('hide');
                hrms.notify("Import success!", 'Success', 'alert', function () {

                    // update grid data ,update datatable jquery
                    // ReloadData();
                    location.reload();
                });
            },
            error: function (status) {
                hrms.notify('error: Import error!', 'error', 'alert', function () { });
            }
        });
        return false;
    });
    // IMPORT EXCEL END

    // Export excel start
    $('#btn-export').on('click', function () {
        $.ajax({
            type: "POST",
            url: "/Admin/NhanVien/ExportExcel",
            beforeSend: function () {
                hrms.run_waitMe($('#gridNhanVien'));
            },
            success: function (response) {
                window.location.href = response;
                hrms.hide_waitMe($('#gridNhanVien'));
            },
            error: function () {
                hrms.notify('Has an error in progress!', 'error', 'alert', function () { });
                hrms.hide_waitMe($('#gridNhanVien'));
            }
        });
    });
    // Export excel end

    function resetFormData() {
        $('#txtTenNV').val('');
        $('#txtGioiTinh').val('Male');
        $('#txtEmail').val('');
        $('#txtSoDienThoai').val('');
        $('#txtMaNV').val('');
        $('#txtNgayVao').val('');
        $('#txtBoPhan').val('');
        $('#txtChucDanh').val('');
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
                    render += "<option value='" + item.Id + "'>" + item.TenBoPhan + "</option >"
                });
                $('#txtBoPhan').html(render);
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading department data', 'error', 'alert', function () { });
            }
        });
    }

    function initSelectOptionChucDanh() {
        $.ajax({
            url: '/Admin/ChucDanh/GetAll',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var render = "<option value=''>--Select team position--</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.TenChucDanh + "</option >"
                });
                $('#txtChucDanh').html(render);
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading team position data', 'error', 'alert', function () { });
            }
        });
    }

    function loadData() {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: '/Admin/NhanVien/GetAll',
            dataType: 'json',
            success: function (response) {
                $.each(response.Data, function (i, item) {
                    render += Mustache.render(template, {
                        Avatar: item.Image == null ? "/img/profiles/avatar-01.jpg" : item.Image,
                        MaNV: item.Id,
                        TenNV: item.TenNV,
                        ChucDanh: hrms.nullString(item.HR_CHUCDANH?.TenChucDanh),
                        BoPhan: item.MaBoPhan,
                        NgaySinh: item.NgaySinh,
                        Email: item.Email,
                        Phone: item.SoDienThoai,
                        NgayVaoCty: item.NgayVao,
                        Status: item.Status,
                        IconStatus: item.Status == "Active" ? '<i class="fa fa-dot-circle-o text-success"></i>' : '<i class="fa fa-dot-circle-o text-danger"></i>'
                    });

                    if (render != '') {
                        $('#tbl-content').html(render);
                    }
                });
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading data', 'error', 'alert', function () { });
            }
        })
    }

    function ReloadData() {
        // update grid data ,update datatable jquery
        hrms.run_waitMe($('#gridNhanVien'));

        $('#btnSearch').submit();
        let myVar = setInterval(function () {
            var table = $('#nhanVienDataTable');
            if (table) {
                table.DataTable().destroy();
                $('#nhanVienDataTable').DataTable({
                    "order": [8, 'asc']
                });
                $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Anything you want.');
                $('select[name="nhanVienDataTable_length"]').removeClass('form-control-sm');
                clearInterval(myVar);
                hrms.hide_waitMe($('#gridNhanVien'));
            }
        }, 500);
    }
}