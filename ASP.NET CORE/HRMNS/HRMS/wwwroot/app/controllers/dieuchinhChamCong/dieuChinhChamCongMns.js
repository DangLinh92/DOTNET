var dieuchinhChamcongController = function () {

    this.initialize = function () {
        registerEvents();
        initSelectOptionBoPhan();
    }

    function registerEvents() {

        // Show and update danh muc dieu chinh cham cong
        // Open popup add dieu chinh cham cong
        //$('#btnCreateDmDCChamCong').on('click', function (e) {

        //    e.preventDefault();

        //    initDanhMucDieuChinh(true);

        //    $('#hd_dmdieuChinhChamCong').val('Add');
        //    $('#_txtTieuDe').val('');
        //    $('#_txtTieuDe').attr('disabled', false);
        //    $('#addDMDieuChinhChamCongModel').modal('show');
        //});

        function initDanhMucDieuChinh(isDMDC) {

            $.ajax({
                url: '/Admin/DCChamCong/GetDanhMucDieuChinh',
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (response) {
                    var render = "<option value='' selected>Choose...</option>";
                    $.each(response, function (i, item) {
                        render += "<option value='" + item.Id + "'>" + item.TieuDe + "</option>"
                    });

                    if (isDMDC) {
                        $('#_txtDMDieuChinhCong').html(render);
                    }
                    else {
                        $('#_txtDM_DieuChinhCong').html(render);
                    }
                },
                error: function (status) {
                    console.log(status);
                    hrms.notify('Cannot loading data', 'error', 'alert', function () { });
                }
            });
        }

        $("#_txtDMDieuChinhCong").on('change', function (e) {

            if ($('#_txtDMDieuChinhCong').val() == '') {
                $('#_txtTieuDe').val('');
                $('#_txtTieuDe').attr('disabled', false);
            }
            else {
                $('#_txtTieuDe').val($('#_txtDMDieuChinhCong option:selected').text());
                $('#_txtTieuDe').attr('disabled', true);
            }
        });

        $('#btnEditDMDChinh').on('click', function (e) {
            $('#_txtTieuDe').attr('disabled', false);
        });

        $('#btnSaveDMDieuChinhChamCong').on('click', function (e) {

            if ($('#frmDMDCChamCongAddEdit').valid()) {
                e.preventDefault();

                var id = $('#_txtDMDieuChinhCong').val();
                var tieude = $('#_txtTieuDe').val();

                $.ajax({
                    url: '/Admin/DCChamCong/SaveDanhMucDieuChinh',
                    type: 'POST',
                    dataType: 'json',
                    data:
                    {
                        Id: id,
                        TieuDe: tieude
                    },
                    async: false,
                    success: function (response) {
                        $('#addDMDieuChinhChamCongModel').modal('hide');
                        hrms.notify("update success!", 'Success', 'alert', function () {
                        });
                    },
                    error: function (status) {
                        hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                    }
                });
            }
        });
        // Show and update danh muc dieu chinh cham cong

        // add update dieu chinh cham cong
        $('#btnCreate').on('click', function () {

            resetModelAdd();

            initSelectOptionNhanVien();
            initDanhMucDieuChinh(false);

            $('#hd_dieuChinhChamCong').val('Add');
            $('#_txtMaNV').attr('disabled', false);
            $('#addDieuChinhChamCongModel').modal('show');
        });

        // Click Edit 
        $('body').on('click', '.edit-DCChamCong', function (e) {

            e.preventDefault();
            resetModelAdd();
            initSelectOptionNhanVien();
            initDanhMucDieuChinh(false);

            var that = $(this).data('id');

            $.ajax({
                type: "GET",
                url: "/Admin/DCChamCong/GetById",
                dataType: "json",
                data:
                {
                    Id: that
                },
                success: function (object) {
                    if (object)
                    {
                        $('#_txtId').val(that);

                        $('#_txtMaNV').val(object.MaNV);
                        $('#_txtMaNV').trigger('change');

                        $('#_txtDM_DieuChinhCong').val(object.DM_DieuChinhCong);
                        $('#_txtDM_DieuChinhCong').trigger('change');

                        $('#_txtNgayCanDieuChinh_From').val(object.NgayCanDieuChinh_From);
                        $('#_txtNgayCanDieuChinh_To').val(object.NgayCanDieuChinh_To);
                        $('#_txtGiaTriBoXung').val(object.GiaTriBoXung);
                        $('#_txtContent').val(object.NoiDungDC);

                        $('#_txtTrangThaiChiTra').val(object.TrangThaiChiTra);
                        $('#_txtTrangThaiChiTra').trigger('change');
                    }
                    else
                    {
                        hrms.notify('error: Not found data!', 'error', 'alert', function () { });
                    }
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });

            $('#hd_dieuChinhChamCong').val('Edit');
            $('#_txtMaNV').attr('disabled', true);
            $('#addDieuChinhChamCongModel').modal('show');
        });

        function resetModelAdd() {

            $('#_txtId').val(0);

            $('#_txtMaNV').val('');
            $('#_txtMaNV').trigger('change');

            $('#_txtNgayDieuChinh').val('');
            $('#_txtNgayCong').val('');
            $('#_txtDSNS').val('');

            $('#_txtTrangThaiChiTra').val('');
            $('#_txtTrangThaiChiTra').trigger('change');
        }

        // lưu dieu chinh cham cong
        $('#btnSaveDieuChinhChamCong').on('click', function (e) {

            if ($('#frmDCChamCongAddEdit').valid()) {

                e.preventDefault();

                var mnNV = $('#_txtMaNV').val();
                var dmdieuchinh = $('#_txtDM_DieuChinhCong').val();
                var ngaydcFrom = $('#_txtNgayCanDieuChinh_From').val();
                var ngaydcTo = $('#_txtNgayCanDieuChinh_To').val();
                var giatri = $('#_txtGiaTriBoXung').val();
                var content = $('#_txtContent').val();
                var status = $('#_txtTrangThaiChiTra').val();
                var action = $('#hd_dieuChinhChamCong').val();
                var id = $('#_txtId').val();

                $.ajax({
                    url: '/Admin/DCChamCong/SaveDieuChinhChamCong?action=' + action,
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        Id: id,
                        MaNV: mnNV,
                        NgayCanDieuChinh_From: ngaydcFrom,
                        NgayCanDieuChinh_To: ngaydcTo,
                        NoiDungDC: content,
                        DM_DieuChinhCong: dmdieuchinh,
                        GiaTriBoXung: giatri,
                        TrangThaiChiTra: status
                    },
                    success: function (response) {

                        $('#addDieuChinhChamCongModel').modal('hide');

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
        $('body').on('click', '.delete-DCChamCong', function (e) {
            e.preventDefault();

            $('#txtHiddenId').val($(this).data('id'));
            $('#delete_DCChamCong').modal('show');
        });

        // Delete
        $('#btnDeleteDCChamCong').on('click', function (e) {

            e.preventDefault();

            var code = $('#txtHiddenId').val();

            $.ajax({
                url: '/Admin/DCChamCong/Delete',
                type: 'POST',
                dataType: 'json',
                data: {
                    Id: code
                },
                success: function (response) {

                    $('#delete_DCChamCong').modal('hide');
                    hrms.notify("Delete success!", 'Success', 'alert', function () {
                        location.reload();
                    });
                },
                error: function (status) {
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // Init data nhan vien
        function initSelectOptionNhanVien() {
            $.ajax({
                url: '/Admin/NhanVien/GetAllActive',
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
        var table = $('#dcChamCongDataTable');
        if (table) {
            table.DataTable().destroy();
        }

        $('#dcChamCongDataTable').DataTable({
            "order": [9, 'asc']
        });
        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="dcChamCongDataTable_length"]').removeClass('form-control-sm');
    }

    this.beginSearch = function () {
        hrms.run_waitMe($('#dcChamCongDataTable'));
    }

    this.doAftersearch = function () {
        InitDataTable();
        hrms.hide_waitMe($('#dcChamCongDataTable'));
    }
}