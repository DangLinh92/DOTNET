var WlpStockSapController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {

        // IMPORT EXCEL START

        // import stock wlp2 trên SAP
        $("#btn-import-wlpSap").on('click', function () {
            $("#fileInputExcel").val(null);
            $('#hd-ImportUrl').val('/OpeationMns/GOCModule/ImportWlpStockSapExcel?param=');
            $('#import_gocPlan').modal('show');
        });

        $('#btnCloseImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                $("#fileInputExcel").val(null);
                $('#import_gocPlan').modal('hide');
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
            // Adding one more key to FormData object  
            // fileData.append('categoryId', $('#ddlCategoryIdImportExcel').combotree('getValue'));
            var url = $('#hd-ImportUrl').val();

            $.ajax({
                url: url,//'/OpeationMns/GOCModule/ImportExcel?param=' + type,
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                beforeSend: function () {
                    hrms.run_waitMe($('#import_gocPlan'));
                },
                success: function (data) {

                    $("#fileInputExcel").val(null);
                    $('#import_gocPlan').modal('hide');

                    hrms.hide_waitMe($('#import_gocPlan'));
                    hrms.notify("Import success!", 'Success', 'alert', function () {

                        location.reload();
                    });
                },
                error: function (status) {
                    hrms.notify('error: Import error!' + status.responseText, 'error', 'alert', function () { });
                    hrms.hide_waitMe($('#import_gocPlan'));
                }
            });
            return false;
        });
    }
}