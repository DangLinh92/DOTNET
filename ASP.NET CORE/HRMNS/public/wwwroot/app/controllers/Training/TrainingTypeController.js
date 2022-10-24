var TrainingTypeController = function () {

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {

        $('#btnAddType').on('click', function () {
            resetFormData();
            $('#hd_addType').val('Add');
            $('#add_type').modal('show');
        });
        
        $('body').on('click', '.m_Edit_trainingType', function (e) {
            e.preventDefault();

            resetFormData();
            $('#hd_addType').val('Edit');
            $('#add_type').modal('show');

            var that = $(this).data('id');

            if (!that)
                return;

            $.ajax({
                type: "GET",
                url: "/Admin/TrainingType/GetTypeById",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (ts) {
                    if (ts) {
                        $('#txtIdTrainingType').val(that);
                        $('#txtTrainName').val(ts.TrainName);
                        $('#txtDescription').val(ts.Description);
                        $('#txtStatusType').val(ts.Status);
                        $('#txtStatusType').trigger('change');
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

        $('body').on('click', '.m_Delete_trainingType', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#txtIdTrainingTypeDelete').val(that);
            $('#delete_type').modal('show');
        });

        $('#btnDeleteTrainingType').on('click', function (e) {
            e.preventDefault();
            let _id = $('#txtIdTrainingTypeDelete').val();
            $.ajax({
                url: '/Admin/TrainingType/DeleteType',
                type: 'POST',
                dataType: 'json',
                data:
                {
                    Id: _id
                },
                success: function (response) {
                    $('#delete_type').modal('hide');
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
        $('#btnSaveTrainingType').on('click', function (e) {

            if ($('#frmTrainingType').valid()) {

                e.preventDefault();

                let action = $('#hd_addType').val();
                let _id = $('#txtIdTrainingType').val();
                let _TrainnigName = $('#txtTrainName').val();
                let _Description = $('#txtDescription').val();
                let _txtStatus = $('#txtStatusType').val();

                $.ajax({
                    url: '/Admin/TrainingType/UpTrainingType?action=' + action,
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        Id: _id,
                        TrainName: _TrainnigName,
                        Description: _Description,
                        Status: _txtStatus
                    },
                    success: function (response) {
                        $('#add_type').modal('hide');
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

        function resetFormData()
        {
            $('#hd_addType').val('');
            $('#txtIdTrainingType').val('');
            $('#txtTrainName').val('');
            $('#txtDescription').val('');
            $('#txtStatusType').val('');
            $('#txtStatusType').trigger('change');
        }
    }
}