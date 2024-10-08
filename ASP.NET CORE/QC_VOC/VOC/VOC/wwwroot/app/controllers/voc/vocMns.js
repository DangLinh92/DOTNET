var vocController = function () {
    this.initialize = function () {
        InitYear();
        registerEvents();
    }

    function registerEvents() {

        // update defect type
        $('#btnAddDefectType').on('click', function (e) {
            e.preventDefault();
            $('#addDefectType').modal('show');
        });

        $('#btnSaveDefect').on('click', function (e) {
            e.preventDefault();

            var nameEng = $('#txtEngName').val();
            var nameKr = $('#txtKoreaName').val();

            $.ajax({
                url: "/Admin/Voc/AddDefectType",
                type: 'POST',
                data:
                {
                    nameEng: nameEng,
                    nameKr: nameKr
                },
                success: function (data) {
                    $('#addDefectType').modal('hide');
                    hrms.notify("Update date success!", 'Success', 'alert', function () { });
                },
                error: function (status) {
                    hrms.notify(status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // update day
        $('#btn-UpdateLastDay').on('click', function (e) {
            e.preventDefault();
            $('#update-LastDay-Model').modal('show');
        });

        $('#btnUpdateDay').on('click', function (e) {

            e.preventDefault();
            var dateUpdate = $('#txtUpdateDate').val();

            if (dateUpdate != '') {
                $.ajax({
                    url: "/Admin/Voc/UpdateDay",
                    type: 'POST',
                    data: {
                        date: dateUpdate
                    },
                    success: function (data) {

                        $('#update-LastDay-Model').modal('hide');
                        hrms.notify("Update date success!", 'Success', 'alert', function () {
                        });
                    },
                    error: function (status) {
                        hrms.notify(status.responseText, 'error', 'alert', function () { });
                    }
                });
            }
            else {
                alert('Last Updat is required!');
            }
        });
        // 

        // preview file
        $('body').on('click', '.view-issue', function (e) {
            e.preventDefault();
            var url = $(this).data('id');
            var urlData = url.substring(0, url.lastIndexOf('.')) + '.pdf#toolbar=0';
            var obj = '<object id="previewFile-obj" data="' + urlData + '" style="width:100%;height:100%"></object>'
            $('#previewFile-result').html(obj);
            $('#previewFile-result').on("contextmenu", function (e) {
                e.preventDefault();
                return false;
            });
            $('#previewFile_Voc').modal('show');
        });

        $('body').on('click', '.view-issue1', function (e) {
            e.preventDefault();
            var url = $(this).data('id');
            var urlData = url.substring(0, url.lastIndexOf('.')) + '.pdf#toolbar=0';
            var obj = '<object id="previewFile-obj" data="' + urlData + '" style="width:100%;height:100%"></object>'
            $('#previewFile-result').html(obj);
            $('#previewFile-result').on("contextmenu", function (e) {
                e.preventDefault();
                return false;
            });
            $('#previewFile_Voc').modal('show');
        });

        $('body').on('click', '.view-issue2', function (e) {
            e.preventDefault();
            var url = $(this).data('id');
            var urlData = url.substring(0, url.lastIndexOf('.')) + '.pdf#toolbar=0';
            var obj = '<object id="previewFile-obj" data="' + urlData + '" style="width:100%;height:100%"></object>'
            $('#previewFile-result').html(obj);
            $('#previewFile-result').on("contextmenu", function (e) {
                e.preventDefault();
                return false;
            });
            $('#previewFile_Voc').modal('show');
        });

        // upload file for issue
        $('body').on('click', '.upload-issue', function (e) {
            e.preventDefault();
            $("#fileInputExcel").val(null);
            var vocId = $(this).data('id');
            $('#hd-ImportType').val(vocId);
            $('#import_Voc').modal('show');
        });

        $('body').on('click', '.upload-issue1', function (e) {
            e.preventDefault();
            $("#fileInputExcel").val(null);
            var vocId = $(this).data('id');
            $('#hd-ImportType').val(vocId+'-2');
            $('#import_Voc').modal('show');
        });

        // Import excel
        // 1. import voc
        $('#btn-importVOC').on('click', function () {
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val('');
            $('#import_Voc').modal('show');
        });

        // Close import
        $('#btnCloseImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                $("#fileInputExcel").val(null);
                $('#hd-ImportType').val('');
                $('#import_Voc').modal('hide');
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
                $('#import_Voc').modal('hide');
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
            var type = $('#hd-ImportType').val();

            var url = '';

            if (type == '') {
                url = '/Admin/Voc/ImportExcel?param=' + type;
            }
            else {
                url = '/Admin/Voc/UpLoadExcel?vocId=' + type; // for upload excel
            }

            $.ajax({
                url: url,
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    $('#import_Voc').modal('hide');
                    hrms.notify("Import success!", 'Success', 'alert', function () {
                        location.reload();
                    });
                },
                error: function (status) {
                    hrms.notify(status.responseText, 'error', 'alert', function () { });
                }
            });
            return false;
        });

        // export excel
        $('#btn-exportVOC').on('click', function () {

            var year = $('#txtTimeTo').val();

            if (!year) {
                year = new Date().getFullYear();
            }

            $.ajax({
                type: "POST",
                url: "/Admin/Voc/ExportExcel",
                data: {
                    year: year
                },
                success: function (response) {
                    window.location.href = response;
                },
                error: function (status) {
                    hrms.notify(status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // search
        $('#btnSearch').on('click', function () {
            var year = $('#txtTimeTo').val();

            sessionStorage.setItem("yearSearch", year);
            var url = "/admin/voc/uploadlist?year=" + year;
            window.location = url;

        });

        // add voc
        $('#btnAddVoc').on('click', function (e) {
            e.preventDefault();
            resetForm();
            $('#hdTypeAddEdit').val('Add');
            initSelectOptionDefectType();
            $('#addEdi_Voc').modal('show');
        });

        // Edit voc
        $('body').on('click', '.edit-voc', function (e) {
            e.preventDefault();

            resetForm();
            initSelectOptionDefectType();

            $('#hdTypeAddEdit').val('Edit');
            $('#addEdi_Voc').modal('show');

            var that = $(this).data('id');

            $.ajax({
                type: "GET",
                url: "/Admin/Voc/GetById",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (voc) {

                    if (voc) {
                        $('#hdId').val(that);
                        $('#cboReceived_site').val(voc.Received_site);
                        $('#cboReceived_site').trigger('change');
                        $('#cboPlaceOfOrigin').val(voc.PlaceOfOrigin);
                        $('#cboPlaceOfOrigin').trigger('change');
                        $('#cboReceivedDept').val(voc.ReceivedDept);
                        $('#cboReceivedDept').trigger('change');
                        $('#txtReceivedDate').val(voc.ReceivedDate);
                        $('#txtSPLReceiveddate').val(voc.SPLReceivedDate);
                        $('#txtCustomer').val(voc.Customer);
                        $('#txtSetModel').val(voc.SETModelCustomer);
                        $('#txtProcessCustomer').val(voc.ProcessCustomer);
                        $('#txtModelFullName').val(voc.ModelFullname);
                        $('#txtDefectName').val(voc.DefectNameCus);
                        $('#txtDefectRate').val(voc.DefectRate);
                        $('#txtPBA').val(voc.PBA_FAE_Result);
                        $('#cboPassClass1').val(voc.PartsClassification);
                        $('#cboPassClass1').trigger('change');
                        $('#txtClassification2').val(voc.PartsClassification2);
                        $('#cboVocCount').val(voc.VOCCount);
                        $('#cboVocCount').trigger('change');
                        $('#cboDefactType').val(voc.AnalysisResult);
                        $('#cboDefactType').trigger('change');
                        $('#txtMarking').val(voc.ProdutionDateMarking);
                        $('#txtDefectCause').val(voc.DefectCause);
                        $('#txtDefectClassification').val(voc.DefectClassification);
                        $('#txtCustomerResponse').val(voc.CustomerResponse);
                        $('#txtFinalApprove').val(voc.Report_FinalApprover);
                        $('#txtReportSender').val(voc.Report_Sender);
                        $('#txtReportSentDate').val(voc.Rport_sentDate);
                        $('#cboVocState').val(voc.VOCState);
                        $('#cboVocState').trigger('change');

                        $('#txtVocFinishDate').val(voc.VOCFinishingDate);
                        $('#txtCustomerGroup').val(voc.CustomerGroup);
                        $('#txtProdutionDate').val(voc.ProdutionDate);
                        $('#txtProdutionDate2').val(voc.ProdutionDate_2);
                        $('#txtReceivedDate2').val(voc.ReceivedDate_2);
                        $('#txtSPLReceiveddate2').val(voc.SPLReceivedDate_2);
                    }
                    else {
                        hrms.notify('error: Not found voc!', 'error', 'alert', function () { });
                    }
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // save data
        $('#btnSaveVoc').on('click', function (e) {
            if ($('#frmAddEditVoc').valid()) {

                e.preventDefault();

                var receivedSite = $('#cboReceived_site').val();
                var placeOfOrigin = $('#cboPlaceOfOrigin').val();
                var receivedDept = $('#cboReceivedDept').val();
                var receivedDate = $('#txtReceivedDate').val();
                var sPLReceiveddate = $('#txtSPLReceiveddate').val();
                var customer = $('#txtCustomer').val();
                var setModel = $('#txtSetModel').val();
                var processCustomer = $('#txtProcessCustomer').val();
                var modelFullName = $('#txtModelFullName').val();
                var defectName = $('#txtDefectName').val();
                var defectRate = $('#txtDefectRate').val();
                var pbA = $('#txtPBA').val();
                var passClass1 = $('#cboPassClass1').val();
                var passClass2 = $('#txtClassification2').val();
                var vocCount = $('#cboVocCount').val();
                var defectType = $('#cboDefactType').val();
                var marking = $('#txtMarking').val();
                var defectCause = $('#txtDefectCause').val();
                var defectClassification = $('#txtDefectClassification').val();
                var customerResponse = $('#txtCustomerResponse').val();
                var finalApprove = $('#txtFinalApprove').val();
                var reportSender = $('#txtReportSender').val();
                var reportSentDate = $('#txtReportSentDate').val();
                var vocState = $('#cboVocState').val();
                var vocFinishDate = $('#txtVocFinishDate').val();
                var code = $('#hdId').val();

                var CustomerGroup = $('#txtCustomerGroup').val();
                var ProdutionDate = $('#txtProdutionDate').val();
                var ProdutionDate_2 = $('#txtProdutionDate2').val();
                var ReceivedDate_2 = $('#txtReceivedDate2').val();
                var SPLReceivedDate_2 = $('#txtSPLReceiveddate2').val();

                if (!code) {
                    code = 0;
                }

                var addEdit = $('#hdTypeAddEdit').val();

                $.ajax({
                    url: '/Admin/Voc/SaveVoc?Id=' + code + '&action=' + addEdit,
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        Received_site: receivedSite,
                        PlaceOfOrigin: placeOfOrigin,
                        ReceivedDept: receivedDept,
                        ReceivedDate: receivedDate,
                        SPLReceivedDate: sPLReceiveddate,
                        SPLReceivedDateWeek: '',
                        Customer: customer,
                        SETModelCustomer: setModel,
                        ProcessCustomer: processCustomer,
                        ModelFullname: modelFullName,
                        DefectNameCus: defectName,
                        PBA_FAE_Result: pbA,
                        DefectRate: defectRate,
                        PartsClassification: passClass1,
                        PartsClassification2: passClass2,
                        ProdutionDateMarking: marking,
                        AnalysisResult: defectType,
                        VOCCount: vocCount,
                        DefectCause: defectCause,
                        DefectClassification: defectClassification,
                        CustomerResponse: customerResponse,
                        Report_FinalApprover: finalApprove,
                        Report_Sender: reportSender,
                        Rport_sentDate: reportSentDate,
                        VOCState: vocState,
                        VOCFinishingDate: vocFinishDate,
                        CustomerGroup: CustomerGroup,
                        ProdutionDate: ProdutionDate,
                        ProdutionDate_2: ProdutionDate_2,
                        ReceivedDate_2: ReceivedDate_2,
                        SPLReceivedDate_2: SPLReceivedDate_2
                    },
                    success: function (response) {
                        $('#addEdi_Voc').modal('hide');
                        hrms.notify("Update success!", 'Success', 'alert', function () {
                            window.location.reload();
                        });
                    },
                    error: function (status) {
                        console.log(status.responsetext);
                        hrms.notify('error:' + status.responsetext, 'error', 'alert', function () { });
                    }
                });
            }
        });

        $('body').on('click', '.delete-voc', function (e) {
            e.preventDefault();
            $('#delete_vocModel').modal('show');
            $('#txtId').val($(this).data('id'));
        });

        $('#btnDelete').on('click', function (e) {

            e.preventDefault();
            var that = $('#txtId').val();
            $.ajax({
                type: "POST",
                url: "/Admin/Voc/Delete",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (response) {
                    $('#delete_vocModel').modal('hide');

                    hrms.notify("Delete success!", 'Success', 'alert', function () {
                        window.location.reload();
                    });
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // reset form
        function resetForm() {
            $('#txtReceivedDate').val('');
            $('#txtSPLReceiveddate').val('');
            $('#txtCustomer').val('');
            $('#txtSetModel').val('');
            $('#txtProcessCustomer').val('');
            $('#txtModelFullName').val('');
            $('#txtDefectName').val('');
            $('#txtPBA').val('');
            $('#txtClassification2').val('');
            $('#cboDefactName').val('');
            $('#cboDefactName').trigger('change');
            $('#txtMarking').val('');
            $('#txtDefectCause').val('');
            $('#txtDefectClassification').val('');
            $('#txtCustomerResponse').val('');
            $('#txtFinalApprove').val('');
            $('#txtReportSentDate').val('');
            $('#txtVocFinishDate').val('');
            $('#txtDefectRate').val('');
            $('#txtReportSender').val('');

            $('#txtCustomerGroup').val('');
            $('#txtProdutionDate').val('');
            $('#txtProdutionDate2').val('');
            $('#txtReceivedDate2').val('');
            $('#txtSPLReceiveddate2').val('');
        }

        function initSelectOptionDefectType() {
            $.ajax({
                url: '/admin/voc/GetDefectType',
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (response) {
                    var render = "<option value=''>--Select defect name--</option>";
                    $.each(response, function (i, item) {
                        render += "<option value='" + item.EngsNotation + "'>" + item.EngsNotation + " - " + item.KoreanNotation + "</option >"
                    });
                    $('#cboDefactType').html(render);
                },
                error: function (status) {
                    hrms.notify('Cannot loading defect data', 'error', 'alert', function () { });
                }
            });
        }
    }

    function InitYear() {
        $('#txtTimeTo').val(sessionStorage.getItem("yearSearch"));
    }

    this.beginSearch = function () {
        hrms.run_waitMe($('#vocMstDataTable'));
    }

    this.doAftersearch = function () {
        // InitDataTable();
        hrms.hide_waitMe($('#vocMstDataTable'));
    }

    function InitDataTable() {
        let myVar = setInterval(function () {

            var table = $('#vocMstDataTable');
            if (table) {
                table.DataTable().destroy();
            }

            $('#vocMstDataTable').DataTable({
                scrollY: 400,
                scrollX: true,
                scrollCollapse: true,
                paging: false,
                select: true,
                fixedColumns: {
                    leftColumns: 5,
                    rightColumns: 1
                }
                , initComplete: function () {
                    this.api().columns([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25]).every(function () {
                        var column = this;
                        var select = $('<select><option value=""></option></select>')
                            .appendTo($(column.header()))
                            .on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );

                                column
                                    .search(val ? '^' + val + '$' : '', true, false)
                                    .draw();
                            });

                        column.data().unique().sort().each(function (d, j) {
                            select.append('<option value="' + d + '">' + d + '</option>')
                        });
                    });
                }
                , columnDefs: [{
                    render: function (data, type, full, meta) {
                        return "<div class='text-wrap width-100'>" + data + "</div>";
                    },
                    targets: '_all'
                }],
                "order": [[4, 'asc']]
            });

            $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
            $('select[name="vocMstDataTable_length"]').removeClass('form-control-sm');
            clearInterval(myVar);
        }, 500);
    }
}