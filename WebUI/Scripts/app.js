var App = function () {

    var assetsPath = '../Content/assets/';

    var globalImgPath = 'img/';

    var showMessage = function (isAllOk, message) {
        toastr.options = {
            "closeButton": false ? false : true,
            "debug": false,
            "positionClass": "toast-top-full-width",
            "onclick": null,
            "showDuration": "1000",
            "hideDuration": "1000",
            "timeOut": isAllOk ? "2500" : "8000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }

        if (isAllOk) {
            toastr.success(message, "Operaci&oacute;n exitosa.");
        } else {
            toastr.error(message, "Ups. Algo sali&oacute; mal.");
        }
    };
    var handleAjaxError = function () {
        $(document).ajaxError(function (event, jqxhr, settings, thrownError) {
            switch (jqxhr.status) {
                case 401:
                    App.showMessage(false, thrownError);
                    setTimeout(function () { window.location.href = window.location.href; }, 3000);
                    return;
                    break;
                default:
                    if (thrownError === "timeout") {
                        App.showMessage(false, "La consulta est&aacute; tardando más de lo esperado. Se ha cancelado la operaci&oacute;n. Intente nuevamente.");
                    } else {
                        App.showMessage(false, "Ha ocurrido un error inesperado. " + thrownError + " Refresque la pantalla e intente nuevamente.");
                    }
                    break;
            }
        });
    };
    var handleValidate = function () {
        if (!jQuery.validator) {
            return;
        }
        jQuery.validator.setDefaults({
            highlight: function (element) {
                $(element).closest('.form-group').addClass('has-error');
            },
            unhighlight: function (element) {
                $(element).closest('.form-group').removeClass('has-error');
            },
            errorElement: 'span',
            errorClass: 'help-block',
            errorPlacement: function (error, element) {
                if (element.parent('.input-group').length) {
                    error.insertAfter(element.parent());
                } else {
                    error.insertAfter(element);
                }
            }
        });
    };
    var initMenu = function () {
        var url = window.location;
        $('ul.nav li a').each(function () {
            if (this.href == url) {
                $("ul.nav li").each(function () {
                    if ($(this).hasClass("active")) {
                        $(this).removeClass("active");
                    }
                });
                $(this).parent().addClass('active');
            }
        });
    };

    //* END:CORE HANDLERS *//

    return {

        init: function () {
            initMenu();
            handleAjaxError();
            handleValidate();            
        },
        //inicialización de componentes core luego del ajax complete
        initAjax: function () { },
        //TODO: pasar cuerpo arriba..
        scrollTo: function (el, offeset) {
            var pos = (el && el.size() > 0) ? el.offset().top : 0;

            if (el) {
                if ($('body').hasClass('page-header-fixed')) {
                    pos = pos - $('.page-header').height();
                } else if ($('body').hasClass('page-header-top-fixed')) {
                    pos = pos - $('.page-header-top').height();
                } else if ($('body').hasClass('page-header-menu-fixed')) {
                    pos = pos - $('.page-header-menu').height();
                }
                pos = pos + (offeset ? offeset : -1 * el.height());
            }

            $('html,body').animate({
                scrollTop: pos
            }, 'slow');
        },
        // function to scroll to the top
        scrollTop: function () {
            App.scrollTo();
        },
        // wrApper function to  block element(indicate loading)
        blockUI: function ($element) {
            var options = {};
            html = '<div class="loading-message"><img src="' + globalImgPath + 'loading-spinner-grey.gif" align=""><span>&nbsp;&nbsp;PROCESANDO...</span></div>';

            if ($element) { // element blocking
                if ($element.height() <= ($(window).height())) {
                    options.cenrerY = true;
                }
                $element.block({
                    message: html,
                    baseZ: options.zIndex ? options.zIndex : 1000,
                    centerY: options.cenrerY !== undefined ? options.cenrerY : false,
                    css: {
                        top: '10%',
                        border: '0',
                        padding: '0',
                        backgroundColor: 'none'
                    },
                    overlayCSS: {
                        backgroundColor: options.overlayColor ? options.overlayColor : '#000',
                        opacity: options.boxed ? 0.05 : 0.1,
                        cursor: 'wait'
                    }
                });
            } else { // page blocking
                $.blockUI({
                    message: html,
                    baseZ: options.zIndex ? options.zIndex : 1000,
                    css: {
                        border: '0',
                        padding: '0',
                        backgroundColor: 'none'
                    },
                    overlayCSS: {
                        backgroundColor: options.overlayColor ? options.overlayColor : '#000',
                        opacity: options.boxed ? 0.05 : 0.1,
                        cursor: 'wait'
                    }
                });
            }
        },
        // wrApper function to  un-block element(finish loading)
        unblockUI: function ($element) {
            if ($element) {
                $element.unblock({
                    onUnblock: function () {
                        $element.css('position', '');
                        $element.css('zoom', '');
                    }
                });
            } else {
                $.unblockUI();
            }
        },
        showMessage: function (isAllOk, message) {
            showMessage(isAllOk, message);
        }
    };

}();


jQuery(document).ready(function () {
    App.init();
});