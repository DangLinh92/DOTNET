var vocOnsiteController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {

        $('#btnAddOnsite').on('click', function (e) {
            e.preventDefault();
            resetFormAdd();
            $('#hdTypeAddEdit').val('Add');
            $('#addEditOnsite').modal('show');
        });

        $('#btnSaveOnsite').on('click', function (e) {
            e.preventDefault();
            var customer = $('#cboCustomer').val();
            var date = $('#txtDate').val();
            var part = $('#cboPart').val();
            var wisolModel = $('#txtWisol_Model').val();
            var customerCode = $('#txtCustomer_Code').val();
            var marking = $('#txtMarking').val();
            var productionDate = $('#txtProductionDate').val();
            var setModel = $('#txtSetModel').val();
            var result = $('#cboResult').val();
            var note = $('#txtNote').val();
            var qty = $('#txtQty').val();
            var type = $('#hdTypeAddEdit').val();
            var id = $('#hdId').val();

            var model = {
                Customer: customer,
                Date: date,
                Part: part,
                Wisol_Model: wisolModel,
                Customer_Code: customerCode,
                Marking: marking,
                ProductionDate: productionDate,
                SetModel: setModel,
                Result: result,
                Note: note,
                Qty: qty
            }

            if (type != 'Add') {
                model.Id = id;
            }

            $.ajax({
                type: 'POST',
                data: model,
                dataType: 'json',
                url: '/admin/onsite/SaveData?action=' + type,
                success: function (res) {
                    $('#addEditOnsite').modal('hide');
                    hrms.notify("Update success!", 'Success', 'alert', function () {
                        window.location.reload();
                    });
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        $('body').on('click', '.edit-vocOnsite', function (e) {
            e.preventDefault();
            resetFormAdd();
            $('#hdTypeAddEdit').val('Edit');

            var that = $(this).data('id');
            $('#hdId').val(that);

            $.ajax({
                type: 'GET',
                data: {
                    id: that
                },
                dataType: 'json',
                url: '/admin/onsite/GetOnsiteById',
                success: function (res) {
                    $('#txtDate').val(res.Date);
                    $('#txtWisol_Model').val(res.Wisol_Model);
                    $('#txtCustomer_Code').val(res.Customer_Code);
                    $('#txtMarking').val(res.Marking);
                    $('#txtProductionDate').val(res.ProductionDate);
                    $('#txtSetModel').val(res.SetModel);
                    $('#cboResult').val(res.Result);
                    $('#cboResult').trigger('change');
                    $('#txtNote').val(res.Note);
                    $('#txtQty').val(res.Qty);
                    $('#addEditOnsite').modal('show');
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

        $('body').on('click', '.delete-vocOnsite', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#txtId').val(that);

            $('#delete_onsite').modal('show');
        });

        $('#btnDelete').on('click', function (e) {
            e.preventDefault();
            var code = $('#txtId').val();
            $.ajax({
                type: 'POST',
                data: {
                    id: code
                },
                dataType: 'json',
                url: '/admin/onsite/DeleteOnsite',
                success: function (res) {
                    $('#delete_onsite').modal('hide');
                    hrms.notify("Delete success!", 'Success', 'alert', function () {
                        window.location.reload();
                    });
                },
                error: function (status) {
                    hrms.notify('error: ' + status.responseText, 'error', 'alert', function () { });
                }
            });
        });

    }

    function resetFormAdd() {
        $('#hdTypeAddEdit').val('');
        $('#hdId').val('');
        $('#txtDate').val('');
        $('#txtWisol_Model').val('');
        $('#txtCustomer_Code').val('');
        $('#txtMarking').val('');
        $('#txtProductionDate').val('');
        $('#txtSetModel').val('');
        $('#cboResult').val('');
        $('#cboResult').trigger('change');
        $('#txtNote').val('');
        $('#txtQty').val(1);
    }
}