var Maps = function () {
    var $mapa = $("#mapa");
    var mostrarMapa = function (json) {
        $mapa.goMap({
            latitude: -34.9214118,
            longitude: -57.9567776,
            zoom: 13,
            maptype: "TERRAIN",
        });
        var bandera = 'img/gluten_free (3).png';

        for (i = 0; i < json.length; i++) {
            $.goMap.createMarker({
                latitude: json[i].Latitud,
                longitude: json[i].Longitud,
                title: json[i].Nombre,
                icon: bandera,
            html: {
                content: '<div class="row" style="margin-right:0px;margin-left:0px;width: 360px;">'+
                            '<div class="col-md-5" style="text-align: center;">' +
                                '<img src=' + json[i].Foto.substr(2) + ' class="img-thumbnail" style="height:100px;width: 125px;margin-top: 27px;"/>' +
                            '</div>'+
                            '<div class="col-md-7">' +
                                '<p style="font-weight: bold;"> Nombre</p> <p>' + json[i].Nombre + '</P>' +
                                '<p style="font-weight: bold;">Hora de Atencion</p> <p>' + json[i].horaAtencion + '</p>' +
                                '<p style="font-weight: bold;">ubicacion</p> <p>' + json[i].Ubicacion+'</p>'+
                            '</div>' +
                        '</div>'
            }
            })
        }

    };

    //* END:CORE HANDLERS *//

    return {
        init: function (json) {
            mostrarMapa(json);
        }
    };
}();