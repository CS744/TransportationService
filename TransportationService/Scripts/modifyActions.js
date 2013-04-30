///////////////////////////////////////////////////////////
////////////////////////// ROUTE //////////////////////////
///////////////////////////////////////////////////////////
function updateRoute(routeId) {
    var routeName = $("#routeNameText").val();
    var isActive = $("#routeActiveButton").hasClass('active') ? true : false;
    var options = document.getElementById('selectedStops').options;
    var stops = [];
    for (var i = 0; i < options.length; i++) {
        stops.push(options[i].value);
    }
    var buses = [];
    var drivers = [];
    var times = [];
    var statuses = [];
    var entryCount = $("#driverBusList option").size();
    for (var i = 0; i < entryCount; i++) {
        var entry = $('#driverBusList option')[i];
        var value = entry.value;
        var valueArray = value.split(";");
        buses.push(valueArray[0]);
        drivers.push(valueArray[1]);
        times.push(valueArray[2]);
        statuses.push(valueArray[3]);
    }
    var noBuses = true;
    var noDrivers = true;
    var noneActive = true;
    var duplicateDepartureTime = false;
    for (var i = 0; i < buses.length; i++) {
        if (buses[i] != "None") {
            noBuses = false;
            break;
        }
    }
    for (var i = 0; i < drivers.length; i++) {
        if (drivers[i] != "None") {
            noDrivers = false;
            break;
        }
    }
    for (var i = 0; i < statuses.length; i++) {
        if (statuses[i] != "INACTIVE") {
            noneActive = false;
            break;
        }
    }
    for (var i = 0; i < times.length; i++) {
        for (var ndx = 0; ndx < times.length; ndx++) {
            if (i != ndx && times[i] == times[ndx] && statuses[i] == "ACTIVE" && statuses[ndx] == "ACTIVE") {
                duplicateDepartureTime = true;
                break;
            }
        }
    }

    if (routeName == "" || (isActive && (stops.length == 0 || noBuses || noDrivers || noneActive || duplicateDepartureTime))) {
        var messageBuilder = new MessageBuilder();
        if (routeName == "") {
            messageBuilder.addMessage("must include a name");
        }
        if (isActive && stops.length == 0) {
            messageBuilder.addMessage("must include at least one stop (or change the route to inactive)");
        }
        if (isActive && (noBuses || noDrivers || noneActive)) {
            messageBuilder.addMessage("must include at least one active bus/driver combination (or change the route to inactive)");
        }
        if (isActive && duplicateDepartureTime)
            messageBuilder.addMessage("must have unique departure times (for all active routes)");
        $("#routeFailureMessage > .error-text").text(messageBuilder.getMessage("The route cannot be added because you"));
        rollDown($("#routeFailureMessage"));
        setTimeout(function () {
            rollUp($("#routeFailureMessage"));
        }, 12000);
        return false;
    }
    var request = {
        stopIds: stops,
        routeName: routeName,
        isActive: isActive,
        buses: buses,
        drivers: drivers,
        times: times,
        statuses: statuses,
        routeId: routeId
    }
    jQuery.ajaxSettings.traditional = true;
    $.post("/Admin/UpdateRoute", request, function (data) {
        if (data.success == "true") {
            $("#view-tr-" + data.id + " > td:nth-child(2)").text(request.routeName);
            $("#view-tr-" + data.id + " > td:nth-child(4)").text(request.isActive ? "Active" : "Inactive");
            $("#view-tr-" + data.id + " > td:nth-child(5)").text(data.stops);
            $("#view-tr-" + data.id + " > td:nth-child(6)").text(data.assigned + "/" + data.capacity);
            $.notify.addMessage("The route was successfully updated!", { type: "success", time: 6000 });
            $("#modal").modal('hide');
            $("#view-item-table").trigger("update");
        } else {
            $("#routeFailureMessage > .error-text").text("The route name already exists. Please enter a unique route name.");
            rollDown($("#routeFailureMessage"));
            setTimeout(function () {
                rollUp($("#routeFailureMessage"));
            }, 6000);
        }
    });
}

