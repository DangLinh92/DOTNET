﻿var thaisanController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('#btn-import').on('click', function () {
            $("#fileInputExcel").val(null);
            $('#import_File').modal('show');
        });

        $('#btnCloseImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                $("#fileInputExcel").val(null);
                $('#hd-ImportData').val('');
                $('#import_File').modal('hide');
                location.reload();
            }
        });

        $('#btnCloseImport').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                $("#fileInputExcel").val(null);
                $('#hd-ImportData').val('');
                $('#import_File').modal('hide');
                location.reload();
            }
        });

        $('#btnImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }

            $.ajax({
                url: '/Admin/NhanVienThaiSan/ImportThaiSanExcel',
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    $('#import_File').modal('hide');
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

        // Export excel start
        $('#btnExport').on('click', function () {

            var maNV = $('#txtMaNV').val();
            var fromTime = $('#txtTimeFrom').val();
            var toTime = $('#txtTimeTo').val();

            $.ajax({
                type: "POST",
                url: "/Admin/NhanVienThaiSan/ExportExcel",
                data: {
                    maNV: maNV,
                    timeFrom: fromTime,
                    timeTo: toTime
                },
                beforeSend: function () {
                    hrms.run_waitMe($('#gridThaisanIndex'));
                },
                success: function (response) {
                    window.location.href = response;
                    hrms.hide_waitMe($('#gridThaisanIndex'));
                },
                error: function () {
                    hrms.notify('Has an error in progress!', 'error', 'alert', function () { });
                    hrms.hide_waitMe($('#gridThaisanIndex'));
                }
            });
        });

        $('#btnCreate').on('click', function () {
            resetFormData();
            $('#hd_thaisan').val('Add');
            initSelectOptionNhanVien();
            $('#add_thaisanModel').modal('show');
        });

        $('body').on('click', '.edit-nv-thaisan', function (e) {
            e.preventDefault();

            resetFormData();
            initSelectOptionNhanVien();
            $('#hd_thaisan').val('Edit');
            $('#add_thaisanModel').modal('show');

            var that = $(this).data('id');

            $.ajax({
                type: "GET",
                url: "/Admin/NhanVienThaiSan/GetById",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (ts) {
                    if (ts) {
                        $('#_txtId').val(that);
                        $('#_txtMaNV').val(ts.MaNV);
                        $('#_txtCheDo').val(ts.CheDoThaiSan);
                        $('#_txtFrom').val(ts.FromDate);
                        $('#_txtTo').val(ts.ToDate);

                        $('#_txtCheDo').trigger('change');
                        $('#_txtMaNV').trigger('change');
                    }
                    else {
                        hrms.notify('error: Not found data!', 'error', 'alert', function () { });
                    }
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        $('#btnSaveThaiSan').on('click', function (e) {
            if ($('#frmThaisan_AddEdit').valid()) {

                e.preventDefault();

                let action = $('#hd_thaisan').val();

                $.ajax({
                    url: '/Admin/NhanVienThaiSan/SaveThaiSan?action=' + action,
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        Id: $('#_txtId').val(),
                        MaNV: $('#_txtMaNV').val(),
                        CheDoThaiSan: $('#_txtCheDo').val(),
                        FromDate: $('#_txtFrom').val(),
                        ToDate: $('#_txtTo').val()
                    },
                    success: function (response) {
                        $('#add_thaisanModel').modal('hide');
                        hrms.notify("Update success!", 'Success', 'alert', function () {
                            window.location.reload();
                        });
                    },
                    error: function (status) {
                        console.log(status.responseText);
                        hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                    }
                });
            }
        });

        $('body').on('click', '.delete-nv-thaisan', function (e) {
            e.preventDefault();
            $('#hdId_delete').val($(this).data('id'));
            $('#delete_nhanVien_ts').modal('show');
        });

        $('#btn-delete_nv_ts').on('click', function (e) {
            e.preventDefault();
            let id = $('#hdId_delete').val();

            $.ajax({
                url: '/Admin/NhanVienThaiSan/Delete',
                type: 'POST',
                data: {
                    id: id
                },
                success: function (response) {
                    $('#delete_nhanVien_ts').modal('hide');
                    hrms.notify("Delete success!", 'Success', 'alert', function () {
                        window.location.reload();
                    });
                },
                error: function (status) {
                    console.log(status.responseText);
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        function resetFormData() {
            $('#hd_thaisan').val('');
            $('#_txtId').val('0');
            $('#_txtMaNV').val('');
            $('#_txtCheDo').val('');
            $('#_txtFrom').val('');
            $('#_txtTo').val('');
        }

        // Init data nhan vien
        function initSelectOptionNhanVien() {
            $.ajax({
                url: '/Admin/NhanVien/GetAllActive',
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

        this.doAftersearch = function () {
            InitDataTable();
        }

        function InitDataTable() {
            var table1 = $('#thaisanDataTable');
            if (table1) {
                table1.DataTable().destroy();
            }

            var table = $('#thaisanDataTable');
            if (table) {
                table.DataTable().destroy();
                $('#thaisanDataTable').DataTable({
                    "order": [6, 'asc']
                });
                $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search.');
                $('select[name="thaisanDataTable_length"]').removeClass('form-control-sm');
            }
        }
    }
}