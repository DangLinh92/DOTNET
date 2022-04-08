var ppmController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('#btnPmmByYear').on('click', function (e) {
            e.preventDefault();
            resetFormByYear();
            $('#hdTypeAddEdit').val('Add');
            $('#addEdiPPMByYear').modal('show');
        });

        $('#btnSavePPMByYear').on('click', function (e) {
            e.preventDefault();

            var action = $('#hdTypeAddEdit').val();

        });

        function resetFormByYear() {
            $('#txtYear').val('');
            $('#txtActualValue').val('0');
            $('#txtTargetPPM').val('0');
            $('#hdTypeAddEdit').val('');
            $('#hdId').val('0');
        }
    }
}