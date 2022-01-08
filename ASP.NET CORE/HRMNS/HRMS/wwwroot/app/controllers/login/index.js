var loginController = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {
        $('#btnLogin').on('click', function (e) {
            e.preventDefault();
            var user = $('#txtUserName').val();
            var pass = $('#txtPassword').val();

            if (typeof user === 'undefined' || user === null || user == '') {
                // myVar is undefined or null
                hrms.notify('Login failed : Incorrect user name', 'error', 'alert', function () { });
                return;
            }

            if (typeof pass === 'undefined' || pass === null || pass == '') {
                // myVar is undefined or null
                hrms.notify('Login failed : Incorrect Password', 'error', 'alert', function () { });
                return;
            }

            login(user,pass);
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
                    window.location.href = "/Admin/Home/Index";
                }
                else {
                    hrms.notify(res.Message, 'error', 'alert', function () { });
                }
            }
        })
    }
}