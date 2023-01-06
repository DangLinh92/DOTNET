var GocPlanController = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {

        // IMPORT EXCEL START
        $('#btn-import-standarQty').on('click', function () {
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val('STANDAR_QTY');
            $('#import_gocPlan').modal('show');
        });

        $('#btn-import-GocPlan').on('click', function () {
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val('GOC_PLAN');
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
                url: '/OpeationMns/GOCPlan/ImportExcel?param=' + type,
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

        // 
        $('body').on('click', '.delete-GocPlan', function (e) {

            var _standarQtyId = $(this).data('id');
            var _fromDate = $(this).data('fromdate');
            var _toDate = $(this).data('todate');
            var _model = $(this).data('mdel');

            $('#txtHiddenId').val(_standarQtyId);
            $('#txtFromDate').val(_fromDate);
            $('#txtToDate').val(_toDate);
            $('#headerDelete').text('Bạn có muốn xóa model và kế hoạch của model ' + _model + '?');

            $('#delete_PlanGOC').modal('show');
        });

        $('#btnDeletePlanGOC').on('click', function (e) {

            let _id = $('#txtHiddenId').val();
            let _from = $('#txtFromDate').val();
            let _to = $('#txtToDate').val();

            $.ajax({
                url: '/OpeationMns/GOCPlan/DeleteGocPlan',
                type: 'POST',
                dataType: 'json',
                data:
                {
                    standarQtyId: _id,
                    from: _from,
                    to: _to,
                },
                success: function (data) {
                    hrms.notify("Delete success!", 'Success', 'alert', function () {
                        $('#btnSearch').submit();
                    });
                },
                error: function (status) {
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                }
            });

            $('#delete_PlanGOC').modal('hide');
        });
    }

    this.beginSearch = function () {

        console.log('beginSearch');
        hrms.run_waitMe($('#gocPlanDataTable'));
    }

    this.doAftersearch = function () {

        console.log('doAftersearch');
        InitDataTable();
        hrms.hide_waitMe($('#gocPlanDataTable'));
    }

    function InitDataTable() {
        console.log('init datatable');

        var table = $('#gocPlanDataTable');
        if (table) {
            table.DataTable().destroy();
        }

        $('#gocPlanDataTable').DataTable({
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
        $('select[name="gocPlanDataTable_length"]').removeClass('form-control-sm');
    }
}