var nhanVienController = function () {
    this.initialize = function () {
        // loadData();
    }

    function registerEvents() {
        // bindding event
    }

    $('#btnCreate').on('click', function () {
        resetFormData();
        initSelectOptionBoPhan();
        initSelectOptionChucDanh();
        $('#add_employee').modal('show');
    });

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
}