var hrms = {
    notify: function (message, title, type, okcallback) {
        if (type === 'alert') {
            alertify.alert(title, message, function () {
                if (title === 'error') {
                    alertify.error(message);
                }
                else {
                    alertify.success("Ok");
                }
            });
        }
        else if (type === 'confirm') {
            alertify.confirm(
                title,
                message,
                function () {
                    alertify.success("Ok");
                    okcallback();
                },
                function () {
                    alertify.error("Cancel")
                });
        }
        else if (type === 'success') {
            alertify.success(message);
        }
        else if (type === 'error') {
            alertify.error(message);
        }
        else if (type === 'warning') {
            alertify.warning(message);
        }
    }
}

$(document).ajaxSend(function (e, xhr, options) {

    console.log(options.type);

    if (options.type.toUpperCase() == "POST" || options.type.toUpperCase() == "PUT") {
        var token = $('form').find("input[name='__RequestVerificationToken']").val();

        console.log(token);

        xhr.setRequestHeader("RequestVerificationToken", token);
    }
});