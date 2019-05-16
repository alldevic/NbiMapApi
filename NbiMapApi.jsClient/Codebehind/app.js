ymaps.ready(init);

function init() {
    var myMap = new ymaps.Map("map", {
            center: [35.76, 37.64],
            zoom: 10
        }, {
            searchControlProvider: 'yandex#search'
        });


    myMap.geoObjects
        .add(new ymaps.Placemark([87.13139, 53.756254], {
            balloonContent: 'цвет <strong>воды пляжа бонди</strong>'
        }, {
            preset: 'islands#icon',
            iconColor: '#0095b6'
        }));
}