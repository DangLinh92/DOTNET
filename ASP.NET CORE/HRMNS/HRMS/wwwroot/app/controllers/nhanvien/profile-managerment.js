var ProfileMng = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });

        $("#fileInputImage").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var idNhanVien = $('#txtMaNhanVien').data('id');
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            data.append('category', 'avatar');
            data.append('Id', idNhanVien);

            // upload to server
            $.ajax({
                type: "POST",
                url: "/admin/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    // save image 
                    $.ajax({
                        type: "POST",
                        url: "/admin/nhanvien/SaveAvatar",
                        dataType: 'json',
                        data: {
                            Id: idNhanVien,
                            url: path
                        },
                        success: function (status) {
                            hrms.notify("Upload success!", 'Success', 'alert', function () {
                                $("#fileInputImage").val(null);
                                $('#imgAvatar').attr('src', path);
                            });
                        },
                        error: function (status) {
                            hrms.notify('There was error save image!' + status.responseText, 'error', 'alert', function () { });
                        }
                    });
                },
                error: function () {
                    hrms.notify('There was error uploading image!', 'error', 'alert', function () { });
                }
            });
        });

        // Open popup edit profile basic
        $('#btn-UpdateProfileBasic').on('click', function (e) {
            e.preventDefault();

            resetFormData();
            initSelectOptionBoPhan();
            initSelectOptionBoPhanDetail();
            initSelectOptionChucDanh();

            $('#profile_info').modal('show');

            var that = $(this).data('id');

            $.ajax({
                type: "GET",
                url: "/Admin/NhanVien/GetProfile",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (nhanVienProfile) {
                    if (nhanVienProfile) {

                        $('#txtMaNhanVien').val(nhanVienProfile.MaNhanVien);
                        $('#txtTenNhanVien').val(nhanVienProfile.TenNhanVien);
                        $('#txtBirthday').val(nhanVienProfile.Birthday);
                        $('#txtGioiTinh').val(nhanVienProfile.GioiTinh);

                        $('#txtBoPhan').val(nhanVienProfile.BoPhan);
                        $('#txtBoPhan').trigger('change');

                        $('#txtBoPhanDetail').val(nhanVienProfile.MaBoPhanDetail);
                        $('#txtBoPhanDetail').trigger('change');

                        $('#txtChucDanh').val(nhanVienProfile.ChucDanh);
                        $('#txtChucDanh').trigger('change');

                        $('#txtNgayVaoCongTy').val(nhanVienProfile.NgayVaoCongTy);
                        $('#txtDCHienTai').val(nhanVienProfile.DCHienTai);
                        $('#txtPhone').val(nhanVienProfile.Phone);
                        $('#txtEmail').val(nhanVienProfile.Email);
                        $('#txtStatus').val(nhanVienProfile.Status);
                        $('#txtStatus').trigger('change');
                    }
                    else {
                        hrms.notify('error: Not found employee!', 'error', 'alert', function () { });
                    }
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        })

        function resetFormData() {
            $('#txtMaNhanVien').val('');
            $('#txtTenNhanVien').val('');
            $('#txtBirthday').val('');
            $('#txtGioiTinh').val('Male');
            $('#txtBoPhan').val('');
            $('#txtBoPhanDetail').val('');
            $('#txtChucDanh').val('');
            $('#txtNgayVaoCongTy').val('');
            $('#txtDCHienTai').val('');
            $('#txtPhone').val('');
            $('#txtEmail').val('');
            $('#txtStatus').val('Active');
        }

        function initSelectOptionBoPhanDetail() {
            $.ajax({
                url: '/Admin/BoPhanDetail/GetAll',
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (response) {
                    var render = "<option value=''>--Select department--</option>";
                    $.each(response, function (i, item) {
                        render += "<option value='" + item.Id + "'>" + item.TenBoPhanChiTiet + "</option >"
                    });
                    $('#txtBoPhanDetail').html(render);
                },
                error: function (status) {
                    console.log(status);
                    hrms.notify('Cannot loading department detail data', 'error', 'alert', function () { });
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
    }
}