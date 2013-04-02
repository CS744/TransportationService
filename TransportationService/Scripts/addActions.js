function addNewRoute() {
    var routeName = $("#routeNameText").val();
    var license = $("#driverList").val();
    var busText = $("#busList").val();
    var isToWork = true;
    var options = document.getElementById('selectedStops').options;
    var stops = [];
    for (var i = 0; i < options.length; i++)
        stops.push(options[i].value);
    if ($("#toHomeButton").hasClass('active')) {
        isToWork = false;
    }
    if (routeName == "" || license == null || busText == null || stops.length == 0) {
        var messageBuilder = new MessageBuilder();
        if (routeName == "") {
            messageBuilder.addMessage("must include a name");
        }
        if (license == null) {
            messageBuilder.addMessage("must add an unused Driver");
        }
        if (busText == null) {
            messageBuilder.addMessage("must add an unused Bus");
        }
        if (stops.length == 0) {
            messageBuilder.addMessage("must include at least one stop");
        }
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
        driverLicense: license,
        busId: parseInt(busText),
        startsAtWork: isToWork
    }
    jQuery.ajaxSettings.traditional = true;
    $.post("/Admin/AddNewRoute", request, function (data) {
        if (data.success == "true") {
            $.notify.addMessage("The route was successfully added!", { type: "success", time: 6000 });
            $("#modal").modal('hide');
            var newItem = $("#view-routes").siblings(".view-child-inner").append("<div class='item-element' data-type='route' data-id='"+data.id+"' onclick=\"viewRoute('" + data.id + "')\">" + getInnerViewItem(routeName) + "</div>").children().last();
            newItem.find(".delete-item").click(function (event) { deleteItemClick(event, $(this)); });
        } else {
            $("#routeFailureMessage > .error-text").text("The route name already exists. Please enter a unique route name.");
            rollDown($("#routeFailureMessage"));
            setTimeout(function () {
                rollUp($("#routeFailureMessage"));
            }, 6000);
        }
    });
}

function addNewStop(addAnother) {
    var location = $("#locationText").val();
    var request = {
        location: location,
    }
    if (location == "") {
        $("#stopFailureMessage > .error-text").text("You must enter a location.");
        rollDown($("#stopFailureMessage"));
        setTimeout(function () {
            rollUp($("#stopFailureMessage"));
        }, 6000);
        return false;
    }
    $.post("/Admin/AddNewStop", request, function (data) { addNewStopCallback(data, !addAnother, location); });
}
function addNewStopCallback(data, hideModal, location) {
    if (data.success == "true") {
        var newItem = $("#view-Stops").siblings(".view-child-inner").append("<div class='item-element' data-type='stop' data-id='" + data.id + "'  onclick=\"viewStop('" + data.id + "')\">" + getInnerViewItem(location) + "</div>").children().last();
        newItem.find(".delete-item").click(function (event) { deleteItemClick(event, $(this)); });
        $.notify.addMessage("The stop was successfully added!", { type: "success", time: 6000 });
        if (hideModal) {
            $("#modal").modal('hide');
        } else {
            $("#locationText").val("");
        }
    } else {
        $("#stopFailureMessage > .error-text").text("The stop location already exists. Please enter a unique stop location.");
        rollDown($("#stopFailureMessage"));
        setTimeout(function () {
            rollUp($("#stopFailureMessage"));
        }, 6000);
    }
}

function addNewBus(addAnother) {
    var state = $("#statesList").val();
    var capacity = $("#capacityText").val();
    var license = $("#licenseText").val().toUpperCase();
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
        $("#busFailureMessage > .error-text").text(messageBuilder.getMessage("The Bus cannot be added because you"));
        rollDown($("#busFailureMessage"));
        setTimeout(function () {
            rollUp($("#busFailureMessage"));
        }, 12000);
        return false;
    }
    var request = {
        capacity: parseInt(capacity),
        license: license,
        state: state
    }
    $.post("/Admin/AddNewBus", request, function (data) { addNewBusCallback(data, !addAnother, license); });
}

function addNewBusCallback(data, hideModal, license) {
    if (data.success == "true") {
        $.notify.addMessage("The bus was successfully added!", { type: "success", time: 6000 });
        var newItem = $("#view-buses").siblings(".view-child-inner").append("<div class='item-element' data-type='bus' data-id='" + data.id + "'  onclick=\"viewBus('" + data.id + "')\">" + getInnerViewItem(license) + "</div>").children().last();
        newItem.find(".delete-item").click(function (event) { deleteItemClick(event, $(this)); });
        if (hideModal) {
            $("#modal").modal('hide');
        }
        else {
            $("#capacityText").val("");
            $("#licenseText").val("");
            $("#statesList").val("--State--");
        }
    } else {
        $("#busFailureMessage > .error-text").text("The license plate already exists. Please enter a unique license plate.");
        rollDown($("#busFailureMessage"));
        setTimeout(function () {
            rollUp($("#busFailureMessage"));
        }, 6000);
    }
}

