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

                $.ajax({
                    url: '/Admin/ChamCongDacBiet/RegisterChamCongDB?action=' + action,
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        MaNV: maNv,
                        MaChamCong_ChiTiet: dmChamCong,
                        NgayBatDau: fromTime,
                        NgayKetThuc: toTime,
                        NoiDung: content,
                        Id: id
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

    function InitDataTable() {
        var table = $('#chamCongDacBietDataTable');
        if (table) {
            table.DataTable().destroy();
        }

        $('#chamCongDacBietDataTable').DataTable({
            "order": [7, 'asc']
        });
        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="chamCongDacBietDataTable_length"]').removeClass('form-control-sm');
    }

    this.beginSearch = function () {
        hrms.run_waitMe($('#chamCongDacBietDataTable'));
    }

    this.doAftersearch = function () {
        InitDataTable();
        hrms.hide_waitMe($('#chamCongDacBietDataTable'));
    }
}