function addNewRoute() {
    var routeName = $("#routeNameText").val();
    var license = $("#driverList").val();
    var busText = $("#busList").val();
    var isToWork = true;
    var options = document.getElementById('selectedStops').options;
    var stops = [options.length];
    for (var i = 0; i < options.length; i++)
        stops[i] = options[i].value;
    if ($("#toHomeButton").hasClass('active')) {
        isToWork = false;
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
        busId: parseInt(busText),
        startsAtWork: isToWork
    }
    jQuery.ajaxSettings.traditional = true;
    $.post("/Admin/AddNewRoute", request, function (data) {
        if (data.success == "true") {
            $.notify.addMessage("The route was successfully added!", { type: "success", time: 6000 });
            $("#modal").modal('hide');
            var newItem = $("#view-routes").siblings(".view-child-inner").append("<div class='item-element' onclick=\"viewRoute('" + data.id + "')\">" + getInnerViewItem(routeName) + "</div>").children().last();
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
        $("#stopFailureMessage > .error-text").text("Must Give a Location.");
        rollDown($("#stopFailureMessage"));
        setTimeout(function () {
            rollUp($("#stopFailureMessage"));
        }, 6000);
        return false;
    }
    $.post("/Admin/AddNewStop", request, function (data) { addNewStopCallback(data, !addAnother); });
}
function addNewStopCallback(data, hideModal) {
    if (data.success == "true") {
        var newItem = $("#view-Stops").siblings(".view-child-inner").append("<div class='item-element' onclick=\"viewStop('" + data.id + "')\">" + getInnerViewItem(location) + "</div>").children().last();
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
    if (license.length != 6 || state == "") {
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
        state: state
    }
    $.post("/Admin/AddNewBus", request, function (data) { addNewBusCallback(data, !addAnother); });
}

function addNewBusCallback(data, hideModal) {
    $.post("/Admin/AddNewBus", request, function (data) {
        if (data.success == "true") {
            $.notify.addMessage("The bus was successfully added!", { type: "success", time: 6000 });
            var newItem = $("#view-buses").siblings(".view-child-inner").append("<div class='item-element' onclick=\"viewBus('" + data.id + "')\">" + getInnerViewItem(license) + "</div>").children().last();
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
    });
}

function addNewDriver(addAnother) {
    var state = $("#statesList").val();
    var name = $("#nameText").val();
    var license = $("#licenseText").val().toUpperCase();
    if (state == "" || name == "" || license == "") {
        $("#driverFailureMessage > .error-text").text("Must fill out ALL information.");
        rollDown($("#employeeFailureMessage"));
        setTimeout(function () {
            rollUp($("#employeeFailureMessage"));
        }, 6000);
        return false;
    }
    var request = {
        state: state,
        name: name,
        license: license
    }
    $.post("/Admin/AddNewDriver", request, function (data) { addNewDriverCallback(data, !addAnother); });
}
function addNewDriverCallback(data, hideModal) {
    $.post("/Admin/AddNewDriver", request, function (data) {
        if (data.success == "true") {
            var newItem = $("#view-Drivers").siblings(".view-child-inner").append("<div class='item-element' onclick=\"viewDriver('" + data.id + "')\">" + getInnerViewItem(name) + "</div>").children().last();
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
    });
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

    if (email == "" || phone == "" || address == "" || ssn == "" || position == "" || name == "" || routeId == null || city == "" || state == "" || state == "--State--") {
        $("#employeeFailureMessage > .error-text").text("Must fill out ALL information.");
        rollDown($("#employeeFailureMessage"));
        setTimeout(function () {
            rollUp($("#employeeFailureMessage"));
        }, 6000);
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
        name: name
    }
    $.post("/Admin/AddNewDriver", request, function (data) { addNewDriverCallback(data, !addAnother); });
}

function addNewEmployeeCallback(data, hideModal) {
    $.post("/Admin/AddNewEmployee", request, function (data) {
        if (data == "true") {
            var newItem = $("#view-Employees").siblings(".view-child-inner").append("<div class='item-element' onclick=\"viewEmployee('" + data.id + "')\">" + getInnerViewItem(name) + "</div>").children().last();
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
            }
        } else {
            $("#employeeFailureMessage > .error-text").text("The social security number already exists. Please enter a unique social security number.");
            rollDown($("#employeeFailureMessage"));
            setTimeout(function () {
                rollUp($("#employeeFailureMessage"));
            }, 6000);
        }
    });
}

function getInnerViewItem(text) {
    return '<div class="icon-remove delete-item"></div><div class="strip">' + text + '</div>';
}