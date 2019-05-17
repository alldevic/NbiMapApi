ymaps.ready(function (ym) {
    var myMap = new ymaps.Map("map", {
        center: [53.758, 87.13],
        zoom: 10
    }, {
        searchControlProvider: 'yandex#search'
    });


    jQuery.getJSON('http://localhost:8080/geojson', function (json) {
        var geoObjects = ym.geoQuery(json)
            .addToMap(myMap)
            .applyBoundsToMap(myMap, {
                checkZoomRange: true
            });
        
        jQuery.getJSON('http://localhost:8080/geopath', function (json) {
            var myPolyline = new ymaps.Polyline(
                json
                , {
                    balloonContent: "Оптимальный маршрут"
                }, {
                    balloonCloseButton: true,
                    strokeColor: "#000000",
                    strokeWidth: 4,
                    strokeOpacity: 0.5
                });

            myMap.geoObjects.add(myPolyline);    
        });

        
    });


});
