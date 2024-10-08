var loginLuongController = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {

        $('#frmLogin').validate({
            errorClass: 'error',
            ignore: [],
            lang: 'en',
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

        $('#btnSave').on('click', function (e) {

            if ($('#frmLogin').valid()) {
                e.preventDefault();

                var pass = $('#txtNewPass').val();
                var pass1 = $('#txtNewPass1').val();

                if (pass === pass1) {
                    $.ajax({
                        type: 'POST',
                        data: {
                            newpassword: pass
                        },
                        dataType: 'json',
                        url: '/luong/login/ChangePassword',
                        success: function (res) {
                            if (res.Success) {
                                window.location.href = "/luong/home/index";
                            }
                            else {
                                hrms.notify(res.Message, 'error', 'alert', function () { });
                            }
                        }
                    })
                }
                else {
                    hrms.notify('Mật khẩu không khớp nhau', 'error', 'alert', function () { });
                }
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
            url: '/luong/login/authen',
            success: function (res) {

                console.log(res);
                if (res.Success) {

                    if (res.Message == "0") {
                        window.location.href = "/luong/login/ChangePassword";
                    }
                    else {
                        window.location.href = "/luong/home/index";
                    }
                }
                else {
                    hrms.notify(res.Message, 'error', 'alert', function () { });
                }
            }
        })
    }
}