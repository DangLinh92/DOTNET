var nhanvien_calamviecController = function () {
    this.initialize = function () {
        registerEvents();

    }

    function registerEvents() {

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
            initSelectDMCalamviec();
            $('#add_nhanvien_calamviecModel').modal('show');
        });

        // Reset data
        function ResetFormAddNhanVienClviec() {
            $('#_txtMaNV').val('');
            $('#_txtDmCaLviec').val('');
            $('#_txtFrom').val('');
            $('#_txtTo').val('');
        }

        // Init data dm ca lam viec
        function initSelectDMCalamviec() {
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
                    $('#_txtDmCaLviec').html(render);
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

                    var groupEmp = response.reduce(function (result, current) {
                        result[current.MaBoPhan] = result[current.MaBoPhan] || [];
                        result[current.MaBoPhan].push(current);
                        return result;
                    }, {})

                    var render = "<option value='' selected='selected'>Select option...</option>";
                    $.each(groupEmp, function (i, item) {
                        render += "<optgroup label='" + Object.keys(groupEmp)[i] + "'>";
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
}