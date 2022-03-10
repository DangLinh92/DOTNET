var nhanvien_calamviecController = function () {
    this.initialize = function () {
        registerEvents();
        initSelectOptionBoPhan();
        initActiveTime();
    }

    function registerEvents() {

        // Import excel
        // 1. import shift
        $('#btn-importShift').on('click', function () {
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val('');
            $('#import_ShiftModel').modal('show');
        });

        // Close import
        $('#btnCloseImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                $("#fileInputExcel").val(null);
                $('#hd-ImportType').val('');
                $('#import_ShiftModel').modal('hide');
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
                $('#import_ShiftModel').modal('hide');
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
                url: '/Admin/NhanVien_CaLamViec/ImportExcel?param=' + type,
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    $('#import_ShiftModel').modal('hide');
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

        // ADD EDIT NHANVIEN CA LAM VIEC
        // Open popup add ca lam viec cho nhan vien
        $('#btnCreate').on('click', function () {
            ResetFormAddNhanVienClviec();
            initSelectOptionNhanVien();
            initSelectDMCalamviec('0');

            $('#hd_NhanVien_Calviec').val('Add');

            $('#add_nhanvien_calamviecModel').modal('show');
        });

        // Reset data
        function ResetFormAddNhanVienClviec() {
            $('#_txtMaNV').val('');
            $('#_txtDmCaLviec').val('');
            $('#_txtFrom').val('');
            $('#_txtTo').val('');
            $('#hd_NhanVien_Calviec').val('');
            $("#_txtMaNV").prop('disabled', false);
            $('#_txtId').val('0');
        }

        // Init data dm ca lam viec
        function initSelectDMCalamviec(setting) {
            $.ajax({
                url: '/Admin/NhanVien_CaLamViec/GetAllDMCaLamViec',
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (response) {

                    var render = "<option value='' selected='selected'>Select option...</option>";
                    $.each(response, function (i, item) {
                        render += "<option value='" + item.Id + "'>" + item.TenCaLamViec + "</option>";
                    });
                    if (setting == '1') {
                        $('#_txtSettingDmCaLviec').html(render);
                    }
                    else {
                        $('#_txtDmCaLviec').html(render);
                    }

                },
                error: function (status) {
                    console.log(status);
                    hrms.notify('Cannot loading ca lam viec', 'error', 'alert', function () { });
                }
            });
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

        // Luu Dang Ky Nhan Vien - Ca Lam Viec
        $('#btnSave_Nv_Calmviec').on('click', function (e) {

            if ($('#frmNhanVien_CaLviec_AddEdit').valid()) {
                e.preventDefault();

                var maNv = $('#_txtMaNV').val();
                var calamviec = $('#_txtDmCaLviec').val();
                var ngayBatDau = $('#_txtFrom').val();
                var ngayKetThuc = $('#_txtTo').val();
                var action = $('#hd_NhanVien_Calviec').val();
                var code = $('#_txtId').val();

                $.ajax({
                    url: '/Admin/NhanVien_CaLamViec/RegisterNhanVienCalamViec?action=' + action,
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        Id: code,
                        MaNV: maNv,
                        Danhmuc_CaLviec: calamviec,
                        BatDau_TheoCa: ngayBatDau,
                        KetThuc_TheoCa: ngayKetThuc
                    },
                    success: function (response) {

                        $('#add_nhanvien_calamviecModel').modal('hide');
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

        // Click Edit Nhan vien ca lam viec
        $('body').on('click', '.edit-nv-calviec', function (e) {
            e.preventDefault();

            ResetFormAddNhanVienClviec();
            initSelectOptionNhanVien();
            initSelectDMCalamviec('0');
            $('#hd_NhanVien_Calviec').val('Edit');
            $("#_txtMaNV").prop('disabled', true);

            $('#add_nhanvien_calamviecModel').modal('show');

            var that = $(this).data('id');

            $.ajax({
                type: "GET",
                url: "/Admin/NhanVien_CaLamViec/FindId",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (response) {
                    if (response) {
                        $('#_txtId').val(that);
                        $('#_txtMaNV').val(response.MaNV);
                        $('#_txtDmCaLviec').val(response.Danhmuc_CaLviec);
                        $('#_txtFrom').val(response.BatDau_TheoCa);
                        $('#_txtTo').val(response.KetThuc_TheoCa);
                        $('#_txtDmCaLviec').trigger('change');
                        $('#_txtMaNV').trigger('change');
                    }
                    else {
                        hrms.notify('error: Not found data row!', 'error', 'alert', function () { });
                    }
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // Show delete nhan vien ca lam viec
        $('body').on('click', '.delete-nv-calviec', function (e) {
            e.preventDefault();
            $('#hdId_delete').val($(this).data('id'));
            $('#delete_nhanVien_calviec').modal('show');
        });

        // Delete nhan vien ca lam viec
        $('#btn-delete_nv_calamviec').on('click', function (e) {
            e.preventDefault();
            var that = $('#hdId_delete').val();
            $.ajax({
                type: "POST",
                url: "/Admin/NhanVien_CaLamViec/Delete",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (response) {
                    if (response) {
                        $('#delete_nhanVien_calviec').modal('hide');
                        hrms.notify("Update success!", 'Success', 'alert', function () {
                            location.reload();
                        });
                    }
                    else {
                        hrms.notify('error: delete not success!', 'error', 'alert', function () { });
                    }
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // Show Dang Ky Ca Lam Viec (admin)
        $('#btn-registTimeShift').on('click', function (e) {
            ResetFormSettingTimeCaLviec();
            initSelectOptionSettingTimeCaLviec();
            initSelectDMCalamviec('1');
            $('#hd_Time_Calviec').val('Add');
            $('#settingTimeCalamviecModel').modal('show');
        });

        function ResetFormSettingTimeCaLviec() {
            $('#_txtIdSetting').val('0');
            $('#_txtSettingDmCaLviec').val('');
            $('#_txtSettingDmCaLviec').trigger('change');
            $('#_txtSettingTimeTo').val('');
            $('#_txtSettingTimeFrom').val('');
            $('#_txtSettingTimeRegisFrom').val('');
            $('#_txtSettingTimeRegisTo').val('');
            $('#hd_Time_Calviec').val('');
            $('#_txtSettingStatus').val('');
            $('#_txtSettingStatus').trigger('change');
            $('#btnAddSettingTime').attr('disabled', true);

            $('#_txtSettingDmCaLviec').attr('disabled', false);
            $('#_txtSettingTimeTo').attr('disabled', false);
            $('#_txtSettingTimeFrom').attr('disabled', false);
            $('#_txtSettingTimeRegisFrom').attr('disabled', false);
            $('#_txtSettingTimeRegisTo').attr('disabled', false);
            $('#_txtSettingStatus').attr('disabled', false);
        }

        // Init data setting time lam viec
        function initSelectOptionSettingTimeCaLviec() {
            $.ajax({
                url: '/Admin/NhanVien_CaLamViec/GetTimeSettingCaLamViec',
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (response) {

                    var render = "<option value='' selected='selected'>Select option...</option>";
                    var ids = '';
                    $.each(response, function (i, item) {
                        ids = item.Id + '^' + item.CaLamViec + '^' + item.NgayBatDau + '^' + item.NgayKetThuc + '^' + item.NgayBatDauDangKy + '^' + item.NgayKetThucDangKy + '^' + item.Status;
                        render += "<option value='" + ids + "'>" + item.DM_CA_LVIEC.TenCaLamViec + ' : [' + item.NgayBatDau + ' -> ' + item.NgayKetThuc + ']' + "</option>";
                    });
                    $('#_txtTimeSelect').html(render);
                },
                error: function (status) {
                    console.log(status);
                    hrms.notify('Cannot loading setting time', 'error', 'alert', function () { });
                }
            });
        }

        // Luu Setting time Ca lam viec
        $('#btnSave_Setting_Calmviec').on('click', function (e) {

            if ($('#frmSettingTime_CaLviec_AddEdit').valid()) {

                e.preventDefault();

                var calamviec = $('#_txtSettingDmCaLviec').val();
                var ngayBatDau = $('#_txtSettingTimeFrom').val();
                var ngayKetThuc = $('#_txtSettingTimeTo').val();

                var ngaybatdauDangKy = $('#_txtSettingTimeRegisFrom').val();
                var ngayKetThucDangKy = $('#_txtSettingTimeRegisTo').val();
                var status = $('#_txtSettingStatus').val();

                var action = $('#hd_Time_Calviec').val();
                var code = $('#_txtIdSetting').val();

                $.ajax({
                    url: '/Admin/NhanVien_CaLamViec/RegisNewShift?action=' + action,
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        Id: code,
                        CaLamViec: calamviec,
                        NgayBatDau: ngayBatDau,
                        NgayKetThuc: ngayKetThuc,
                        NgayBatDauDangKy: ngaybatdauDangKy,
                        NgayKetThucDangKy: ngayKetThucDangKy,
                        Status: status
                    },
                    success: function (response) {

                        $('#settingTimeCalamviecModel').modal('hide');
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

        // change select box setting time
        $('#_txtTimeSelect').on('change', function (e) {
            e.preventDefault();

            // ids = item.Id + '^' + item.CaLamViec + '^' + item.NgayBatDau + '^' + item.NgayKetThuc + '^' + item.NgayBatDauDangKy + '^' + item.NgayKetThucDangKy;
            var ids = $(this).val();

            if (ids.indexOf('^') >= 0) {
                var id = ids.split('^')[0];
                var calviec = ids.split('^')[1];
                var ngaybatdau = ids.split('^')[2];
                var ngayketthuc = ids.split('^')[3];
                var ngaydakybatdau = ids.split('^')[4];
                var ngaydakyKetthuc = ids.split('^')[5];
                var status = ids.split('^')[6];

                $('#_txtIdSetting').val(id);
                $('#_txtSettingDmCaLviec').val(calviec);
                $('#_txtSettingDmCaLviec').trigger('change');
                $('#_txtSettingTimeFrom').val(ngaybatdau);
                $('#_txtSettingTimeTo').val(ngayketthuc);

                $('#_txtSettingTimeRegisFrom').val(ngaydakybatdau);
                $('#_txtSettingTimeRegisTo').val(ngaydakyKetthuc);
                $('#hd_Time_Calviec').val('Edit');
                $('#btnAddSettingTime').attr('disabled', false);

                $('#_txtSettingStatus').val(status);
                $('#_txtSettingStatus').trigger('change');

                $('#_txtSettingDmCaLviec').attr('disabled', true);
                $('#_txtSettingTimeTo').attr('disabled', true);
                $('#_txtSettingTimeFrom').attr('disabled', true);
                $('#_txtSettingTimeRegisFrom').attr('disabled', true);
                $('#_txtSettingTimeRegisTo').attr('disabled', true);
                $('#_txtSettingStatus').attr('disabled', true);
            }
            else {
                $('#_txtIdSetting').val('0');
                $('#_txtSettingDmCaLviec').val('');
                $('#_txtSettingDmCaLviec').trigger('change');
                $('#_txtSettingTimeTo').val('');
                $('#_txtSettingTimeFrom').val('');
                $('#_txtSettingTimeRegisFrom').val('');
                $('#_txtSettingTimeRegisTo').val('');
                $('#hd_Time_Calviec').val('Add');
                $('#btnAddSettingTime').attr('disabled', true);

                $('#_txtSettingStatus').val('');
                $('#_txtSettingStatus').trigger('change');

                $('#_txtSettingDmCaLviec').attr('disabled', false);
                $('#_txtSettingTimeTo').attr('disabled', false);
                $('#_txtSettingTimeFrom').attr('disabled', false);
                $('#_txtSettingTimeRegisFrom').attr('disabled', false);
                $('#_txtSettingTimeRegisTo').attr('disabled', false);
                $('#_txtSettingStatus').attr('disabled', false);
            }
        });

        // clear data to add setting time
        $('#btnAddSettingTime').on('click', function (e) {

            e.preventDefault();
            $('#_txtTimeSelect').val('');
            $('#_txtTimeSelect').trigger('change');
            $('#_txtIdSetting').val('0');
            $('#_txtSettingDmCaLviec').val('');
            $('#_txtSettingDmCaLviec').trigger('change');
            $('#_txtSettingTimeTo').val('');
            $('#_txtSettingTimeFrom').val('');
            $('#_txtSettingTimeRegisFrom').val('');
            $('#_txtSettingTimeRegisTo').val('');
            $('#hd_Time_Calviec').val('Add');

            $('#_txtSettingStatus').val('');
            $('#_txtSettingStatus').trigger('change');

            $('#_txtSettingDmCaLviec').attr('disabled', false);
            $('#_txtSettingTimeTo').attr('disabled', false);
            $('#_txtSettingTimeFrom').attr('disabled', false);
            $('#_txtSettingTimeRegisFrom').attr('disabled', false);
            $('#_txtSettingTimeRegisTo').attr('disabled', false);
            $('#_txtSettingStatus').attr('disabled', false);
        });

        // enable to edit setting time
        $('#btnEditSettingTime').on('click', function (e) {
            e.preventDefault();
            $('#_txtSettingDmCaLviec').attr('disabled', false);
            $('#_txtSettingTimeTo').attr('disabled', false);
            $('#_txtSettingTimeFrom').attr('disabled', false);
            $('#_txtSettingTimeRegisFrom').attr('disabled', false);
            $('#_txtSettingTimeRegisTo').attr('disabled', false);
            $('#_txtSettingStatus').attr('disabled', false);
        });

        // Approve
        $('#btnApprove').on('click', function (e) {

            e.preventDefault();
            $('#hiIdApprove').val(0);

            if ($('#cboDepartment').val() == '' || $('#searchStatus').val() != 'N') {
                hrms.notify("Approve theo phòng ban có status = 'Not approved yet'", 'error', 'alert', function () { });
            }
            else {
                $('#approve_nhanVien_calviec').modal('show');
            }
        });

        // UnApprove
        $('#btnUnApprove').on('click', function (e) {

            e.preventDefault();
            $('#hiIdUnApprove').val(0);

            if ($('#cboDepartment').val() == '' || $('#searchStatus').val() != 'Y') {
                hrms.notify("UnApprove theo phòng ban có status = 'Approved'", 'error', 'alert', function () { });
            }
            else {
                $('#unapprove_nhanVien_calviec').modal('show');
            }
        });

        // Submit approve
        $('#btn-approve_nv_calamviec').on('click', function (e) {

            e.preventDefault();

            var _dept = $('#cboDepartment').val();
            var _status = $('#searchStatus').val();
            var code = $('#hiIdApprove').val();

            $.ajax({
                url: '/Admin/NhanVien_CaLamViec/Approve',
                type: 'POST',
                dataType: 'json',
                data: {
                    dept: _dept,
                    status: _status,
                    Id: code
                },
                success: function (response) {
                    $('#approve_nhanVien_calviec').modal('hide');
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

        // Click approve nhan vien ca lam viec tren gridview
        $('body').on('click', '.approve-nv-calviec', function (e) {
            e.preventDefault();

            $('#hiIdApprove').val($(this).data('id'));
            $('#approve_nhanVien_calviec').modal('show');
        });

         // Click unapprove nhan vien ca lam viec tren gridview
        $('body').on('click', '.unapprove-nv-calviec', function (e) {
            e.preventDefault();

            $('#hiIdUnApprove').val($(this).data('id'));
            $('#unapprove_nhanVien_calviec').modal('show');
        });

        // Submit unapprove
        $('#btn-Unapprove_nv_calamviec').on('click', function (e) {

            e.preventDefault();

            var _dept = $('#cboDepartment').val();
            var _status = $('#searchStatus').val();
            var code = $('#hiIdUnApprove').val();

            $.ajax({
                url: '/Admin/NhanVien_CaLamViec/UnApprove',
                type: 'POST',
                dataType: 'json',
                data: {
                    dept: _dept,
                    status: _status,
                    Id: code
                },
                success: function (response) {
                    $('#unapprove_nhanVien_calviec').modal('hide');
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
    }

    this.doAftersearch = function () {
        InitDataTable();
    }

    function InitDataTable() {
        var table = $('#nvCalamviecDataTable');
        if (table) {
            table.DataTable().destroy();
        }

        $('#nvCalamviecDataTable').DataTable({
            "order": [8, 'asc']
        });
        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="nvCalamviecDataTable_length"]').removeClass('form-control-sm');
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
                $('#cboDepartment').html(render);
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading department data', 'error', 'alert', function () { });
            }
        });
    }

    function initActiveTime() {
        $.ajax({
            url: '/Admin/NhanVien_CaLamViec/GetActiveTime',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                $('#hTimeFrom').text(response.NgayBatDau);
                $('#hTimeTo').text(response.NgayKetThuc);

                $('#hRegisterTimeFrom').text(response.NgayBatDauDangKy)
                $('#hRegisterTimeTo').text(response.NgayKetThucDangKy)
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading data', 'error', 'alert', function () { });
            }
        });
    }
}