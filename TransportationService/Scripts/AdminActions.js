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
    var busText = $("#busList").val()
    var request = {
        stopIds: route.stops,
        routeName: routeName,
        driverName: driverName,
        busId: parseInt(busText)
    }
    jQuery.ajaxSettings.traditional = true;
    $.post("/Admin/AddNewRoute", request, function (data) {
        if (data == "true") {
            $.notify.addMessage("The route was successfully added!", { type: "success", time: 7000 });
            $("#modal").modal('hide');
        } else {
            rollDown($("#routeFailureMessage"));
            setTimeout(function () {
                rollUp($("#routeFailureMessage"));
            }, 7000);
        }
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
            if (data == "true") {
                $("#locationText").val("");
                $.notify.addMessage("The stop was successfully added!", { type: "success", time: 7000 });
            } else {
                rollDown($("#stopFailureMessage"));
                setTimeout(function () {
                    rollUp($("#stopFailureMessage"));
                }, 7000);
            }
        });
    } else {
        $.post("/Admin/AddNewStop", request, function (data) {
            if (data == "true") {
                $.notify.addMessage("The stop was successfully added!", { type: "success", time: 7000 });
                $("#modal").modal('hide');
            } else {
                rollDown($("#stopFailureMessage"));
                setTimeout(function () {
                    rollUp($("#stopFailureMessage"));
                }, 7000);
            }
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


function addBus() {
    console.log("supposed to be adding bus");
    $.post("/Admin/AddBus", {}, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}

function addNewBus(addAnother) {
    var capacity = $("#capacityText").val();
    var license = $("#licenseText").val();
    var request = {
        capacity: parseInt(capacity),
        license: license
    }
    if (addAnother) {
        $.post("/Admin/AddNewBus", request, function (data) {
            if (data == "true") {
                $("#capacityText").val("");
                $("#licenseText").val("");
                $.notify.addMessage("The bus was successfully added!", { type: "success", time: 7000 });
            } else {
                rollDown($("#busFailureMessage"));
                setTimeout(function () {
                    rollUp($("#busFailureMessage"));
                }, 7000);
            }
        });
    } else {
        $.post("/Admin/AddNewBus", request, function (data) {
            if (data == "true") {
                $.notify.addMessage("The bus was successfully added!", { type: "success", time: 7000 });
                $("#modal").modal('hide');
            } else {
                rollDown($("#busFailureMessage"));
                setTimeout(function () {
                    rollUp($("#busFailureMessage"));
                }, 7000);
            }
        });
    }
}