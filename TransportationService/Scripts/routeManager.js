var Route = function () {
    this.stops = [];
    this.addStop = function (stopId) {
        route.stops.push(stopId);
    }
}
var route = new Route();