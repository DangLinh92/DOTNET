var loginController = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {

        $('#frmLogin').validate({
            errorClass: 'error',
            ignore: [],
            lang:'en',
            rules: {
                userName: {
                    required: true
                },
                passWord: {
                    required: true
                }
            }
        });

        $('#btnLogin').on('click', function (e) {

            if ($('#frmLogin').valid()) {
                e.preventDefault();

                var user = $('#txtUserName').val();
                var pass = $('#txtPassword').val();

                login(user, pass);
            }

        });
    }

    var login = function (user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                UserName: user,
                Password: pass
            },
            dataType: 'json',
            url: '/admin/login/authen',
            success: function (res) {
                if (res.Success) {
                    window.location.href = "/admin/home/index";
                }
                else {
                    hrms.notify(res.Message, 'error', 'alert', function () { });
                }
            }
        })
    }
}