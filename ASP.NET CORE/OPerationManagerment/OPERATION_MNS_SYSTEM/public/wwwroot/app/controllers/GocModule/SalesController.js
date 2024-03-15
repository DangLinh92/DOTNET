var SalesController = function () {
    this.initialize = function () {
        registerEvents();
        initSelectGetPlanIds();
        initSelectGetSites();
    }

    function registerEvents() {
        $('#btn-import-goc_byHand').on('click', function () {
            let masterID = $('#cboMasterID').val();
            let planID = $('#cboPlanID').val();
            let siteID = $('#cboSiteId').val();
            let param = masterID + '^' + planID + '^' + siteID + '^LFEM';
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val(param);
            $('#hd-ImportUrl').val('/OpeationMns/GOCModule/ImportPlanGocWeekExcel?param=');
            $('#import_gocPlan').modal('show');
        });

        $('#btn-import-goc_byHandSmt').on('click', function () {
            let masterID = $('#cboMasterID').val();
            let planID = $('#cboPlanID').val();
            let siteID = $('#cboSiteId').val();
            let param = masterID + '^' + planID + '^' + siteID + '^SMT';
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val(param);
            $('#hd-ImportUrl').val('/OpeationMns/GOCModule/ImportPlanGocWeekExcel?param=');
            $('#import_gocPlan').modal('show');
        });

        // IMPORT EXCEL START
        $('#btn-import-sales').on('click', function () {
            let masterID = $('#cboMasterID').val();
            let planID = $('#cboPlanID').val();
            let siteID = $('#cboSiteId').val();
            let param = masterID + '^' + planID + '^' + siteID;
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val(param);
            $('#hd-ImportUrl').val('/OpeationMns/GOCModule/ImportExcel?param=');
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
            var url = $('#hd-ImportUrl').val() + type;

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
                    $('#hd-ImportType').val('');
                    $('#import_gocPlan').modal('hide');

                    hrms.hide_waitMe($('#import_gocPlan'));
                    hrms.notify("Import success!", 'Success', 'alert', function () {

                        document.getElementById("btnSearch").click();
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

    function initSelectGetPlanIds() {
        $.ajax({
            url: '/OpeationMns/SCPMaster/GetPlanIds',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {

                var render = "";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.code + "'>" + item.name + "</option >"
                });
                $('#cboPlanID').html(render);
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading data', 'error', 'alert', function () { });
            }
        });
    }

    function initSelectGetSites() {
        $.ajax({
            url: '/OpeationMns/SCPMaster/GetSites',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {

                var render = "<option value=' '> - </option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.code + "'>" + item.name + "</option >"
                });
                $('#cboSiteId').html(render);
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading data', 'error', 'alert', function () { });
            }
        });
    }

    this.beginSearch = function () {
        hrms.run_waitMe($('#salesContent'));
    }

    this.doAftersearch = function () {
        InitDataTable();
        hrms.hide_waitMe($('#salesContent'));
    }

    function InitDataTable() {
        $('#gocSalesPlanDataTable').DataTable({
            scrollY: 600,
            scrollX: true,
            scrollCollapse: true,
            paging: false,
            select: true,
            "searching": true,
            /*     "order": [3, 'asc'],*/
            fixedColumns: {
                left: 3
            },
            fixedHeader: true
        });

        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="gocSalesPlanDataTable_length"]').removeClass('form-control-sm');
    }
}