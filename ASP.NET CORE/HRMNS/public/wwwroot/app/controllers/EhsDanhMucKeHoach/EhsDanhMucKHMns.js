var EhsDMKeHoachController = function () {

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {

        // delete chi tiet noi dung.
        $('#btnDelete_NoiDungChiTiet').on('click', function (e) {
            e.preventDefault();

            let Id = $('#hd_IdNoiDungChiTiet').val();
           
            $.ajax({
                type: "POST",
                url: "/Admin/EhsDanhMucKeHoach/DeleteNoiDungChiTiet",
                dataType: "json",
                data: {
                    Id: Id
                },
                success: function (response) {
                    hrms.notify("Update success!", 'Success', 'alert', function () {
                        $('#addEditNoiDungChiTietModel').modal('hide');
                        ItemClickNoiDung(response.MaNoiDung);
                    });
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // edit chi tiet noi dung.
        $('#btnSave_NoiDungChiTiet').on('click', function (e) {
            e.preventDefault();

            let Id = $('#hd_IdNoiDungChiTiet').val();
            let nhaThau = $('#_txtNhaThau').val();
            let chuky = $('#_txtChuKy').val() + '/' + $('#_txtDonViChuKy').val();
            let vitri = $('#_txtViTri').val();
            let soLuong = $('#_txtSoLuong').val();
            let NgayKiemDinh = $('#_txtNgayThucHien').val();
            let thoiGianHuanLuyen = $('#_txtThoiGian_ThucHien').val();
            let yeuCau = $('#_txtYeuCau').val();
            let ngayKhaiBaoTB = $('#_txtNgayKhaiBaoThietBi').val();
            let thongBaoTruoc = $('#_txtThoiGianThongBao').val() + '/' + $('#_txtDonViThoiGianThongBao').val();

            let maHieuMayKiemTra = $('#_txtMaHieuMayKiemTra').val();
            let tienDoHoanThanh =  $('#_txtTienDoHoanThanh').val();
            let soTien = $('#_txtSoTien').val();
            let ketQua = $('#_txtKetQua').val();

            $.ajax({
                type: "POST",
                url: "/Admin/EhsDanhMucKeHoach/UpdateNoiDungChiTiet",
                dataType: "json",
                data: {
                    Id: Id,
                    NhaThau: nhaThau,
                    ChuKy: chuky,
                    ViTri: vitri,
                    SoLuong: soLuong,
                    NgayThucHien: NgayKiemDinh,
                    ThoiGian_ThucHien: thoiGianHuanLuyen,
                    YeuCau: yeuCau,
                    NgayKhaiBaoThietBi: ngayKhaiBaoTB,
                    ThoiGianThongBao: thongBaoTruoc,
                    MaHieuMayKiemTra: maHieuMayKiemTra,
                    TienDoHoanThanh: tienDoHoanThanh,
                    SoTien: soTien,
                    KetQua: ketQua
                },
                success: function (response) {
                    hrms.notify("Update success!", 'Success', 'alert', function () {
                        $('#addEditNoiDungChiTietModel').modal('hide');
                        ItemClickNoiDung(response.MaNoiDung);
                    });
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        $('body').on('click', '.edit-noidungKH', function (e) {
            e.preventDefault();

            $('#addEditNoiDungChiTietModel').modal('show');
            var maNoiDungChiTiet = $(this).data('id');

            $.ajax({
                type: "POST",
                url: "/Admin/EhsDanhMucKeHoach/GetNoiDungChiTietById",
                dataType: "json",
                data: {
                    Id: maNoiDungChiTiet
                },
                success: function (noidung) {
                    if (noidung) {

                        $('#hd_IdNoiDungChiTiet').val(noidung.Id);

                        $('#_txtNoiDung').val(noidung.EHS_NOIDUNG.NoiDung);
                        $('#_txtNhaThau').val(noidung.NhaThau);

                        if (noidung.ChuKy.split('/').length > 1) {
                            let chuKy = noidung.ChuKy.split('/')[0];
                            let donvi = noidung.ChuKy.split('/')[1];
                            $('#_txtChuKy').val(chuKy);
                            $('#_txtDonViChuKy').val(donvi);
                            $('#_txtDonViChuKy').trigger('change');
                        }
                        
                        $('#_txtViTri').val(noidung.ViTri);
                        $('#_txtSoLuong').val(noidung.SoLuong);
                        $('#_txtNgayThucHien').val(noidung.NgayThucHien);
                        $('#_txtThoiGian_ThucHien').val(noidung.ThoiGian_ThucHien);
                        $('#_txtYeuCau').val(noidung.YeuCau);
                        $('#_txtNgayKhaiBaoThietBi').val(noidung.NgayKhaiBaoThietBi);

                        $('#_txtMaHieuMayKiemTra').val(noidung.MaHieuMayKiemTra);
                        $('#_txtTienDoHoanThanh').val(noidung.TienDoHoanThanh);
                        $('#_txtSoTien').val(noidung.SoTien);
                        $('#_txtKetQua').val(noidung.KetQua);

                        if (noidung.ThoiGianThongBao.split('/').length > 1) {
                            let tgthongBao = noidung.ThoiGianThongBao.split('/')[0];
                            let donviTB = noidung.ThoiGianThongBao.split('/')[1];

                            $('#_txtThoiGianThongBao').val(tgthongBao);
                            $('#_txtDonViThoiGianThongBao').val(donviTB);
                            $('#_txtDonViThoiGianThongBao').trigger('change');
                        }
                    }
                    else {
                        hrms.notify('error: Not found content detail!', 'error', 'alert', function () { });
                    }
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        $('#btnSaveNoiDungKeHoach').on('click', function (e) {

            if ($('#frmNoiDungKeHoach').valid()) {
                e.preventDefault();

                var maNoiDung = $('#txtNoiDungKeHoachId').val();
                var noidung = $('#txtNoiDungKeHoach').val();

                $.ajax({
                    url: '/admin/EhsDanhMucKeHoach/UpdateNoiDung',
                    type: 'POST',
                    data: {
                        maNoiDung: maNoiDung,
                        noidung: noidung
                    },
                    success: function () {
                        hrms.notify("Update success!", 'Success', 'alert', function () {
                            $('#update_NoiDungkehoach').modal('hide');
                            window.location.reload();
                        });
                    },
                    error: function (status) {
                        console.log(status.responseText);
                        hrms.notify('error:' + status.responseText, 'error', 'alert', function () {
                        });
                    }
                });
            }
        });

        $('#btnDelete_NoiDungKH').on('click', function (e) {

            if ($('#frmNoiDungKeHoach').valid()) {
                e.preventDefault();

                var maNoiDung = $('#txtNoiDungKeHoachId').val();

                $.ajax({
                    url: '/admin/EhsDanhMucKeHoach/DeleteNoiDung',
                    type: 'POST',
                    data: {
                        maNoiDung: maNoiDung
                    },
                    success: function () {
                        hrms.notify("Update success!", 'Success', 'alert', function () {
                            $('#update_NoiDungkehoach').modal('hide');
                            window.location.reload();
                        });
                    },
                    error: function (status) {
                        console.log(status.responseText);
                        hrms.notify('error:' + status.responseText, 'error', 'alert', function () {
                        });
                    }
                });
            }
        });

        $('#btnSaveDemucKeHoach').on('click', function (e) {

            if ($('#frmDemucKeHoach').valid()) {
                e.preventDefault();

                var maDemuc = $('#txtDemucKeHoachId').val();
                var tenDemuc = $('#txtTenDemucKeHoach_VN').val();
                var luatDinh = $('#txtLuatDinhLquanDeMuc_Edit').val();

                $.ajax({
                    url: '/admin/EhsDanhMucKeHoach/UpdateDemucKeHoach',
                    type: 'POST',
                    data: {
                        maDemuc: maDemuc,
                        tenDemuc: tenDemuc,
                        luatDinh: luatDinh
                    },
                    success: function () {
                        hrms.notify("Update success!", 'Success', 'alert', function () {
                            $('#update_Demuckehoach').modal('hide');
                            window.location.reload();
                        });
                    },
                    error: function (status) {
                        console.log(status.responseText);
                        hrms.notify('error:' + status.responseText, 'error', 'alert', function () {
                        });
                    }
                });
            }
        });

        $('#btnDeleteDemucKeHoach').on('click', function (e) {

            if ($('#frmDemucKeHoach').valid()) {
                e.preventDefault();

                var maDemuc = $('#txtDemucKeHoachId').val();

                $.ajax({
                    url: '/admin/EhsDanhMucKeHoach/DeleteDemucKeHoach',
                    type: 'POST',
                    data: {
                        maDemuc: maDemuc
                    },
                    success: function () {
                        hrms.notify("Update success!", 'Success', 'alert', function () {
                            $('#update_Demuckehoach').modal('hide');
                            window.location.reload();
                        });
                    },
                    error: function (status) {
                        console.log(status.responseText);
                        hrms.notify('error:' + status.responseText, 'error', 'alert', function () {
                        });
                    }
                });
            }
        });

        $('#btnShowAddKH').on('click', function () {
            $('#txtTenKeHoach_VN').val('');
            $('#txtTenKeHoach_KR').val('');
            $('#txtLuatDinhLquan').val('');
            $('#add_kehoach').modal('show');
        });

        $('#btnSaveNewKeHoach').on('click', function (e) {

            if ($('#frmAddKeHoach').valid()) {
                e.preventDefault();

                var nameVN = $('#txtTenKeHoach_VN').val();
                var nameKR = $('#txtTenKeHoach_KR').val();
                var luatDinh = $('#txtLuatDinhLquan').val();

                $.ajax({
                    url: '/admin/EhsDanhMucKeHoach/AddKeHoach',
                    type: 'POST',
                    data: {
                        nameVN: nameVN,
                        nameKR: nameKR,
                        luatDinh: luatDinh
                    },
                    success: function () {
                        hrms.notify("Update success!", 'Success', 'alert', function () {
                            $('#add_kehoach').modal('hide');
                            window.location.reload();
                        });
                    },
                    error: function (status) {
                        console.log(status.responseText);
                        hrms.notify('error:' + status.responseText, 'error', 'alert', function () {
                        });
                    }
                });
            }
        });

        $('#btnDeleteKeHoach').on('click', function () {

           var maKH = $('#txtKeHoachId_Delete').val();
            $.ajax({
                url: '/admin/EhsDanhMucKeHoach/DeleteKeHoach',
                type: 'POST',
                data: {
                    maKeHoach: maKH
                },
                success: function () {
                    hrms.notify("Delete success!", 'Success', 'alert', function () {
                        $('#delete_KeHoach').modal('hide');
                        window.location.reload();
                    });
                },
                error: function (status) {
                    console.log(status.responseText);
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () {
                    });
                }
            });
        });

        // Import excel

        $('#btn-importNoiDung').on('click', function () {
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val('IMPORT_NOIDUNG_CHITIET');
            $('#import_DeMucKeHoachModel').modal('show');
        });

        // 1. import overtime
        $('#btn-importDemuc').on('click', function () {
            $("#fileInputExcel").val(null);
            $('#import_DeMucKeHoachModel').modal('show');
        });

        // Close import
        $('#btnCloseImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                $("#fileInputExcel").val(null);
                $('#import_DeMucKeHoachModel').modal('hide');
                location.reload();
            }
        });

        // Close import
        $('#btnCloseImport').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                $("#fileInputExcel").val(null);
                $('#import_DeMucKeHoachModel').modal('hide');
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
                url: '/Admin/EhsDanhMucKeHoach/ImportExcel?param=' + type,
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    $('#import_DeMucKeHoachModel').modal('hide');
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

        // get file template noi dung
        $('#btn-getFileNoiDung').on('click', function (e) {

            e.preventDefault();
            var maKH = $('.roles-menu>ul li.active').data('val');

            $.ajax({
                url: '/admin/EhsDanhMucKeHoach/GetFileNoiDungChiTiet',
                type: 'POST',
                data: {
                    maKeHoach: maKH
                },
                success: function (url) {
                    window.location.href = url;
                },
                error: function (status) {
                    console.log(status.responseText);
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () {
                    });
                }
            });
        });
    }
}