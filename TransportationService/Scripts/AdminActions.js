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
            $("#view-routes").siblings(".view-child-inner").append("<div class='item-element' onclick=\"viewRoute('" + data.id + "')\">" + routeName + "</div>");
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
    $.post("/Admin/ModifyRoute", {routeId: rId}, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}

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
        if (data == "true") {
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
    if (location == "") {
        $("#stopFailureMessage > .error-text").text("Must Give a Location.");
        rollDown($("#stopFailureMessage"));
        setTimeout(function () {
            rollUp($("#stopFailureMessage"));
        }, 6000);
        return false;
    }
    if (addAnother) {
        $.post("/Admin/AddNewStop", request, function (data) {
            if (data == "true") {
                $("#locationText").val("");
                $.notify.addMessage("The stop was successfully added!", { type: "success", time: 6000 });
            } else {
                $("#stopFailureMessage > .error-text").text("The stop location already exists. Please enter a unique stop location.");
                rollDown($("#stopFailureMessage"));
                setTimeout(function () {
                    rollUp($("#stopFailureMessage"));
                }, 6000);
            }
        });
    } else {
        $.post("/Admin/AddNewStop", request, function (data) {
            if (data == "true") {
                $.notify.addMessage("The stop was successfully added!", { type: "success", time: 6000 });
                $("#modal").modal('hide');
            } else {
                $("#stopFailureMessage > .error-text").text("The stop location already exists. Please enter a unique stop location.");
                rollDown($("#stopFailureMessage"));
                setTimeout(function () {
                    rollUp($("#stopFailureMessage"));
                }, 6000);
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

function removeStopFromRoute(elem) {
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
    var state = $("#statesList").val();
    var capacity = $("#capacityText").val();
    var license = $("#licenseText").val();
    if (license.length != 7 || state == "") {
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
    if (addAnother) {
        $.post("/Admin/AddNewBus", request, function (data) {
            if (data.success == "true") {
                $("#capacityText").val("");
                $("#licenseText").val("");
                $("#statesList").val("--State--");
                $.notify.addMessage("The bus was successfully added!", { type: "success", time: 6000 });
                $("#view-buses").siblings(".view-child-inner").append("<div class='item-element' onclick=\"viewBus('" + data.id + "')\">" + license + "</div>");
            } else {
                $("#busFailureMessage > .error-text").text("The license plate already exists. Please enter a unique license plate.");
                rollDown($("#busFailureMessage"));
                setTimeout(function () {
                    rollUp($("#busFailureMessage"));
                }, 6000);
            }
        });
    } else {
        $.post("/Admin/AddNewBus", request, function (data) {
            if (data.success == "true") {
                $.notify.addMessage("The bus was successfully added!", { type: "success", time: 6000 });
                $("#view-buses").siblings(".view-child-inner").append("<div class='item-element' onclick=\"viewBus('" + data.id + "')\">" + license + "</div>");
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
}

function modifyBus(bId) {
    console.log("supposed to be modifying bus");
    $.post("/Admin/ModifyBus", {busId: bId}, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
            });
}

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

function addDriver() {
    console.log("supposed to be adding driver");
    $.post("/Admin/AddDriver", {}, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}

function addNewDriver(addAnother) {
    var state = $("#statesList").val();
    var name = $("#nameText").val();
    var license = $("#licenseText").val();
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
    if (addAnother) {
        $.post("/Admin/AddNewDriver", request, function (data) {
            if (data == "true") {
                $("#nameText").val("");
                $("#licenseText").val("");
                $("#statesList").val("--State--");
                $.notify.addMessage("The driver was successfully added!", { type: "success", time: 6000 });
            } else {
                $("#driverFailureMessage > .error-text").text("The driver's license already exists. Please enter a unique license.");
                rollDown($("#driverFailureMessage"));
                setTimeout(function () {
                    rollUp($("#driverFailureMessage"));
                }, 6000);
            }
        });
    } else {
        $.post("/Admin/AddNewDriver", request, function (data) {
            if (data == "true") {
                $.notify.addMessage("The driver was successfully added!", { type: "success", time: 6000 });
                $("#modal").modal('hide');
            } else {
                $("#driverFailureMessage > .error-text").text("The driver's license already exists. Please enter a unique license.");
                rollDown($("#driverFailureMessage"));
                setTimeout(function () {
                    rollUp($("#driverFailureMessage"));
                }, 6000);
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
    var isMale = $("#isMale").hasClass("active");
    var email = $("#emailText").val();
    var phone = $("#phoneText").val();
    var address = $("#addressText").val();
    var city = $("#cityText").val();
    var state = $("#statesList").val();
    var routeId = $("#routeList").val();
    var ssn = $("#ssnText").val();
    var position = $("#positionText").val();
    var name = $("#nameText").val();

    if (email == "" || phone == "" || address == "" || ssn == "" || position == "" || name == "" || routeId == null) {
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
    if (addAnother) {
        $.post("/Admin/AddNewEmployee", request, function (data) {
            if (data == "true") {
                $("#ssnText").val("");
                $("#positionText").val("");
                $("#nameText").val("");
                $.notify.addMessage("The employee was successfully added!", { type: "success", time: 6000 });
            } else {
                $("#employeeFailureMessage > .error-text").text("The social security number already exists. Please enter a unique social security number.");
                rollDown($("#employeeFailureMessage"));
                setTimeout(function () {
                    rollUp($("#employeeFailureMessage"));
                }, 6000);
            }
        });
    } else {
        $.post("/Admin/AddNewEmployee", request, function (data) {
            if (data == "true") {
                $.notify.addMessage("The employee was successfully added!", { type: "success", time: 6000 });
                $("#modal").modal('hide');
            } else {
                $("#employeeFailureMessage > .error-text").text("The social security number already exists. Please enter a unique social security number.");
                rollDown($("#employeeFailureMessage"));
                setTimeout(function () {
                    rollUp($("#employeeFailureMessage"));
                }, 6000);
            }
        });
    }
}

function viewMe(id) {
    elem = $(id);
    if (elem.hasClass("viewing")) {
        elem.removeClass("viewing")
        rollUp(elem.siblings(".view-child-inner"));
    }
    else {
        $(".view-child-header.viewing").each(function (ndx) {
            rollUp($(this).siblings(".view-child-inner"));
            $(this).removeClass("viewing");
        });
        elem.addClass("viewing");
        rollDown(elem.siblings(".view-child-inner"));
    }
}

function viewBus(id) {
    var request = { id: id };
    $.post("/ViewInformation/ViewBus", request, function (data) {
        $("#view-container > .view-container-right").html(data);
    });
}

function viewRoute(id) {
    var request = { id: id };
    $.post("/ViewInformation/ViewRoute", request, function (data) {
        $("#view-container > .view-container-right").html(data.html);
    });
}

function moveStopUp() {
    var selected = $("#selectedStops option:selected");
    var text = selected.text();
    var index = $("#selectedStops option").index(selected);
    if (index != 0) {
        selected.remove();
        $("#selectedStops option:eq(" + (index - 1) + ")").before("<option value='1'>" + text + "</option>");
        $('#selectedStops option')[index - 1].selected = true;
    }
    $('#selectedStops').focus();
}

function moveStopDown() {
    var selected = $("#selectedStops option:selected");
    var text = selected.text();
    var index = $("#selectedStops option").index(selected);
    var size = $("#selectedStops option").size();
    if (index != size - 1) {
        selected.remove();
        $("#selectedStops option:eq(" + index + ")").after("<option value='1'>" + text + "</option>");
        $('#selectedStops option')[index + 1].selected = true;
    }
    $('#selectedStops').focus();
}

function removeStop() {
    var selected = $("#selectedStops option:selected");
    var stopId = selected.val();
    var location = selected.text();
    var index = selected.index();
    $("#stops").append("<option value=\"" + stopId + "\">" + location + "</option>");
    var mylist = $('#stops');
    var listitems = mylist.children('option').get();
    listitems.sort(function (a, b) {
        var first = parseInt($(a).val());
        var second = parseInt($(b).val());
        if (first == second)
            return 0;
        else if (first > second)
            return 1;
        else
            return -1;
    })
    $.each(listitems, function (idx, itm) { mylist.append(itm); });
    var size = $("#selectedStops option").size();
    $("#selectedStops option:selected").remove();
    if (index == size - 1 && size > 1)
        index--;
    $('#selectedStops option')[index].selected = true;
    $('#selectedStops').focus();
}

function addStopToRoute() {
    var stops = document.getElementById('selectedStops');
    var stopId = document.getElementById('stops').value;
    var location = $("#stops option:selected").text();
    stops.options[stops.options.length] = new Option(location, stopId);
    var select = document.getElementById('stops');
    select.remove(select.selectedIndex);
    document.getElementById('stops').focus();
}