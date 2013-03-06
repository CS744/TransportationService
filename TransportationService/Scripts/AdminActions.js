// Simon Weisse


function addRoute() {
    $.post("/Admin/AddRoute", {}, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}
function addNewRoute() {
    var routeName = $("#routeNameText").val();
    var license = $("#driverList").val();
    var busText = $("#busList").val();
    var isToWork = true;
    if ($("#toHomeButton").hasClass('active')) {
        isToWork = false;
    }
    var request = {
        stopIds: route.stops,
        routeName: routeName,
        driverLicense: license,
        busId: parseInt(busText),
        startsAtWork: isToWork
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
    var state = $("#selectedState").val();
    var capacity = $("#capacityText").val();
    var license = $("#licenseText").val();
    var request = {
        capacity: parseInt(capacity),
        license: license,
        state: state
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

function addDriver() {
    console.log("supposed to be adding driver");
    $.post("/Admin/AddDriver", {}, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}

function addNewDriver(addAnother) {
    var state = $("#selectedState").val();
    var gender = $("#genderText").val();
    var name = $("#nameText").val();
    var license = $("#licenseText").val();
    var request = {
        gender: gender,
        state: state,
        name: name,
        license: license
    }
    if (addAnother) {
        $.post("/Admin/AddNewDriver", request, function (data) {
            if (data == "true") {
                $("#nameText").val("");
                $("#licenseText").val("");
                $.notify.addMessage("The driver was successfully added!", { type: "success", time: 3000 });
            } else {
                rollDown($("#driverFailureMessage"));
                setTimeout(function () {
                    rollUp($("#driverFailureMessage"));
                }, 3000);
            }
        });
    } else {
        $.post("/Admin/AddNewDriver", request, function (data) {
            if (data == "true") {
                $.notify.addMessage("The driver was successfully added!", { type: "success", time: 3000 });
                $("#modal").modal('hide');
            } else {
                rollDown($("#driverFailureMessage"));
                setTimeout(function () {
                    rollUp($("#driverFailureMessage"));
                }, 3000);
            }
        });
    }
}

function addEmployee() {
    console.log("supposed to be adding employee");
    $.post("/Admin/AddEmployee", {}, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}

function addNewEmployee(addAnother) {
    var gender = $("#genderText").val();
    var email = $("#emailText").val();
    var phone = $("#phoneText").val();
    var address = $("#addressText").val();
    var routeId = $("#routeList").val();
    var ssn = $("#ssnText").val();
    var position = $("#positionText").val();
    var name = $("#nameText").val();
    var request = {
        gender: gender,
        email: email,
        phone: phone,
        address: address,
        routeId: parseInt(routeId),
        ssn: ssn,
        position: position,
        name: name
    }
    if (addAnother) {
        $.post("/Admin/AddNewEmployee", request, function (data) {
            if (data == "true") {
                $("#ssnText").val("");
                $("#positionText").val("");
                $("#nameText").val("");
                $.notify.addMessage("The employee was successfully added!", { type: "success", time: 3000 });
            } else {
                rollDown($("#employeeFailureMessage"));
                setTimeout(function () {
                    rollUp($("#employeeFailureMessage"));
                }, 3000);
            }
        });
    } else {
        $.post("/Admin/AddNewEmployee", request, function (data) {
            if (data == "true") {
                $.notify.addMessage("The employee was successfully added!", { type: "success", time: 3000 });
                $("#modal").modal('hide');
            } else {
                rollDown($("#employeeFailureMessage"));
                setTimeout(function () {
                    rollUp($("#employeeFailureMessage"));
                }, 3000);
            }
        });
    }
}