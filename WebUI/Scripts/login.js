var Login = function() {
    var $containerLogin = $("#containerLogin");
    var $containerResetear = $("#containerResetear");
    var mostrarLogin = function () {
        $containerLogin.slideDown(); $containerResetear.slideUp();
    };
    var mostrarRestaurarCuenta = function () {
        $containerLogin.slideUp(); $containerResetear.slideDown();
    };
    var initResetearForm = function () {
        $("form", $containerResetear).submit(function (event) {
            if (!$(this).valid()) {
                return;
            }
            event.preventDefault();
            $.ajax({
                type: 'POST',
                url: $(this).attr('action'),
                data: $(this).serialize(),
                dataType: 'json',
                beforeSend: function () {
                    App.blockUI();
                },
                complete: function () {
                    App.unblockUI();
                },
                success: function (response) {
                    App.showMessage(response.Success, response.Message);
                    if (response.Success) {
                        mostrarLogin();
                    }
                }
            });
        });
    };

    //* END:CORE HANDLERS *//

    return {
        init: function () {
            initResetearForm();
        },
        mostrarRestaurarCuenta: function () {
            mostrarRestaurarCuenta();
        },
        mostrarLogin: function () {
            mostrarLogin();
        }
    };
}();