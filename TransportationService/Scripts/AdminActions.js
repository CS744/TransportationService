// Simon Weisse


function addRoute() {
    $.post("/Admin/AddRoute", {}, function (data) {

        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}
function addNewRoute() {
    var request = {
        stopIds: route.stops
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

function addNewStop() {
    var streetName = $("#streetText").val();
    var streetNumber = $("#numberText").val();
    var request = {
        streetName: streetName,
        streetNumber: streetNumber
    }
    $.post("/Admin/AddNewStop", request, function (data) {
        $("#streetText").val("");
        $("#numberText").val("");
    });
}

function addStopToRoute(elem) {
    var stopId = elem.dataset['stopid'];
    var html = "<i class='added-stop icon-arrow-right'></i><div class='added-stop'>" + stopId + "</div>";
    var addedStops = $("#added-stops");
    addedStops.html(addedStops.html() + html);
    $(elem).remove();
    route.addStop(stopId);
}