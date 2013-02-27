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
            $("#routeSuccessMessage").show();
            window.setTimeout(function () {
                $("#routeSuccessMessage").fadeTo(500, 0).slideUp(500, function () {
                    $(this).hide();
                });
            }, 3000);
            $("#modal").modal('hide');
        } else {
            $("#routeFailureMessage").show();
            window.setTimeout(function () {
                $("#routeFailureMessage").fadeTo(500, 0).slideUp(500, function () {
                    $(this).hide();
                });
            }, 3000);
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
                $("#stopSuccessModalMessage").show();
                window.setTimeout(function () {
                    $("#stopSuccessModalMessage").fadeTo(500, 0).slideUp(500, function () {
                        $(this).hide();
                    });
                }, 3000);
            } else {
                $("#stopFailureMessage").show();
                window.setTimeout(function () {
                    $("#stopFailureMessage").fadeTo(500, 0).slideUp(500, function () {
                        $(this).hide();
                    });
                }, 3000);
            }
        });
    } else {
        $.post("/Admin/AddNewStop", request, function (data) {
            if (data == "true") {
                $("#stopSuccessMessage").show();
                window.setTimeout(function () {
                    $("#stopSuccessMessage").fadeTo(500, 0).slideUp(500, function () {
                        $(this).hide();
                    });
                }, 3000);
                $("#modal").modal('hide');
            } else {
                $("#stopFailureMessage").show();
                window.setTimeout(function () {
                    $("#stopFailureMessage").fadeTo(500, 0).slideUp(500, function () {
                        $(this).hide();
                    });
                }, 3000);
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
                $("#busSuccessModalMessage").show();
                window.setTimeout(function () {
                    $("#busSuccessModalMessage").fadeTo(500, 0).slideUp(500, function () {
                        $(this).hide();
                    });
                }, 3000);
            } else {
                $("#busFailureMessage").show();
                window.setTimeout(function () {
                    $("#busFailureMessage").fadeTo(500, 0).slideUp(500, function () {
                        $(this).hide();
                    });
                }, 3000);
            }
        });
    } else {
        $.post("/Admin/AddNewBus", request, function (data) {
            if (data == "true") {
                $("#busSuccessMessage").show();
                window.setTimeout(function () {
                    $("#busSuccessMessage").fadeTo(500, 0).slideUp(500, function () {
                        $(this).hide();
                    });
                }, 3000);
                $("#modal").modal('hide');
            } else {
                $("#busFailureMessage").show();
                window.setTimeout(function () {
                    $("#busFailureMessage").fadeTo(500, 0).slideUp(500, function () {
                        $(this).hide();
                    });
                }, 3000);
            }
        });
    }
}