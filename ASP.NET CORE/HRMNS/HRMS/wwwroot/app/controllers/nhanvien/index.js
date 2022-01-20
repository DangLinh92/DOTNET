const { type } = require("jquery");

var nhanVienController = function () {
    this.initialize = function () {
        // loadData();
    }

    function registerEvents() {
        // bindding event
    }

    // Open popup Add employee
    $('#btnCreate').on('click', function () {
        resetFormData();
        initSelectOptionBoPhan();
        initSelectOptionChucDanh();
        $('#add_employee').modal('show');
    });

    // Click Edit employee
    $('body').on('click', '.edit-employee', function (e) {
        e.preventDefault();

        var that = $(this).data('id');

        $.ajax({
            type: "GET",
            url: "/Admin/NhanVien/GetById",
            dataType: "json",
            data: {
                id:that
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

            $.ajax({
                url: '/Admin/NhanVien/SaveEmployee',
                type: 'POST',
                dataType: 'json',
                data: {
                    Action: 'Add',
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
                    hrms.notify("Thêm mới thành công!", 'Success', 'alert', function () {

                        // update grid data ,update datatable jquery
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
                            }
                        }, 500);
                    });
                },
                error: function (status) {
                    console.log(status.responseText);
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                }
            });
        }
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