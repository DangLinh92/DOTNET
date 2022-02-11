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
            var idNhanVien = $('#hdMaNV').data('id');
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
                        $('#txtGioiTinh').trigger('change');

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

                        $('#txtTinhTrangHonNhan').val(nhanVienProfile.TinhTrangHonNhan);
                        $('#txtTinhTrangHonNhan').trigger('change');
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

        function resetFormPersonalInfoData() {
            $('#id-Personal').val('');
            $('#txtCMTND').val('');
            $('#txtNgayCapCMTND').val('');
            $('#txtNoiCapCMTND').val('');
            $('#txtDanToc').val('');
            $('#txtTonGiao').val('');
            $('#txtNoiSinh').val('');
            $('#txtNguyenQuan').val('');
            $('#txtDiaChiThuongTru').val('');
            $('#txtMaSoThue').val('');
            $('#txtTruongDaoTao').val('');
        }

        // Open popup personal info
        $('#btn-UpdatePersonalInfo').on('click', function (e) {
            e.preventDefault();

            resetFormPersonalInfoData();

            $('#personal_info_modal').modal('show');

            var that = $('#btn-UpdateProfileBasic').data('id');

            $.ajax({
                type: "GET",
                url: "/Admin/NhanVien/GetProfile",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (nhanVienProfile) {
                    if (nhanVienProfile) {
                        $('#id-Personal').val(that);
                        $('#txtCMTND').val(nhanVienProfile.CMTND);
                        $('#txtNgayCapCMTND').val(nhanVienProfile.NgayCapCMTND);
                        $('#txtNoiCapCMTND').val(nhanVienProfile.NoiCapCMTND);
                        $('#txtDanToc').val(nhanVienProfile.DanToc);
                        $('#txtTonGiao').val(nhanVienProfile.TonGiao);
                        $('#txtNoiSinh').val(nhanVienProfile.NoiSinh);
                        $('#txtNguyenQuan').val(nhanVienProfile.NguyenQuan);
                        $('#txtDiaChiThuongTru').val(nhanVienProfile.DiaChiThuongTru);
                        $('#txtMaSoThue').val(nhanVienProfile.MaSoThue);
                        $('#txtTruongDaoTao').val(nhanVienProfile.TruongDaoTao);
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

        // Open popup bank info
        $('#btn-UpdateBankInfo').on('click', function (e) {
            e.preventDefault();

            resetFormBankInfoData();

            $('#bank_info_modal').modal('show');

            var that = $('#btn-UpdateProfileBasic').data('id');

            $.ajax({
                type: "GET",
                url: "/Admin/NhanVien/GetProfile",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (nhanVienProfile) {
                    if (nhanVienProfile) {
                        $('#id-BankInfo').val(that);
                        $('#txtTenNganHang').val(nhanVienProfile.TenNganHang);
                        $('#txtSoTaiKhoanNH').val(nhanVienProfile.SoTaiKhoanNH);
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

        function resetFormBankInfoData() {
            $('#id-BankInfo').val('');
            $('#txtTenNganHang').val('');
            $('#txtSoTaiKhoanNH').val('');
        }

        // Update emergency contact
        $('#btn-UpdateEmergency').on('click', function (e) {
            e.preventDefault();

            resetFormEmergencyInfoData();

            $('#emergency_contact_modal').modal('show');

            var that = $('#btn-UpdateProfileBasic').data('id');

            $.ajax({
                type: "GET",
                url: "/Admin/NhanVien/GetProfile",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (nhanVienProfile) {
                    if (nhanVienProfile) {
                        $('#id-Emergency').val(that);
                        $('#txtQuanHeNguoiThan').val(nhanVienProfile.QuanHeNguoiThan);
                        $('#txtSoDienThoaiNguoiThan').val(nhanVienProfile.SoDienThoaiNguoiThan);
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

        function resetFormEmergencyInfoData() {
            $('#id-Emergency').val('');
            $('#txtQuanHeNguoiThan').val('');
            $('#txtSoDienThoaiNguoiThan').val('');
        }

        // Update BHXH
        $('#btn-UpdateBHXH').on('click', function (e) {
            e.preventDefault();

            resetFormBHXH();

            $('#bhxh_infor_modal').modal('show');
            var that = $('#btn-UpdateProfileBasic').data('id');

            $.ajax({
                type: "GET",
                url: "/Admin/NhanVien/GetProfile",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (nhanVienProfile) {
                    if (nhanVienProfile) {
                        $('#id-Bhxh').val(that);
                        $('#txtMaBHXH').val(nhanVienProfile.MaBHXH);
                        $('#txtNgayThamGia').val(nhanVienProfile.NgayThamGia);
                        $('#txtNgayKetThuc').val(nhanVienProfile.NgayKetThuc);
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

        function resetFormBHXH() {
            $('#id-Bhxh').val('');
            $('#txtMaBHXH').val('');
            $('#txtNgayThamGia').val('');
            $('#txtNgayKetThuc').val('');
        }

        // Update Phep Nam
        $('#btn-UpdatePhepNam').on('click', function (e) {
            e.preventDefault();
            $('#phepNam_infor_modal').modal('show');
        });

        // Update ngay nghi viec
        $('#btn-updateQuitwork').on('click', function (e) {
            e.preventDefault();

            resetFormQuitWork();

            $('#quitWork_infor_modal').modal('show');

            var that = $('#btn-UpdateProfileBasic').data('id');

            $.ajax({
                type: "GET",
                url: "/Admin/NhanVien/GetProfile",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (nhanVienProfile) {
                    if (nhanVienProfile) {
                        $('#hd-QuitWork').val(that);
                        $('#txtNgayNghiViec').val(nhanVienProfile.NgayNghiViec);
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

        function resetFormQuitWork() {
            $('#hd-QuitWork').val('');
            $('#txtNgayNghiViec').val('');
        }

        // Update ky luat ld
        $('#btn-Update-KyLuatLaoDong').on('click', function (e) {

            e.preventDefault();

            resetFormKyLuatLD();

            $('#LaborDiscipline_infor_modal').modal('show');

            var that = $('#btn-UpdateProfileBasic').data('id');

            $.ajax({
                type: "GET",
                url: "/Admin/NhanVien/GetProfile",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (nhanVienProfile) {
                    if (nhanVienProfile) {
                        $('#id-KyLuatLD').val(that);
                        $('#txtKyLuatLD').val(nhanVienProfile.KyLuatLaoDong);
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

        function resetFormKyLuatLD() {
            $('#id-KyLuatLD').val('');
            $('#txtKyLuatLD').val('');
        }

        $('#btnClearContract').on('click', function (e) {
            e.preventDefault();
            resetContractInfo();
        });

        function resetContractInfo() {
            $('#txtMaHD').val('');
            $('#txtIdContract').val('');
            $('#txtTenHD').val('');
            $('#slLoaiHD').val(0);
            $('#slLoaiHD').trigger('change');
            $('#txtNgayKy').val('');
            $('#txtNgayHieuLuc').val('');
            $('#txtNgayHetHieuLuc').val('');
        }

        // delete area qua trinh ctac
        $('body').on('click', '.btn-delete-qtrinhCtac', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $('#id-form-qtrinhCtac').attr("data-ajax-url", "/admin/NhanVien/UpdateViewQuatrinhCtac?id=" + id)
            $('#id-form-qtrinhCtac').attr("data-ajax-update", "#quatrinhCtac_content_modal")
            $('#id-form-qtrinhCtac').attr("data-ajax-success", "reloadJs")
            $('#id-form-qtrinhCtac').attr("data-ajax-confirm","Are you sure you want delete this?")

            //$('#btn-submit-qtrCongTac').submit();
            $('#btnSubmitQtct').submit();
        });

        // add new area qua trinh ctac
        $('body').on('click', '.btn-add-Area-qtCtac', function (e) {
            e.preventDefault();
            $('#id-form-qtrinhCtac').attr("data-ajax-url", "/admin/NhanVien/UpdateViewQuatrinhCtac?id=-9999")
            $('#id-form-qtrinhCtac').attr("data-ajax-update", "#quatrinhCtac_content_modal")
            $('#id-form-qtrinhCtac').attr("data-ajax-success", "reloadJs")
            $('#id-form-qtrinhCtac').removeAttr("data-ajax-confirm")

            //$('#btn-submit-qtrCongTac').submit();
            $('#btnSubmitQtct').submit();
        });

        // save data qua trinh cong tac
        $('body').on('click', '#btn-submit-qtrCongTac', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $('#id-form-qtrinhCtac').attr("data-ajax-url", "/admin/NhanVien/UpdateQuatrinhCtac?id=" + id)
            $('#id-form-qtrinhCtac').attr("data-ajax-update", "#quatrinhCongTacTab")
            $('#id-form-qtrinhCtac').attr("data-ajax-success", "ReloadPageOnsuccess")
            $('#id-form-qtrinhCtac').attr("data-ajax-confirm", "Are you sure you want update this aaa?")
            $('#btnSubmitQtct').submit();
        });
    }

    this.InitLoaiHopDong = function () {
        $.ajax({
            url: '/Admin/HRLoaiHD/GetAll',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var render = "<option value='0'>--Chọn Loại Hợp Đồng--</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.TenLoaiHD + "</option >"
                });
                $('#slLoaiHD').html(render);
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading contract type data', 'error', 'alert', function () { });
            }
        });
    }
}