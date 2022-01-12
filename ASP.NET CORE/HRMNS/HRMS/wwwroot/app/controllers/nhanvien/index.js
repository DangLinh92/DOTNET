var nhanVienController = function () {
    this.initialize = function () {
        // loadData();
    }

    function registerEvents() {
        // bindding event
    }

    function loadData() {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: '/Admin/NhanVien/GetAll',
            dataType: 'json',
            success: function (response) {
                $.each(response.Data, function (i, item) {
                    render += Mustache.render(template, {
                        Avatar: item.Image == null ? "/img/profiles/avatar-01.jpg" : item.Image,
                        MaNV: item.Id,
                        TenNV: item.TenNV,
                        ChucDanh: hrms.nullString(item.HR_CHUCDANH?.TenChucDanh),
                        BoPhan: item.MaBoPhan,
                        NgaySinh: item.NgaySinh,
                        Email: item.Email,
                        Phone: item.SoDienThoai,
                        NgayVaoCty: item.NgayVao,
                        Status: item.Status,
                        IconStatus: item.Status == "Active" ? '<i class="fa fa-dot-circle-o text-success"></i>' : '<i class="fa fa-dot-circle-o text-danger"></i>'
                    });

                    if (render != '') {
                        $('#tbl-content').html(render);
                    }
                });
            },
            error: function (status) {
                console.log(status);
                hrms.notify('Cannot loading data', 'error', 'alert', function () { });
            }
        })
    }
}