var ppmController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        // 1. import k1
        $('#btn-importK1Month').on('click', function () {
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val('month');
            $('#import_Voc').modal('show');
        });

        $('#btn-importK1Year').on('click', function () {
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val('year');
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

            if (type == 'month') {
                url = '/Admin/k1/ImportExcel?param=K1_Month';
            }
            else if (type == 'year') {
                url = '/Admin/k1/ImportExcel?param=K1_Year';
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

        // ----------------------- PPM by month -----------------------------
        // show add model
        $('#btnPmmByYear').on('click', function (e) {
            e.preventDefault();
            resetFormByYear();
            $('#hdTypeAddEdit').val('Add');
            $('#addEdiPPMByYear').modal('show');


        });

        $('body').on('click', '.edit-ppmYear', function (e) {
            e.preventDefault();
            resetFormByYear();
            var that = $(this).data('id');

            $('#hdId').val(that);
            $('#hdTypeAddEdit').val('Edit');
            $('#addEdiPPMByYear').modal('show');

            $.ajax({
                type: "GET",
                url: "/admin/k1/GetPPMByYear",
                dataType: "json",
                data:
                {
                    id: that
                },
                success: function (ppm) {
                    if (ppm) {
                        $('#txtYear').val(ppm.Year);
                        $('#txtActualValue').val(ppm.ValuePPM);
                        $('#txtTargetPPM').val(ppm.TargetPPM);
                        $('#cboModule').val(ppm.Module);
                        $('#cboModule').trigger('change');
                    }
                    else {
                        hrms.notify('error: Not found ppm!', 'error', 'alert', function () { });
                    }
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });

        });

        // ------------ show delete pop up
        $('body').on('click', '.delete-ppmYear', function (e) {
            e.preventDefault();
            $('#txtIdDelete').val($(this).data('id'));
            $('#txtTypeDelete').val('year');
            $('#delete_ppm').modal('show');
        });

        $('body').on('click', '.delete-ppmYearMonth', function (e) {
            e.preventDefault();
            $('#txtIdDelete').val($(this).data('id'));
            $('#txtTypeDelete').val('year-month');
            $('#delete_ppm').modal('show');
        });

        $('#delete_ppm').on('click', function (e) {
            e.preventDefault();

            var id = $('#txtIdDelete').val();
            var type = $('#txtTypeDelete').val();
            var url = '';

            if (type == 'year') {
                url = '/admin/k1/DeletePPMByYear';
            }
            else {
                url = '/admin/k1/DeletePPMByYearMonth';
            }

            $.ajax({
                type: "POST",
                url: url,
                dataType: "json",
                data:
                {
                    Id: id
                },
                success: function (ppm) {

                    $('#delete_ppm').modal('hide');
                    hrms.notify("Update success!", 'Success', 'alert', function () {
                        window.location.reload();
                    });
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });
        //-------------

        // update ppm by year
        $('#btnSavePPMByYear').on('click', function (e) {

            if ($('#frmAddEditPPMByYear').valid()) {
                e.preventDefault();
                var action = $('#hdTypeAddEdit').val();
                var module = $('#cboModule').val();
                var year = $('#txtYear').val();
                var actualVal = $('#txtActualValue').val();
                var target = $('#txtTargetPPM').val();
                var id = $('#hdId').val();

                var data = {
                    Year: year,
                    Module: module,
                    ValuePPM: actualVal,
                    TargetPPM: target
                };

                if (action != 'Add') {
                    data.Id = id;
                }

                $.ajax({
                    type: "POST",
                    url: "/admin/k1/UpdatePPMByYear?action=" + action,
                    dataType: "json",
                    data: data,
                    success: function (response) {
                        $('#addEdiPPMByYear').modal('hide');

                        hrms.notify("Update success!", 'Success', 'alert', function () {
                            window.location.reload();
                        });
                    },
                    error: function (status) {
                        hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                    }
                });
            }
        });

        function resetFormByYear() {
            $('#txtYear').val('');
            $('#txtActualValue').val('0');
            $('#txtTargetPPM').val('0');
            $('#hdTypeAddEdit').val('');
            $('#hdId').val('0');
        }

        // --------------------- PPM BY YEAR MONTH -------------------------
        // show add model
        $('#btnGmes').on('click', function (e) {
            e.preventDefault();
            resetFormByYearMonth();
            $('#hdTypeAddEdit_GmesData').val('Add');
            $('#addEdiGmesDataByMonth').modal('show');

            $.ajax({
                type: "GET",
                url: "/admin/k1/GetTargetByYear",
                dataType: "json",
                success: function (target) {
                    $('#txtTargetppm_gmes').val(target);
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        $('body').on('click', '.edit-ppmYearMonth', function (e) {
            e.preventDefault();
            resetFormByYearMonth();

            var that = $(this).data('id');

            $('#hdId_GmesData').val(that);
            $('#hdTypeAddEdit_GmesData').val('Edit');
            $('#addEdiGmesDataByMonth').modal('show');

            $.ajax({
                type: "GET",
                url: "/admin/k1/GetPPMByYearMonth",
                dataType: "json",
                data:
                {
                    id: that
                },
                success: function (ppm) {
                    if (ppm) {
                        $('#cboCustomer_gmes').val(ppm.Customer);
                        $('#cboCustomer_gmes').trigger('change');

                        $('#txtYear_gmes').val(ppm.Year + '-' + ppm.Month);
                        $('#txtActualValue_gmes').val(ppm.Value);
                        $('#txtTargetppm_gmes').val(ppm.TargetValue);

                        $('#cboModule_gmes').val(ppm.Module);
                        $('#cboModule_gmes').trigger('change');

                        $('#cboType_gmes').val(ppm.Type);
                        $('#cboType_gmes').trigger('change');
                    }
                    else {
                        hrms.notify('error: Not found ppm!', 'error', 'alert', function () { });
                    }
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // update ppm by year
        $('#btnSaveGmesByMonth').on('click', function (e) {
            if ($('#frmAddEditPPMByYearMonth').valid()) {

                e.preventDefault();
                var action = $('#hdTypeAddEdit_GmesData').val();
                var module = $('#cboModule_gmes').val();
                var custome = $('#cboCustomer_gmes').val();
                var type = $('#cboType_gmes').val();
                var yearMonth = $('#txtYear_gmes').val();
                var year = '';
                var month = '';
                if (yearMonth.includes('-')) {
                    year = yearMonth.split('-')[0];
                    month = yearMonth.split('-')[1];
                }

                var actualVal = $('#txtActualValue_gmes').val();
                var target = $('#txtTargetppm_gmes').val();
                var id = $('#hdId_GmesData').val();

                var data = {
                    Month: month,
                    Year: year,
                    Module: module,
                    Value: actualVal,
                    TargetValue: target,
                    Customer: custome,
                    Type: type
                };

                if (action != 'Add') {
                    data.Id = id;
                }

                $.ajax({
                    type: "POST",
                    url: "/admin/k1/UpdatePPM?action=" + action,
                    dataType: "json",
                    data: data,
                    success: function (response) {
                        $('#addEdiGmesDataByMonth').modal('hide');

                        hrms.notify("Update success!", 'Success', 'alert', function () {
                            window.location.reload();
                        });
                    },
                    error: function (status) {
                        hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                    }
                });
            }
        });

        function resetFormByYearMonth() {
            $('#cboCustomer_gmes').val();
            $('#txtYear_gmes').val('');
            $('#txtActualValue_gmes').val('0');
            $('#txtTargetppm_gmes').val('0');
            $('#hdTypeAddEdit_GmesData').val('');
            $('#hdId_GmesData').val('0');
        }
    }
}