var ProfileMng = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });

        $("#fileInputImage").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var idNhanVien = $('#txtMaNhanVien').data('id');
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            data.append('category', 'avatar');
            data.append('Id', idNhanVien);

            // upload to server
            $.ajax({
                type: "POST",
                url: "/admin/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    // save image 
                    $.ajax({
                        type: "POST",
                        url: "/admin/nhanvien/SaveAvatar",
                        dataType: 'json',
                        data: {
                            Id: idNhanVien,
                            url: path
                        },
                        success: function (status) {
                            hrms.notify("Upload success!", 'Success', 'alert', function () {
                                $("#fileInputImage").val(null);
                                $('#imgAvatar').attr('src', path);
                            });
                        },
                        error: function (status) {
                            hrms.notify('There was error save image!' + status.responseText, 'error', 'alert', function () { });
                        }
                    });
                },
                error: function () {
                    hrms.notify('There was error uploading image!', 'error', 'alert', function () { });
                }
            });
        });
    }

}