///////////////////////////////////////////////////////////
////////////////////////// ROUTE //////////////////////////
///////////////////////////////////////////////////////////
function updateRoute(routeId) {
    var routeName = $("#routeNameText").val();
    var license = $("#driverList").val();
    var busText = $("#busList").val();
    var options = document.getElementById('selectedStops').options;
    var stops = [options.length];
    for (var i = 0; i < options.length; i++) {
        stops[i] = options[i].value;
    }
    if (routeName == "" || license == null || busText == "") {
        $("#routeFailureMessage > .error-text").text("Route name and Driver's name must be correctly entered.");
        rollDown($("#routeFailureMessage"));
        setTimeout(function () {
            rollUp($("#routeFailureMessage"));
        }, 6000);
        return false;
    }
    var request = {
        stopIds: stops,
        routeName: routeName,
        driverLicense: license,
        busId: busText,
        routeId: routeId
    }
    jQuery.ajaxSettings.traditional = true;
    $.post("/Admin/UpdateRoute", request, function (data) {
        if (data.success == "true") {
            $(".item-info[data-type='name']").text(request.routeName);
            $(".item-info[data-type='licensePlate']").text(data.licensePlate);
            $(".item-info[data-type='driverName']").text(data.driverName);
            $.notify.addMessage("The route was successfully updated!", { type: "success", time: 6000 });
            $("#modal").modal('hide');
        } else {
            $("#routeFailureMessage > .error-text").text("The route name already exists. Please enter a unique route name.");
            rollDown($("#routeFailureMessage"));
            setTimeout(function () {
                rollUp($("#routeFailureMessage"));
            }, 6000);
        }
    });
}

function modifyRoute(rId) {
    $.post("/Admin/ModifyRoute", { routeId: rId }, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}

///////////////////////////////////////////////////////////
/////////////////////////// BUS ///////////////////////////
///////////////////////////////////////////////////////////
function updateBus(busId) {
    var state = $("#statesList").val();
    var capacity = $("#capacityText").val();
    var license = $("#licenseText").val();
    var status = 1;
    if ($("#isActive").hasClass('active')) {
        status = "0";
    }
    if (license == "" || state == "") {
        $("#busFailureMessage > .error-text").text("Fill In All Criteria Correctly.");
        rollDown($("#busFailureMessage"));
        setTimeout(function () {
            rollUp($("#busFailureMessage"));
        }, 6000);
        return false;
    }
    var request = {
        capacity: parseInt(capacity),
        license: license,
        state: state,
        status: status,
        busId: busId
    }
    $.post("/Admin/UpdateBus", request, function (data) {
        if (data == "true") {
            var statusText = request.status == 0 ? "Active" : "Inactive";
            $(".item-info[data-type='status']").text(statusText);
            $(".item-info[data-type='state']").text(request.state);
            $(".item-info[data-type='licensePlate']").text(request.license);
            $(".item-info[data-type='capacity']").text(request.capacity);

            $.notify.addMessage("The bus was successfully updated!", { type: "success", time: 6000 });
            $("#modal").modal('hide');
        } else {
            $("#busFailureMessage > .error-text").text("The license plate already exists. Please enter a unique license plate.");
            rollDown($("#busFailureMessage"));
            setTimeout(function () {
                rollUp($("#busFailureMessage"));
            }, 6000);
        }
    });
}

function modifyBus(bId) {
    console.log("supposed to be modifying bus");
    $.post("/Admin/ModifyBus", { busId: bId }, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}

///////////////////////////////////////////////////////////
/////////////////////////// DRIVER ////////////////////////
///////////////////////////////////////////////////////////
function updateDriver(driverId) {
    var state = $("#statesList").val();
    var license = $("#licenseText").val();
    var name = $("#nameText").val();
    var foundIssue = true;
    if (name == "") {
        $("#driverFailureMessage > .error-text").text("Please enter the driver's name.");
    } else if (license == "") {
        $("#driverFailureMessage > .error-text").text("Please enter the driver's license.");
    } else if (state == "") {
        $("#driverFailureMessage > .error-text").text("Please enter the state the driver's license was issued in.");
    } else {
        foundIssue = false;
    }
    if (foundIssue) {
        rollDown($("#driverFailureMessage"));
        setTimeout(function () {
            rollUp($("#driverFailureMessage"));
        }, 6000);
        return false;
    }
    var request = {
        license: license,
        state: state,
        name: name,
        driverId: driverId
    }
    $.post("/Admin/UpdateDriver", request, function (data) {
        if (data == "true") {
            $(".item-info[data-type='name']").text(name);
            $(".item-info[data-type='state']").text(request.state);
            $(".item-info[data-type='licensePlate']").text(request.license);

            $.notify.addMessage("The driver was successfully updated!", { type: "success", time: 6000 });
            $("#modal").modal('hide');
        } else {
            $("#driverFailureMessage > .error-text").text("The license plate, state combination already exists. Please enter a unique license plate, state combination.");
            rollDown($("#driverFailureMessage"));
            setTimeout(function () {
                rollUp($("#driverFailureMessage"));
            }, 6000);
        }
    });
}

function modifyDriver(dId) {
    console.log("supposed to be modifying driver");
    $.post("/Admin/ModifyDriver", { driverId: dId }, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}

///////////////////////////////////////////////////////////
/////////////////////////// EMPLOYEE //////////////////////
///////////////////////////////////////////////////////////
function updateEmployee(employeeId) {
    var email = $("#emailText").val();
    var phone = $("#phoneText").val();
    var address = $("#addressText").val();
    var position = $("#positionText").val();
    var routeId = $("#routeList").val();
    var city = $("#cityText").val();
    var state = $("#statesList").val();
    var zip = $("#zipText").val();

    if (email == "" || !isValidPhoneNumber(phone) || address == "" || position == "" || routeId == null || city == "" || state == "--State--" || !isValidZip(zip)) {
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
        if (routeId == null) {
            messageBuilder.addMessage("must select a route");
        }
        $("#employeeFailureMessage > .error-text").text(messageBuilder.getMessage("The Employee cannot be added because you"));
        rollDown($("#employeeFailureMessage"));
        setTimeout(function () {
            rollUp($("#employeeFailureMessage"));
        }, 12000);
        return false;
    }

    var request = {
        address: address,
        assignedTo: routeId,
        city: city,
        email: email,
        employeeId: employeeId,
        phone: phone,
        position: position,
        state: state,
        zip: zip
    }
    $.post("/Admin/UpdateEmployee", request, function (data) {
        if (data == "true") {
            $(".item-info[data-type='address']").text(request.address);
            $(".item-info[data-type='city']").text(request.city);
            $(".item-info[data-type='zip']").text(request.zip);
            $(".item-info[data-type='state']").text(request.state);
            $(".item-info[data-type='email']").text(request.email);
            $(".item-info[data-type='phone']").text(request.phone);
            $(".item-info[data-type='position']").text(request.position);
            $(".item-info[data-type='assignedTo']").text(request.assignedTo);

            $.notify.addMessage("The employee was successfully updated!", { type: "success", time: 6000 });
            $("#modal").modal('hide');
        } else {
            $("#employeeFailureMessage > .error-text").text("An error occurred ");
            rollDown($("#employeeFailureMessage"));
            setTimeout(function () {
                rollUp($("#employeeFailureMessage"));
            }, 6000);
        }
    });
}

function modifyEmployee(eId) {
    console.log("supposed to be modifying employee");
    $.post("/Admin/ModifyEmployee", { employeeId: eId }, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}