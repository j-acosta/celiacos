var Admin = function() {
    var $modalGenerico = $("#modalGenerico");
    var showModalEmpresa = function (url) {
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'html',
            beforeSend: function () {
                App.blockUI();
            },
            complete: function () {
                App.unblockUI();
            },
            success: function (respuesta) {
                $modalGenerico.html(respuesta);
                $modalGenerico.modal();
            }
        });
    };

    //* END:CORE HANDLERS *//

    return {
        showModalEmpresa: function (url) {
            showModalEmpresa(url);
        }
    };
}();