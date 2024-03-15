var YieldController = function () {

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {

        $('#btnShowAddNewTraining').on('click', function () {
            resetFormData();
            $('#hd_training').val('Add');
            initSelectModel();
            $('#add_training').modal('show');
        });

        $('body').on('click', '.m_Edit_yield', function (e) {
            e.preventDefault();

            resetFormData();
            initSelectModel();

            $('#hd_training').val('Edit');
            $('#add_training').modal('show');

            var that = $(this).data('id');

            if (!that)
                return;

            $.ajax({
                type: "GET",
                url: "/opeationmns/yieldofmodel/GetYieldById",
                dataType: "json",
                data: {
                    Id: that
                },
                success: function (ts) {
                    if (ts) {
                        $('#txtIdYield').val(that);
                        $('#_txtModel').val(ts.Model);
                        $('#txtYieldPlan').val(ts.YieldPlan);
                        $('#txtYieldActual').val(ts.YieldActual);
                        $('#txtMonth').val(ts.Month);
                        $('#_txtModel').trigger('change');
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

        $('body').on('click', '.m_Delete_yield', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#hd_training_delete').val(that);
            $('#delete_training').modal('show');
        });

        $('#btnDeleteTraining').on('click', function (e) {
            e.preventDefault();
            let _id = $('#hd_training_delete').val();
            $.ajax({
                url: '/opeationmns/yieldofmodel/DeleteYield',
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
        $('#btnSaveYield').on('click', function (e) {

            if ($('#frmTraining').valid()) {

                e.preventDefault();

                let action = $('#hd_training').val();
                let _id = $('#txtIdYield').val();
                let _model = $('#_txtModel').val();
                let _yieldPlan = $('#txtYieldPlan').val();
                let _yieldActual = $('#txtYieldActual').val();
                let _month = $('#txtMonth').val();

                $.ajax({
                    url: '/opeationmns/yieldofmodel/putyield?action=' + action,
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        Id: _id,
                        Model: _model,
                        YieldPlan: _yieldPlan,
                        YieldActual: _yieldActual,
                        Month: _month
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

        function resetFormData()
        {
            $('#txtIdYield').val('');
            $('#_txtModel').val('');
            $('#txtYieldPlan').val('');
            $('#txtYieldActual').val('');
            $('#txtMonth').val('');
            $('#_txtModel').trigger('change');
        }

        // Init data model
        function initSelectModel()
        {
            $.ajax({
                url: '/OpeationMns/YieldOfModel/GetAllModel',
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (response) {

                    var groupEmp = response.reduce(function (result, current) {
                        result['Model'] = result['Model'] || [];
                        result['Model'].push(current);
                        return result;
                    }, {});

                    var render = "<option value='' selected='selected'>Select option...</option>";
                    $.each(groupEmp, function (gr, item) {
                        render += "<optgroup label='" + gr + "'>";
                        $.each(item, function (j, sub) {
                            render += "<option value='" + sub + "'>" + sub + "</option>"
                        });
                        render += "</optgroup>"
                    });

                    $('#_txtModel').html(render);
                },
                error: function (status) {
                    console.log(status);
                    hrms.notify('Cannot loading model', 'error', 'alert', function () { });
                }
            });
        }

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