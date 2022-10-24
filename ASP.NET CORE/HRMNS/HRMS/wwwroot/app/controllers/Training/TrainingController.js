var trainingController = function () {

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {

        $('#btnShowAddNewTraining').on('click', function () {
            resetFormData();
            $('#hd_training').val('Add');
            initSelectOptionTrainingType();
            $('#add_training').modal('show');
        });
        

        $('body').on('click', '.m_View_training', function (e) {
            e.preventDefault();

            $('#view_nhanvien_training').modal('show');
            var that = $(this).data('id');

            if (!that)
                return;

            $.ajax({
                type: "GET",
                url: "/Admin/TrainingList/GetNhanVienTraining",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (response) {
                    var render = "";
                    $.each(response, function (i, item) {
                        render += "<tr>" +
                                            "<td>" + i + "</td>" +
                                            "<td>" + item.MaNV + "</td>" +
                                            "<td>" + item.HR_NHANVIEN.TenNV + "</td>"
                                  + "</tr>"
                    });

                    if (render == "") {
                        render = "<tr><td>-</td><td>-</td><td>-</td></tr>";
                    }

                    InitDataTableModel();
                    $('#bdyNhanVienTraining').html(render);
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        $('body').on('click', '.m_Edit_training', function (e) {
            e.preventDefault();

            resetFormData();
            initSelectOptionTrainingType();
            $('#hd_training').val('Edit');
            $('#add_training').modal('show');

            var that = $(this).data('id');

            if (!that)
                return;

            $.ajax({
                type: "GET",
                url: "/Admin/TrainingList/GetTrainingById",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (ts) {
                    if (ts) {
                        $('#txtIdTraining').val(that);
                        $('#txtTrainnigType').val(ts.TrainnigType);
                        $('#txtTrainer').val(ts.Trainer);
                        $('#txtFromDate').val(ts.FromDate);
                        $('#txtToDate').val(ts.ToDate);
                        $('#txtDescription').val(ts.Description);
                        $('#txtTrainnigType').trigger('change');
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

        $('body').on('click', '.m_Delete_training', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#hd_training_delete').val(that);
            $('#delete_training').modal('show');
        });

        $('#btnDeleteTraining').on('click', function (e) {
            e.preventDefault();
            let _id = $('#hd_training_delete').val();
            $.ajax({
                url: '/Admin/TrainingList/DeleteTraining',
                type: 'POST',
                dataType: 'json',
                data:
                {
                    Id: _id
                },
                success: function (response) {
                    $('#delete_training').modal('hide');
                    hrms.notify("Delete success!", 'Success', 'alert', function () {
                        window.location.reload();
                    });
                },
                error: function (status) {
                    hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        // Click save popup
        $('#btnSaveTraining').on('click', function (e) {

            if ($('#frmTraining').valid()) {

                e.preventDefault();

                let action = $('#hd_training').val();
                let _id = $('#txtIdTraining').val();
                let _TrainnigType = $('#txtTrainnigType').val();
                let _Trainer = $('#txtTrainer').val();
                let _fromDate = $('#txtFromDate').val();
                let _ToDate = $('#txtToDate').val();
                let _Description = $('#txtDescription').val()

                $.ajax({
                    url: '/Admin/TrainingList/PutTraining?action=' + action,
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        Id: _id,
                        TrainnigType: _TrainnigType,
                        Trainer: _Trainer,
                        FromDate: _fromDate,
                        ToDate: _ToDate,
                        Description: _Description
                    },
                    success: function (response) {
                        $('#add_training').modal('hide');
                        hrms.notify("Update success!", 'Success', 'alert', function () {
                            window.location.reload();
                        });
                    },
                    error: function (status) {
                        hrms.notify('error:' + status.responseText, 'error', 'alert', function () { });
                    }
                });
            }
        });

        function resetFormData() {
            $('#txtIdTraining').val('');
            $('#txtTrainnigType').val('');
            $('#txtTrainer').val('');
            $('#txtCost').val('');
            $('#txtFromDate').val('');
            $('#txtToDate').val('');
            $('#txtDescription').val;
            $('#txtTrainnigType').trigger('change');

        }

        function initSelectOptionTrainingType() {
            $.ajax({
                url: '/Admin/TrainingList/GetTrainingType',
                type: 'POST',
                dataType: 'json',
                async: false,
                success: function (response) {
                    var render = "<option value='' selected='selected'>Select option...</option>";
                    $.each(response, function (i, item) {
                        render += "<option value='" + item.Id + "'>" + item.TrainName + "</option >"
                    });
                    $('#txtTrainnigType').html(render);
                },
                error: function (status) {
                    hrms.notify('Cannot loading', 'error', 'alert', function () { });
                }
            });
        }


        // Import excel
        // 1. import even log
        $('body').on('click', '.m_Import_training', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $("#fileInputExcel").val(null);
            $('#hd-ImportType').val(that);// mã training
            $('#import_Training').modal('show');
        });

        // Close import
        $('#btnCloseImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                $("#fileInputExcel").val(null);
                $('#hd-ImportType').val('');
                $('#import_Training').modal('hide');
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
                $('#import_Training').modal('hide');
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
                url: '/Admin/TrainingList/ImportNhanVienToTraining?param=' + type,
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    $('#import_Training').modal('hide');
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

        function InitDataTableModel() {
            var table = $('#nhanvienTrainingDatatable');
            if (table) {
                table.DataTable().destroy();
            }

            $('#nhanvienTrainingDatatable').DataTable({
                paging: true,
                select: true,
                "searching": true
            });
            $('input[type=search]').addClass('floating').removeClass('form-control-sm').css('width', 300).attr('placeholder', 'Type to search');
            $('select[name="nhanvienTrainingDatatable_length"]').removeClass('form-control-sm');
        }
    }
}