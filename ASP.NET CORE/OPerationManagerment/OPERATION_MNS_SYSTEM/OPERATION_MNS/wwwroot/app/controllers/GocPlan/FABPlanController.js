var FABPlanController = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {

        // IMPORT EXCEL START
        $('#btn-import-standarQty').on('click', function () {
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val('FAB_WLP1');
            $('#import_gocPlan').modal('show');
        });

        $('#btn-import-GocPlan').on('click', function () {
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val('FAB_WLP1');
            $('#import_gocPlan').modal('show');
        });

        $('#btnCloseImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                $("#fileInputExcel").val(null);
                $('#hd-ImportType').val('');
                $('#import_gocPlan').modal('hide');
                location.reload();
            }
        });

        $('#btnCloseImport').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                $("#fileInputExcel").val(null);
                $('#hd-ImportType').val('');
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
            var type = $('#hd-ImportType').val();

            $.ajax({
                url: '/OpeationMns/GOCPlan/Import_WLP1_InputFAB?param=' + type,
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                beforeSend: function () {
                    hrms.run_waitMe($('#import_gocPlan'));
                },
                success: function (data) {
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

    this.beginSearch = function () {

        console.log('beginSearch');
        hrms.run_waitMe($('#FABPlanDataTable'));
    }

    this.doAftersearch = function () {

        console.log('doAftersearch');
        InitDataTable();
        hrms.hide_waitMe($('#FABPlanDataTable'));
    }

    function InitDataTable() {
        console.log('init datatable');

        var table = $('#FABPlanDataTable');
        if (table) {
            table.DataTable().destroy();
        }

        $('#FABPlanDataTable').DataTable({
            scrollY: 530,
            scrollX: true,
            scrollCollapse: true,
            paging: false,
            select: true,
            "searching": true,
            fixedColumns: {
                left: 6
            },
            fixedHeader: true
        });

        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="FABPlanDataTable_length"]').removeClass('form-control-sm');
    }
}