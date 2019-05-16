ymaps.ready(init);

function init() {

    var multiRoute = new ymaps.multiRouter.MultiRoute({
        referencePoints: [
            [53.756254, 87.13139],
            [54.756254, 87.13139],
            [53.756254, 86.13139]
        ],
        params: {
           
            results: 2
        }
    }, {
        boundsAutoApply: true
    });


    var myMap = new ymaps.Map("map", {
        center: [53.758, 87.13],
        zoom: 10
    }, {
        searchControlProvider: 'yandex#search'
    });
    myMap.geoObjects.add(multiRoute);

    myMap.geoObjects
        .add(new ymaps.Placemark([53.756254, 87.13139], {
            balloonContent: 'цвет <strong>воды пляжа бонди</strong>'
        }, {
            preset: 'islands#icon',
            iconColor: '#0095b6'
        }));
}