function modifyRoute(event, rId) {
    $.post("/Admin/ModifyRoute", { routeId: rId }, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
    event.stopPropagation();
    event.preventDefault();
    return false;
}

///////////////////////////////////////////////////////////
/////////////////////////// BUS ///////////////////////////
///////////////////////////////////////////////////////////
function updateBus(busId) {
    var state = $("#statesList").val();
    var capacity = $("#capacityText").val();
    var license = $("#licenseText").val();
    if (!isValidCapacity(capacity) || !isValidPlate(license) || state == "--State--") {
        var messageBuilder = new MessageBuilder();
        if (!isValidCapacity(capacity)) {
            messageBuilder.addMessage("must enter a valid capacity");
        }
        if (!isValidPlate(license)) {
            messageBuilder.addMessage("must enter a six character license plate");
        }
        if (state == "--State--") {
            messageBuilder.addMessage("must select a state");
        }
        $("#busFailureMessage > .error-text").text(messageBuilder.getMessage("The bus cannot be added because you"));
        rollDown($("#busFailureMessage"));
        setTimeout(function () {
            rollUp($("#busFailureMessage"));
        }, 12000);
        return false;
    }
    var request = {
        capacity: parseInt(capacity),
        license: license,
        state: state,
        busId: busId
    }
    $.post("/Admin/UpdateBus", request, function (data) {
        if (data.success == "true") {
            $("#view-tr-" + data.id + " > td:nth-child(2)").text(request.license);
            $("#view-tr-" + data.id + " > td:nth-child(3)").text(request.state);
            $("#view-tr-" + data.id + " > td:nth-child(4)").text(request.capacity);

            $.notify.addMessage("The bus was successfully updated!", { type: "success", time: 6000 });
            $("#modal").modal('hide');
            $("#view-item-table").trigger("update");
        } else {
            $("#busFailureMessage > .error-text").text(data.reason);
            rollDown($("#busFailureMessage"));
            setTimeout(function () {
                rollUp($("#busFailureMessage"));
            }, 6000);
        }
    });
}

function modifyBus(event, bId) {
    console.log("supposed to be modifying bus");
    $.post("/Admin/ModifyBus", { busId: bId }, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
    event.stopPropagation();
    event.preventDefault();
    return false;
}

///////////////////////////////////////////////////////////
/////////////////////////// DRIVER ////////////////////////
///////////////////////////////////////////////////////////
function updateDriver(driverId) {
    var state = $("#statesList").val();
    var license = $("#licenseText").val();
    var name = $("#nameText").val();
    if (state == "--State--" || name == "" || !isValidLicense(license)) {
        var messageBuilder = new MessageBuilder();
        if (name == "") {
            messageBuilder.addMessage("must include a name");
        }
        if (!isValidLicense(license)) {
            messageBuilder.addMessage("must enter a valid license (1 letter followed by 13 digits)");
        }
        if (state == "--State--") {
            messageBuilder.addMessage("must select a state");
        }
        $("#driverFailureMessage > .error-text").text(messageBuilder.getMessage("The driver cannot be added because you"));
        rollDown($("#driverFailureMessage"));
        setTimeout(function () {
            rollUp($("#driverFailureMessage"));
        }, 12000);
        return false;
    }
    var request = {
        license: license,
        state: state,
        name: name,
        driverId: driverId
    }
    $.post("/Admin/UpdateDriver", request, function (data) {
        if (data.success == "true") {
            $("#view-tr-" + data.id + " > td:nth-child(2)").text(name);
            $("#view-tr-" + data.id + " > td:nth-child(3)").text(request.license);
            $("#view-tr-" + data.id + " > td:nth-child(4)").text(request.state);

            $.notify.addMessage("The driver was successfully updated!", { type: "success", time: 6000 });
            $("#modal").modal('hide');
            $("#view-item-table").trigger("update");
        } else {
            $("#driverFailureMessage > .error-text").text("The license plate, state combination already exists. Please enter a unique license plate, state combination.");
            rollDown($("#driverFailureMessage"));
            setTimeout(function () {
                rollUp($("#driverFailureMessage"));
            }, 6000);
        }
    });
}

function modifyDriver(event, dId) {
    console.log("supposed to be modifying driver");
    $.post("/Admin/ModifyDriver", { driverId: dId }, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
    event.stopPropagation();
    event.preventDefault();
    return false;
}

///////////////////////////////////////////////////////////
/////////////////////////// EMPLOYEE //////////////////////
///////////////////////////////////////////////////////////
function updateEmployee(employeeId) {
    var email = $("#emailText").val();
    var phone = $("#phoneText").val();
    var address = $("#addressText").val();
    var position = $("#positionText").val();
    var morningRouteId = $("#morningRouteList").val();
    var eveningRouteId = $("#eveningRouteList").val();
    var city = $("#cityText").val();
    var state = $("#statesList").val();
    var zip = $("#zipText").val();

    if (email == "" || !isValidPhoneNumber(phone) || address == "" || position == "" || city == "" || state == "--State--" || !isValidZip(zip)) {
        var messageBuilder = new MessageBuilder();
        if (position == "") {
            messageBuilder.addMessage("must include a position");
        }
        if (email == "") {
            messageBuilder.addMessage("must include an email address");
        }
        if (!isValidPhoneNumber(phone)) {
            messageBuilder.addMessage("must have valid phone number (10 digits)");
        }
        if (address == "") {
            messageBuilder.addMessage("must include an address");
        }
        if (city == "") {
            messageBuilder.addMessage("must include a city");
        }
        if (state == "--State--") {
            messageBuilder.addMessage("must select a state");
        }
        if (!isValidZip(zip)) {
            messageBuilder.addMessage("must have valid zip code (5 digits)");
        }

        $("#employeeFailureMessage > .error-text").text(messageBuilder.getMessage("The employee cannot be added because you"));
        rollDown($("#employeeFailureMessage"));
        setTimeout(function () {
            rollUp($("#employeeFailureMessage"));
        }, 12000);
        return false;
    }

    if (morningRouteId == "--Route--") {
        morningRouteId = -1;
    }

    if (eveningRouteId == "--Route--") {
        eveningRouteId = -1;
    }

    var request = {
        address: address,
        morningRouteId: morningRouteId,
        eveningRouteId: eveningRouteId,
        city: city,
        email: email,
        employeeId: employeeId,
        phone: phone,
        position: position,
        state: state,
        zip: zip
    }
    $.post("/Admin/UpdateEmployee", request, function (data) {
        if (data.success == "true") {
            //name
            //ssn
            $("#view-tr-" + data.id + " > td:nth-child(4)").text(request.position);
            $("#view-tr-" + data.id + " > td:nth-child(5)").text(request.email);
            //gender
            $("#view-tr-" + data.id + " > td:nth-child(7)").text(request.phone);
            $("#view-tr-" + data.id + " > td:nth-child(8)").text(request.address + ", " + request.city + ", " + request.state + " " + request.zip);
            $("#view-tr-" + data.id + " > td:nth-child(9)").text(request.morningRouteId == -1 ? "none" : request.morningRouteId + data.morningRouteName);
            $("#view-tr-" + data.id + " > td:nth-child(10)").text(request.eveningRouteId == -1 ? "none" : request.eveningRouteId + data.eveningRouteName);

            //$(".item-info[data-type='morningAssignedTo']").text(request.morningAssignedTo);
            //$(".item-info[data-type='eveningAssignedTo']").text(request.eveningAssignedTo);

            $.notify.addMessage("The employee was successfully updated!", { type: "success", time: 6000 });
            $("#modal").modal('hide');

            $("#view-item-table").trigger("update");
        } else {
            $("#employeeFailureMessage > .error-text").text("An error occurred ");
            rollDown($("#employeeFailureMessage"));
            setTimeout(function () {
                rollUp($("#employeeFailureMessage"));
            }, 6000);
        }
    });
}

function modifyEmployee(event, eId) {
    console.log("supposed to be modifying employee");
    $.post("/Admin/ModifyEmployee", { employeeId: eId }, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
    event.stopPropagation();
    event.preventDefault();
    return false;
}
