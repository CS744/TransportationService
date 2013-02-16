// Simon Weisse


function addRoute() {
    $.post("/Admin/AddRoute", {}, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}
function addNewRoute() {
    var routeName = $("#routeNameText").val()
    var driverName = $("#driverNameText").val()
    var request = {
        stopIds: route.stops,
        routeName: routeName,
        driverName: driverName
    }
    jQuery.ajaxSettings.traditional = true;
    $.post("/Admin/AddNewRoute", request, function (data) {
        $("#modal").modal('hide');
    });
}

function addStop() {
    console.log("supposed to be adding stop");
    $.post("/Admin/AddStop", {}, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}

function addNewStop(addAnother) {
    var location = $("#locationText").val();
    var request = {
        location: location,
    }
    if (addAnother) {
        $.post("/Admin/AddNewStop", request, function (data) {
            $("#locationText").val("");
        });
    } else {
        $.post("/Admin/AddNewStop", request, function (data) {
            $("#modal").modal('hide');
        });
    }
}

function addStopToRoute(elem) {
    var stopId = elem.dataset['stopid'];
    var html = "<i class='added-stop icon-arrow-right'></i><div class='added-stop'>" + stopId + "</div>";
    var addedStops = $("#added-stops");
    addedStops.html(addedStops.html() + html);
    $(elem).remove();
    route.addStop(stopId);
}