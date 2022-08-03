var nhanvien_calamviecController = function () {
    this.initialize = function () {
        registerEvents();
        initSelectOptionBoPhan();
        // initActiveTime();
    }

    function registerEvents() {

        $('#btn-exportShift').on('click', function () {

            let department = $('#cboDepartment').val();
            let status = $('#searchStatus').val();
            let timeFrom = $('#searchFromTime').val();
            let timeTo = $('#searchToTime').val();

            $.ajax({
                type: "POST",
                data:
                {
                    department: department,
                    status: status,
                    timeFrom: timeFrom,
                    timeTo: timeTo
                },
                url: "/Admin/NhanVien_CaLamViec/ExportExcel",
                beforeSend: function () {
                    hrms.run_waitMe($('#gridNvienCalamviecIndex'));
                },
                success: function (response) {
                    window.location.href = response;
                    hrms.hide_waitMe($('#gridNvienCalamviecIndex'));
                },
                error: function () {
                    hrms.notify('Has an error in progress!', 'error', 'alert', function () { });
                    hrms.hide_waitMe($('#gridNvienCalamviecIndex'));
                }
            });
        });

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

                        if (response.CaLV_DB) {
                            $('#_txtDmCaLviec').val(response.CaLV_DB);
                        }
                        else {
                            $('#_txtDmCaLviec').val(response.Danhmuc_CaLviec);
                        }

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
        //$('#btn-registTimeShift').on('click', function (e) {
        //    ResetFormSettingTimeCaLviec();
        //    initSelectOptionSettingTimeCaLviec();
        //    initSelectDMCalamviec('1');
        //    $('#hd_Time_Calviec').val('Add');
        //    $('#settingTimeCalamviecModel').modal('show');
        //});

        //function ResetFormSettingTimeCaLviec() {
        //    $('#_txtIdSetting').val('0');
        //    $('#_txtSettingDmCaLviec').val('');
        //    $('#_txtSettingDmCaLviec').trigger('change');
        //    $('#_txtSettingTimeTo').val('');
        //    $('#_txtSettingTimeFrom').val('');
        //    $('#_txtSettingTimeRegisFrom').val('');
        //    $('#_txtSettingTimeRegisTo').val('');
        //    $('#hd_Time_Calviec').val('');
        //    $('#_txtSettingStatus').val('');
        //    $('#_txtSettingStatus').trigger('change');
        //    $('#btnAddSettingTime').attr('disabled', true);

        //    $('#_txtSettingDmCaLviec').attr('disabled', false);
        //    $('#_txtSettingTimeTo').attr('disabled', false);
        //    $('#_txtSettingTimeFrom').attr('disabled', false);
        //    $('#_txtSettingTimeRegisFrom').attr('disabled', false);
        //    $('#_txtSettingTimeRegisTo').attr('disabled', false);
        //    $('#_txtSettingStatus').attr('disabled', false);
        //}

        // Init data setting time lam viec
        //function initSelectOptionSettingTimeCaLviec() {
        //    $.ajax({
        //        url: '/Admin/NhanVien_CaLamViec/GetTimeSettingCaLamViec',
        //        type: 'GET',
        //        dataType: 'json',
        //        async: false,
        //        success: function (response) {

        //            var render = "<option value='' selected='selected'>Select option...</option>";
        //            var ids = '';
        //            $.each(response, function (i, item) {
        //                ids = item.Id + '^' + item.CaLamViec + '^' + item.NgayBatDau + '^' + item.NgayKetThuc + '^' + item.NgayBatDauDangKy + '^' + item.NgayKetThucDangKy + '^' + item.Status;
        //                render += "<option value='" + ids + "'>" + item.DM_CA_LVIEC.TenCaLamViec + ' : [' + item.NgayBatDau + ' -> ' + item.NgayKetThuc + ']' + "</option>";
        //            });
        //            $('#_txtTimeSelect').html(render);
        //        },
        //        error: function (status) {
        //            console.log(status);
        //            hrms.notify('Cannot loading setting time', 'error', 'alert', function () { });
        //        }
        //    });
        //}

        // Luu Setting time Ca lam viec
        //$('#btnSave_Setting_Calmviec').on('click', function (e) {

        //    if ($('#frmSettingTime_CaLviec_AddEdit').valid()) {

        //        e.preventDefault();

        //        var calamviec = $('#_txtSettingDmCaLviec').val();
        //        var ngayBatDau = $('#_txtSettingTimeFrom').val();
        //        var ngayKetThuc = $('#_txtSettingTimeTo').val();

        //        var ngaybatdauDangKy = $('#_txtSettingTimeRegisFrom').val();
        //        var ngayKetThucDangKy = $('#_txtSettingTimeRegisTo').val();
        //        var status = $('#_txtSettingStatus').val();

        //        var action = $('#hd_Time_Calviec').val();
        //        var code = $('#_txtIdSetting').val();

        //        $.ajax({
        //            url: '/Admin/NhanVien_CaLamViec/RegisNewShift?action=' + action,
        //            type: 'POST',
        //            dataType: 'json',
        //            data: {
        //                Id: code,
        //                CaLamViec: calamviec,
        //                NgayBatDau: ngayBatDau,
        //                NgayKetThuc: ngayKetThuc,
        //                NgayBatDauDangKy: ngaybatdauDangKy,
        //                NgayKetThucDangKy: ngayKetThucDangKy,
        //                Status: status
        //            },
        //            success: function (response) {

        //                $('#settingTimeCalamviecModel').modal('hide');
        //                hrms.notify("Update success!", 'Success', 'alert', function () {

        //                    location.reload();
        //                });
        //            },
        //            error: function (status) {
        //                console.log(status.responseText);
        //                hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
        //            }
        //        });
        //    }
        //});

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
            $('#approve_nhanVien_calviec').modal('show');
        });

        // UnApprove
        $('#btnUnApprove').on('click', function (e) {

            e.preventDefault();
            $('#hiIdUnApprove').val(0);
            $('#unapprove_nhanVien_calviec').modal('show');
        });

        // Submit approve
        $('#btn-approve_nv_calamviec').on('click', function (e) {

            e.preventDefault();

            var code = $('#hiIdApprove').val();
            var ids = [];

            if (code == '0') {
                // Iterate over all checkboxes in the table
                let arrV = $($.fn.dataTable.tables(true)).DataTable().$('input[type="checkbox"]');
                arrV.each(function () {
                    // If checkbox is checked
                    if (this.checked) {
                        ids.push(this.value);
                    }
                });
            }
            else {
                ids.push(code);
            }

            if (ids.length == 0) {
                hrms.notify('error: Choose item for approve', 'error', 'alert', function () { });
                return;
            }

            $.ajax({
                url: '/Admin/NhanVien_CaLamViec/Approve',
                type: 'POST',
                dataType: 'json',
                data: {
                    lstID: ids,
                    action: 'approve'
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
            var code = $('#hiIdUnApprove').val();

            var ids = [];

            if (code == '0') {
                // Iterate over all checkboxes in the table
                let arrV = $($.fn.dataTable.tables(true)).DataTable().$('input[type="checkbox"]');
                arrV.each(function () {
                    // If checkbox is checked
                    if (this.checked) {
                        ids.push(this.value);
                    }
                });
            }
            else {
                ids.push(code);
            }

            if (ids.length == 0) {
                hrms.notify('error: Choose item for unapprove', 'error', 'alert', function () { });
                return;
            }

            $.ajax({
                url: '/Admin/NhanVien_CaLamViec/Approve',
                type: 'POST',
                dataType: 'json',
                data: {
                    lstID: ids,
                    action: 'unapprove'
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
        var table1 = $('#nvCalamviecDataTable');
        if (table1) {
            table1.DataTable().destroy();
        }

        var table = $('#nvCalamviecDataTable').DataTable({
            select: true,
            "searching": true,
            columnDefs: [{
                targets: 0,
                className: 'dt-body-center',
                render: function (data, type, full, meta) {
                    return '<input type="checkbox" name="id[]" value="' + $('<div/>').text(data).html() + '">';
                }
            }],
            "order": [10, 'asc']
        });
        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="nvCalamviecDataTable_length"]').removeClass('form-control-sm');

        // Handle click on "Select all" control
        $('#nvCalamviecDataTable-select-all').on('click', function () {
            // Get all rows with search applied
            var rows = table.rows({ 'search': 'applied' }).nodes();
            // Check/uncheck checkboxes for all rows in the table
            $('input[type="checkbox"]', rows).prop('checked', this.checked);
        });

        // Handle click on checkbox to set state of "Select all" control
        $('#nvCalamviecDataTable tbody').on('change', 'input[type="checkbox"]', function () {
            // If checkbox is not checked
            if (!this.checked) {
                var el = $('#nvCalamviecDataTable-select-all').get(0);
                // If "Select all" control is checked and has 'indeterminate' property
                if (el && el.checked && ('indeterminate' in el)) {
                    // Set visual state of "Select all" control
                    // as 'indeterminate'
                    el.indeterminate = true;
                }
            }
        });
    }

    function initSelectOptionBoPhan() {
        $.ajax({
            url: '/Admin/BoPhan/GetAll',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {

                if (deparment != '') {
                    var render = "<option value='" + deparment + "'>" + deparment + "</option >";
                    $('#cboDepartment').html(render);
                }
                else {
                    var render = "<option value=''>--Select department--</option>";
                    $.each(response, function (i, item) {
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

    //function initActiveTime() {
    //    $.ajax({
    //        url: '/Admin/NhanVien_CaLamViec/GetActiveTime',
    //        type: 'GET',
    //        dataType: 'json',
    //        async: false,
    //        success: function (response) {
    //            $('#hTimeFrom').text(response.NgayBatDau);
    //            $('#hTimeTo').text(response.NgayKetThuc);

    //            $('#hRegisterTimeFrom').text(response.NgayBatDauDangKy);
    //            $('#hRegisterTimeTo').text(response.NgayKetThucDangKy);

    //            var today = GetToDayDate();
    //            if (today >= response.NgayBatDauDangKy && today <= response.NgayKetThucDangKy)
    //            {
    //                $('#divCreateData').css('display', true);
    //            }
    //            else
    //            {
    //                if (roleUsers.includes('Admin') || roleUsers.includes('HR'))
    //                {
    //                    $('#divCreateData').css('display', true);
    //                }
    //                else
    //                {
    //                    $('#divCreateData').css('display', 'none');
    //                }
    //            }
    //        },
    //        error: function (status) {
    //            console.log(status);
    //            hrms.notify('Cannot loading data', 'error', 'alert', function () { });
    //        }
    //    });
    //}

    function GetToDayDate() {
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
        var yyyy = today.getFullYear();

        return today = yyyy + '-' + mm + '-' + dd;
    }
}