function addNewDriver(addAnother) {
    var state = $("#statesList").val();
    var name = $("#nameText").val();
    var license = $("#licenseText").val().toUpperCase();
    if (state == "--State--" || name == "" || !isValidLicense(license)) {
        var messageBuilder = new MessageBuilder();
        if (name == "") {
            messageBuilder.addMessage("must include a name");
        }
        if (!isValidLicense(license)) {
            messageBuilder.addMessage("must enter a valid license (1 character followed by 13 digits)");
        }
        if (state == "--State--") {
            messageBuilder.addMessage("must select a state");
        }
        $("#driverFailureMessage > .error-text").text(messageBuilder.getMessage("The Driver cannot be added because you"));
        rollDown($("#driverFailureMessage"));
        setTimeout(function () {
            rollUp($("#driverFailureMessage"));
        }, 12000);
        return false;
    }
    var request = {
        state: state,
        name: name,
        license: license
    }
    $.post("/Admin/AddNewDriver", request, function (data) { addNewDriverCallback(data, !addAnother, name); });
}
function addNewDriverCallback(data, hideModal, name) {
    if (data.success == "true") {
        var newItem = $("#view-Drivers").siblings(".view-child-inner").append("<div class='item-element' data-type='driver' data-id='" + data.id + "'  onclick=\"viewDriver('" + data.driverId + "')\">" + getInnerViewItem(name) + "</div>").children().last();
        newItem.find(".delete-item").click(function (event) { deleteItemClick(event, $(this)); });
        $.notify.addMessage("The driver was successfully added!", { type: "success", time: 6000 });
        if (hideModal) {
            $("#modal").modal('hide');
        }
        else {
            $("#nameText").val("");
            $("#licenseText").val("");
            $("#statesList").val("--State--");
        }
    } else {
        $("#driverFailureMessage > .error-text").text("The driver's license already exists. Please enter a unique license.");
        rollDown($("#driverFailureMessage"));
        setTimeout(function () {
            rollUp($("#driverFailureMessage"));
        }, 6000);
    }
}

function addNewEmployee(addAnother) {
    var isMale = $("#isMale").hasClass("active");
    var email = $("#emailText").val();
    var phone = $("#phoneText").val();
    var address = $("#addressText").val();
    var ssn = $("#ssnText").val();
    var position = $("#positionText").val();
    var name = $("#nameText").val();
    var routeId = $("#routeList").val();
    var city = $("#cityText").val();
    var state = $("#statesList").val();
    var zip = $("#zipText").val();

    if (email == "" || !isValidPhoneNumber(phone) || address == "" || !isValidSSN(ssn) || position == "" || name == "" || routeId == null || city == "" || state == "--State--" || !isValidZip(zip)) {
        var messageBuilder = new MessageBuilder();
        if (name == "") {
            messageBuilder.addMessage("must include a name");
        }
        if (!isValidSSN(ssn)) {
            messageBuilder.addMessage("must have valid social security number (9 digits)");
        }
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
            messageBuilder.addMessage("must create a route");
        }
        $("#employeeFailureMessage > .error-text").text(messageBuilder.getMessage("The Employee cannot be added because you"));
        rollDown($("#employeeFailureMessage"));
        setTimeout(function () {
            rollUp($("#employeeFailureMessage"));
        }, 12000);
        return false;
    }

    var request = {
        isMale: isMale,
        email: email,
        phone: phone,
        address: address,
        city: city,
        state: state,
        routeId: parseInt(routeId),
        ssn: ssn,
        position: position,
        name: name,
        zip: zip
    }


    $.post("/Admin/addNewEmployee", request, function (data) { addNewEmployeeCallback(data, !addAnother, name); });

}

function addNewEmployeeCallback(data, hideModal, name) {
    if (data.success == "true") {
        var newItem = $("#view-Employees").siblings(".view-child-inner").append("<div class='item-element' data-type='employee' data-id='" + data.id + "'  onclick=\"viewEmployee('" + data.employeeId + "')\">" + getInnerViewItem(name) + "</div>").children().last();
        newItem.find(".delete-item").click(function (event) { deleteItemClick(event, $(this)); });
        $.notify.addMessage("The employee was successfully added!", { type: "success", time: 6000 });
        if (hideModal) {
            $("#modal").modal('hide');
        }
        else {
            $("#nameText").val("");
            $("#ssnText").val("");
            $("#emailText").val("");
            $("#phoneText").val("");
            $("#addressText").val("");
            $("#cityText").val("");
            $("#statesList").val("--State--");
            $("#zipText").val("");
        }
    } else {
        $("#employeeFailureMessage > .error-text").text("The social security number already exists. Please enter a unique social security number.");
        rollDown($("#employeeFailureMessage"));
        setTimeout(function () {
            rollUp($("#employeeFailureMessage"));
        }, 6000);
    }
}

function getInnerViewItem(text) {
    return '<div class="icon-remove delete-item"></div><div class="strip">' + text + '</div>';
}