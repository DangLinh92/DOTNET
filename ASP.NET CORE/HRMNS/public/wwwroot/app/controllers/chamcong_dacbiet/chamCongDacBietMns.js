var chamcongDacBietController = function () {

    this.initialize = function () {
        registerEvents();
        initSelectOptionBoPhan();
    }

    function registerEvents() {

        // Open popup add overtime
        $('#btnCreate').on('click', function () {

            resetModelAdd();

            initSelectOptionNhanVien();
            initSelectOptionChamCongChiTiet();

            $('#hd_chamCongDB').val('Add');
            $('#_txtMaNV').attr('disabled', false);
            $('#chkall').attr('disabled', false);
            $('#addEditChamCongDacBietModel').modal('show');
        });

        // Click Edit 
        $('body').on('click', '.edit-ChamCongDB', function (e) {

            e.preventDefault();
            resetModelAdd();
            initSelectOptionNhanVien();
            initSelectOptionChamCongChiTiet();

            var that = $(this).data('id');

            $.ajax({
                type: "GET",
                url: "/Admin/ChamCongDacBiet/GetById",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (object) {
                    if (object) {
                        $('#_txtMaNV').val(object.MaNV);
                        $('#_txtMaNV').trigger('change');

                        $('#_txtId').val(that);
                        $('#_txtMaChamCongChiTiet').val(object.MaChamCong_ChiTiet);
                        $('#_txtMaChamCongChiTiet').trigger('change');
                        $('#_txtNgayBatDau').val(object.NgayBatDau);
                        $('#_txtNgayKetThuc').val(object.NgayKetThuc);
                        $('#_txtContent').val(object.NoiDung);
                    }
                    else {
                        hrms.notify('error: Not found data!', 'error', 'alert', function () { });
                    }
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });

            $('#hd_chamCongDB').val('Edit');
            $('#_txtMaNV').attr('disabled', true);
            $('#chkall').attr('disabled', true);
            $('#addEditChamCongDacBietModel').modal('show');
        });

        // save data
        $('#btnSave_ChamCongDB').on('click', function (e) {

            if ($('#frmChamCongDacBietAddEdit').valid()) {
                e.preventDefault();

                var maNv = $('#_txtMaNV').val();
                var dmChamCong = $('#_txtMaChamCongChiTiet').val();
                var fromTime = $('#_txtNgayBatDau').val();
                var toTime = $('#_txtNgayKetThuc').val();
                var content = $('#_txtContent').val();
                var action = $('#hd_chamCongDB').val();
                var id = $('#_txtId').val();

                let arrManv = [];
                $.each(maNv, function (index, value) {
                    if (value) {
                        let nv = {
                            MaNV: value,
                            MaChamCong_ChiTiet: dmChamCong,
                            NgayBatDau: fromTime,
                            NgayKetThuc: toTime,
                            NoiDung: content,
                            Id: id
                        };

                        arrManv.push(nv);
                    }
                    return arrManv;
                });

                $.ajax({
                    url: '/Admin/ChamCongDacBiet/RegisterChamCongDB?action=' + action,
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        arrManv: arrManv
                    },
                    success: function (response) {

                        $('#addEditChamCongDacBietModel').modal('hide');

                        hrms.notify("Update success!", 'Success', 'alert', function () {
                            location.reload();
                        });
                    },
                    error: function (status) {
                        hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                    }
                });
            }
        });

        // show form delete overtime
        $('body').on('click', '.delete-ChamCongDB', function (e) {
            e.preventDefault();

            $('#txtHiddenId').val($(this).data('id'));
            $('#delete_chamcongDB').modal('show');
        });

        // Delete
        $('#btnChamCongDB').on('click', function (e) {

            e.preventDefault();

            var code = $('#txtHiddenId').val();

            $.ajax({
                url: '/Admin/ChamCongDacBiet/Delete',
                type: 'POST',
                dataType: 'json',
                data: {
                    Id: code
                },
                success: function (response) {

                    $('#delete_chamcongDB').modal('hide');
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

        // Approve
        $('#btnApprove').on('click', function (e) {

            e.preventDefault();
            $('#hiIdApprove').val(0);
            $('#approve_overtime').modal('show');
        });

        // UnApprove
        $('#btnUnApprove').on('click', function (e) {

            e.preventDefault();
            $('#hiIdUnApprove').val(0);
            $('#unapprove_overtime').modal('show');
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
            else
            {
                ids.push(code);
            }

            if (ids.length == 0) {
                hrms.notify('error: Choose item for approve', 'error', 'alert', function () { });
                return;
            }

            $.ajax({
                url: '/Admin/ChamCongDacBiet/ApproveAction',
                type: 'POST',
                dataType: 'json',
                data: {
                    lstID: ids,
                    action: 'approve'
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

        // UnApprove
        $('#btn-Unapprove_overtime').on('click', function (e) {

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
            else
            {
                ids.push(code);
            }

            if (ids.length == 0) {
                hrms.notify('error: Choose item for unapprove', 'error', 'alert', function () { });
                return;
            }

            $.ajax({
                url: '/Admin/ChamCongDacBiet/ApproveAction',
                type: 'POST',
                dataType: 'json',
                data: {
                    lstID: ids,
                    action: 'unapprove'
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

        function resetModelAdd() {
            $('#_txtId').val(0);
            $('#_txtMaNV').val('');
            $('#_txtMaChamCongChiTiet').val('');
            $('#_txtMaNV').trigger('change');
            $('#_txtMaChamCongChiTiet').trigger('change');
            $('#_txtNgayBatDau').val('');
            $('#_txtNgayKetThuc').val('');
            $('#_txtContent').val('');
        }

        // Init data nhan vien
        function initSelectOptionNhanVien() {
            $.ajax({
                url: '/Admin/NhanVien/GetAll',
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (response) {

                    var groupEmp = response.reduce(function (result, current) {
                        result[current.MaBoPhan] = result[current.MaBoPhan] || [];
                        result[current.MaBoPhan].push(current);
                        return result;
                    }, {})

                    var render = "<option value='' selected='selected'>Select option...</option>";
                    $.each(groupEmp, function (gr, item) {
                        if (deparment != '') {
                            if (deparment == gr) {
                                render += "<optgroup label='" + gr + "'>";
                                $.each(item, function (j, sub) {
                                    render += "<option value='" + sub.Id + "'>" + sub.Id + "-" + sub.TenNV + "</option>"
                                });
                                render += "</optgroup>"
                            }
                        }
                        else {
                            render += "<optgroup label='" + gr + "'>";
                            $.each(item, function (j, sub) {
                                render += "<option value='" + sub.Id + "'>" + sub.Id + "-" + sub.TenNV + "</option>"
                            });
                            render += "</optgroup>"
                        }
                    });
                    $('#_txtMaNV').html(render);
                },
                error: function (status) {
                    console.log(status);
                    hrms.notify('Cannot loading nhan vien', 'error', 'alert', function () { });
                }
            });
        }

        // Init data cham cong chi tiet
        function initSelectOptionChamCongChiTiet() {
            $.ajax({
                url: '/Admin/ChamCongDacBiet/GetDmucChamCongChiTiet',
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (response) {

                    var groupEmp = response.reduce(function (result, current) {
                        result[current.DM_DANGKY_CHAMCONG.TieuDe] = result[current.DM_DANGKY_CHAMCONG.TieuDe] || [];
                        result[current.DM_DANGKY_CHAMCONG.TieuDe].push(current);
                        return result;
                    }, {})

                    var render = "<option value='' selected='selected'>Select option...</option>";
                    $.each(groupEmp, function (gr, item) {
                        render += "<optgroup label='" + gr + "'>";
                        $.each(item, function (j, sub) {
                            render += "<option value='" + sub.Id + "'>" + sub.TenChiTiet + '-' + sub.KyHieuChamCong + "</option>"
                        });
                        render += "</optgroup>"
                    });
                    $('#_txtMaChamCongChiTiet').html(render);
                },
                error: function (status) {
                    console.log(status);
                    hrms.notify('Cannot loading ma cham cong chi tiet', 'error', 'alert', function () { });
                }
            });
        }

        // Import excel
        // 1. import overtime
        $('#btn-importChamCongDB').on('click', function () {
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
                url: '/Admin/ChamCongDacBiet/ImportExcel?param=' + type,
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

                if (deparment != '' && deparment != 'SP') {
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

    function InitDataTable() {
        var table = $('#chamCongDacBietDataTable');
        if (table) {
            table.DataTable().destroy();
        }

        var table = $('#chamCongDacBietDataTable').DataTable({
            scrollY: 600,
            scrollX: true,
            scrollCollapse: true,
            paging: false,
            select: true,
            "searching": true,
            initComplete: function () {
                this.api().columns([1, 2, 3, 4, 5, 6, 8, 9, 10]).every(function () {
                    var column = this;
                    var select = $('<select><option value="">All</option></select>')
                        .appendTo($(column.header()))
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );

                            column
                                .search(val ? '^' + val + '$' : '', true, false)
                                .draw();
                        });

                    column.data().unique().sort().each(function (d, j) {
                        if (d == '<span class="badge bg-inverse-success">Yes</span>') {
                            select.append('<option value="Yes">Yes</option>')
                        }
                        else if (d == '<span class="badge bg-inverse-warning">Request</span>') {
                            select.append('<option value="Request">Request</option>')
                        }
                        else if (d == '<span class="badge bg-inverse-danger">No</span>') {
                            select.append('<option value="No">No</option>')
                        }
                        else {
                            select.append('<option value="' + d + '">' + d + '</option>')
                        }
                    });
                });
            }
            , columnDefs: [{
                targets: 0,
                className: 'dt-body-center',
                render: function (data, type, full, meta) {
                    return '<input type="checkbox" name="id[]" value="' + $('<div/>').text(data).html() + '">';
                }
            }],
            "order": [[10, 'asc']]
        });
        table.columns.adjust().draw();
        table.order([11, 'asc']).draw();

        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="chamCongDacBietDataTable_length"]').removeClass('form-control-sm');

        // Handle click on "Select all" control
        $('#chamCongDacBietDataTable-select-all').on('click', function () {
            // Get all rows with search applied
            var rows = table.rows({ 'search': 'applied' }).nodes();
            // Check/uncheck checkboxes for all rows in the table
            $('input[type="checkbox"]', rows).prop('checked', this.checked);
        });

        // Handle click on checkbox to set state of "Select all" control
        $('#chamCongDacBietDataTable tbody').on('change', 'input[type="checkbox"]', function () {
            // If checkbox is not checked
            if (!this.checked) {
                var el = $('#chamCongDacBietDataTable-select-all').get(0);
                // If "Select all" control is checked and has 'indeterminate' property
                if (el && el.checked && ('indeterminate' in el)) {
                    // Set visual state of "Select all" control
                    // as 'indeterminate'
                    el.indeterminate = true;
                }
            }
        });
    }

    this.beginSearch = function () {
        hrms.run_waitMe($('#chamCongDacBietDataTable'));
    }

    this.doAftersearch = function () {
        InitDataTable();
        hrms.hide_waitMe($('#chamCongDacBietDataTable'));
    }

}