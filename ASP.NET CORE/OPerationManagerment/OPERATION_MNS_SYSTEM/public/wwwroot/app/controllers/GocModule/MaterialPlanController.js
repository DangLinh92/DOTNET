var MaterialPlanController = function () {
    this.initialize = function () {
        registerEvents();
        initSelectGetPlanIds();
        initSelectGetSites();
    }

    function registerEvents() {
        // IMPORT EXCEL START
        $('#btn-import-material').on('click', function () {
            let masterID = $('#cboMasterID').val();
            let planID = $('#cboPlanID').val();
            let siteID = $('#cboSiteId').val();
            let param = masterID + '^' + planID + '^' + siteID;
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val(param);
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

            $.ajax({
                url: '/OpeationMns/GOCModule/ImportMaterialExcel?param=' + type,
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

        $('#btnLoadDataSCP').on('click', function () {

            let masterId = $('#cboMasterID').val();
            let planId = $('#cboPlanID').val();
            let siteId = $('#cboSiteId').val();

            if (masterId.length < 3 || planId.length < 3 || siteId.length < 3) {
                hrms.notify('error: Thông tin masterId,plan Id,Site Id không phù hợp', 'error', 'alert', function () { });
            }
            else {
                $.ajax({
                    url: '/OpeationMns/GOCModule/GetSCPData',
                    type: 'POST',
                    data: {
                        masterID: masterId,
                        planID: planId,
                        siteID: siteId
                    },
                    beforeSend: function () {
                        hrms.run_waitMe($('#productionContent'));
                    },
                    success: function (data) {
                        hrms.hide_waitMe($('#productionContent'));

                        if (data) {
                            hrms.notify("Load data success!", 'Success', 'alert', function () {
                            });
                        }
                        else {
                            hrms.notify("Load data error!", 'error', 'alert', function () {
                            });
                        }
                    },
                    error: function (status) {
                        hrms.notify('error: Load error!' + status.responseText, 'error', 'alert', function () { });
                        hrms.hide_waitMe($('#productionContent'));
                    }
                });
            }
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
        hrms.run_waitMe($('#productionContent'));
    }

    this.doAftersearch = function () {
        InitDataTable();
        hrms.hide_waitMe($('#productionContent'));
    }

    function InitDataTable() {
        $('#gocMaterialPlanDataTable').DataTable({
            scrollY: 600,
            scrollX: true,
            scrollCollapse: true,
            paging: false,
            select: true,
            "searching": true,
            /*     "order": [3, 'asc'],*/
            fixedColumns: {
                left: 4
            },
            fixedHeader: true,
            dom: 'Bfrtip',
            buttons: [
                'excel'
            ]
        });

        $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
        $('select[name="gocMaterialPlanDataTable_length"]').removeClass('form-control-sm');
    }
}