var vocController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {

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

            $.ajax({
                url: '/Admin/Home/ImportExcel?param=' + type,
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
                    hrms.notify('error: Import error!', 'error', 'alert', function () { });
                }
            });
            return false;
        });
    }

    this.beginSearch = function () {
        hrms.run_waitMe($('#vocMstDataTable'));
    }

    this.doAftersearch = function () {
        InitDataTable();
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
                fixedColumns: {
                    left: 5
                },
                select: true,
                initComplete: function () {
                    var api = this.api();

                    // For each column
                    api
                        .columns()
                        .eq(0)
                        .each(function (colIdx) {
                            // Set the header cell to contain the input element
                            var cell = $('.filters th').eq(
                                $(api.column(colIdx).header()).index()
                            );
                            var title = $(cell).text();
                            $(cell).html('<input type="text" placeholder="' + title + '" />');

                            // On every keypress in this input
                            $(
                                'input',
                                $('.filters th').eq($(api.column(colIdx).header()).index())
                            )
                                .off('keyup change')
                                .on('keyup change', function (e) {
                                    e.stopPropagation();

                                    // Get the search value
                                    $(this).attr('title', $(this).val());
                                    var regexr = '({search})'; //$(this).parents('th').find('select').val();

                                    var cursorPosition = this.selectionStart;
                                    // Search the column for that value
                                    api
                                        .column(colIdx)
                                        .search(
                                            this.value != ''
                                                ? regexr.replace('{search}', '(((' + this.value + ')))')
                                                : '',
                                            this.value != '',
                                            this.value == ''
                                        )
                                        .draw();

                                    $(this)
                                        .focus()[0]
                                        .setSelectionRange(cursorPosition, cursorPosition);
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