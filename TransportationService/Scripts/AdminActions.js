function addRoute(isToWork) {
    $.post("/Admin/AddRoute", { isToWork: isToWork }, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}

function addStop() {
    console.log("supposed to be adding stop");
    $.post("/Admin/AddStop", {}, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
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

function addDriver() {
    console.log("supposed to be adding driver");
    $.post("/Admin/AddDriver", {}, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}

function addEmployee() {
    console.log("supposed to be adding employee");
    $.post("/Admin/AddEmployee", {}, function (data) {
        $("#modal").replaceWith(data);
        $("#modal").modal();
    });
}

function moveStopUp() {
    var selected = $("#selectedStops option:selected");
    var text = selected.text();
    var value = selected.val();
    var index = $("#selectedStops option").index(selected);
    if (index != 0) {
        selected.remove();
        $("#selectedStops option:eq(" + (index - 1) + ")").before("<option value='" + value + "'>" + text + "</option>");
        $('#selectedStops option')[index - 1].selected = true;
    }
    $('#selectedStops').focus();
}

function moveStopDown() {
    var selected = $("#selectedStops option:selected");
    var text = selected.text();
    var value = selected.val();
    var index = $("#selectedStops option").index(selected);
    var size = $("#selectedStops option").size();
    if (index != size - 1) {
        selected.remove();
        $("#selectedStops option:eq(" + index + ")").after("<option value='" + value + "'>" + text + "</option>");
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
    customSort("#stops");
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

$(document).ready(function () {
    if (window.location.pathname == "/EmployeeManagement") {
        document.getElementById("mainArea-header").innerHTML = "Employee Management";
        return false;
    }
    $.post("/Admin/RefreshAdmin", {}, function (data) {
        if (data.error == undefined || data.error != true) {
            userManager.currentUser = data.user;
            $("#mainArea-header").html(data.headerText);
            $("#sign-out-text").removeClass('hidden');
        }
    });
    $(".delete-item").click(function (event) { return deleteItemClick(event, $(this)); });
});

function deleteItemClick(type, id, event) {
    var action = "";
    if (type == "route") {
        action = "DeleteRoute";
    }
    else if (type == "stop") {
        action = "DeleteStop";
    }
    else if (type == "bus") {
        action = "DeleteBus";
    }
    else if (type == "driver") {
        action = "DeleteDriver";
    }
    else if (type == "employee") {
        action = "DeleteEmployee";
    }
    else {
        alert("ERROR HAS OCCURED");
        return false;
    }
    var callback = function (d) {
        if (d.isValid) {
            $.post("/Utility/GetConfirmationHTML", {}, function (html) {
                $("#modal").replaceWith(html);
                $("#modal").modal();
                $("#confirm-button").click(function (e) {
                    $.post("/Admin/" + action, { id: id }, function (data) {
                        $.notify.addMessage("Successfully deleted!", { type: "success", time: 6000 });
                        var sel = "removeItemById('" + id + "')";
                        if ($("button[onclick='" + sel + "']")) {
                            $(".view-container-right").html("");
                        }
                        var item = $("#view-tr-" + id);
                        rollUp(item, 300, function () { item.remove(); });
                        $("#modal").modal("hide");
                    });
                });
            });
        } else {
            var jqueryString = "$('#modal').modal('hide')";
            $("#modal").replaceWith('<div id="modal" class="modal hide" style="margin-left: -125px; width: 250px;"><div class="modal-header">Cannot Delete</div><div class="modal-body" style="text-align: center;">' + d.message + '</div><div class="modal-footer" style="padding:5px;"><button class="btn" onclick="' + jqueryString + '">OK</button></div></div>');
            $("#modal").modal();
        }
    }
    $.post("/Utility/IsValid" + action, { id: id }, function (d) {
        callback(d);
    });
    if (event) {
        event.stopPropagation();
        event.preventDefault();
    }
    return false;
}

//"driverBus" is a stupid & terrible name but I couldn't think of anything better and "busDriver" just sounds like a driver
function addDriverBus() {
    var driverId = document.getElementById('driverList').value;
    var busId = document.getElementById('busList').value;
    var driverDetail = $("#driverList option:selected").text();
    var busDetail = $("#busList option:selected").text();
    if (driverId == "None")
        driverDetail = "Driver: None";
    if (busId == "None")
        busDetail = "Bus: None";

    var departureTime = $("#hourList option:selected").text() + ":" + $("#minuteList option:selected").text() + " " + $("#ampmLabel").text();
    var status = $("#driverBusActiveButton").hasClass('active') ? "ACTIVE" : "INACTIVE";
    if (driverId == "None" && busId == "None")
        status = "INACTIVE";

    var value = busId + ";" + driverId + ";" + departureTime + ";" + status;
    var text = busDetail + "; " + driverDetail + "; Departs: " + departureTime + "; " + status;
    var driverBusList = document.getElementById('driverBusList');
    driverBusList.options[driverBusList.options.length] = new Option(text, value);
    if (driverId != "None") {
        var driverList = document.getElementById('driverList');
        driverList.remove(driverList.selectedIndex);
    }
    if (busId != "None") {
        var busList = document.getElementById('busList');
        busList.remove(busList.selectedIndex);
    }
    //reset fields
    document.getElementById("hourList").options[0].selected = true;
    document.getElementById("minuteList").options[0].selected = true;
    $("#driverBusActiveButton").removeClass('active');
    $("#driverBusInactiveButton").addClass('active');
}

function removeDriverBus(editEntry) {
    var selected = $("#driverBusList option:selected");
    if (selected == null)
        return;
    var value = selected.val();
    var text = selected.text();
    var index = selected.index();

    var valueArray = value.split(";");
    var busValue = valueArray[0];
    var driverValue = valueArray[1];

    var textArray = text.split(";");
    var busText = textArray[0];
    var driverText = textArray[1];



    if (editEntry) {
        var departureTimeValue = valueArray[2];
        var status = valueArray[3];
        var timeArray = departureTimeValue.split(":");
        var hourValue = parseInt(timeArray[0]);
        timeArray = timeArray[1].split(" ");
        var minuteValue = parseInt(timeArray[0]);
        var hourIndex = 0;
        var minuteIndex = 0;
        if (hourValue == 7 || hourValue == 4)
            hourIndex = 1;
        else if (hourValue == 8 || hourValue == 5)
            hourIndex = 2;
        else if (hourValue == 9 || hourValue == 6)
            hourIndex = 3;
        var counter = 0;
        for (var i = 0; i < 12; i++) {
            if (counter == minuteValue) {
                minuteIndex = i;
                break;
            }
            counter += 5;
        }

        //add bus and driver fields back into their lists
        if (busValue != "None")
            $("#busList").append("<option value=\"" + busValue + "\" selected>" + busText + "</option>");
        else
            $("#busList")[0].selectedIndex = 0
        if (driverValue != "None")
            $("#driverList").append("<option value=\"" + driverValue + "\" selected>" + driverText + "</option>");
        else
            $("#driverList")[0].selectedIndex = 0
        customSort("#busList");
        customSort("#driverList");
        if (status == "ACTIVE") {
            $("#driverBusInactiveButton").removeClass('active');
            $("#driverBusActiveButton").addClass('active');
        } else {
            $("#driverBusActiveButton").removeClass('active');
            $("#driverBusInactiveButton").addClass('active');
        }
        document.getElementById("hourList").options[hourIndex].selected = true;
        document.getElementById("minuteList").options[minuteIndex].selected = true;
    } else {
        //this code is in two places for a reason - leave it alone
        //add bus and driver fields back into their lists
        if (busValue != "None")
            $("#busList").append("<option value=\"" + busValue + "\">" + busText + "</option>");
        if (driverValue != "None")
            $("#driverList").append("<option value=\"" + driverValue + "\">" + driverText + "</option>");
        customSort("#busList");
        customSort("#driverList");
    }

    var size = $("#driverBusList option").size();
    $("#driverBusList option:selected").remove();
    if (index == size - 1 && size > 1) {
        index--;
        $('#driverBusList option')[index].selected = true;
    }
    $('#driverBusList').focus();
}

function customSort(list) {//include the # in the name of the list
    var mylist = $(list);
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
}

function viewRoutes() {
    $.post("/Admin/ViewRoutes", {}, function (html) {
        $("#view-container").html(html);
        $("#view-item-table").tablesorter();
        $("#view-item-table tr").addClass("clickable");
        $("#view-item-table tr").attr("title", "View Route Information");
        $("#view-item-table tr").click(function (event) {
            viewRouteInformation(this.dataset.id);
        });
        $("#view-item-table").find("tbody > tr").css("cursor", "pointer");
    });
}

function viewEmployeesTable() {
    $.post("/Admin/ViewEmployees", {}, function (html) {
        $("#view-container").html(html);
        $("#view-item-table").tablesorter();
    });
}

function viewBuses() {
    $.post("/Admin/ViewBuses", {}, function (html) {
        $("#view-container").html(html);
        $("#view-item-table").tablesorter();
    });
}

function viewDrivers() {
    $.post("/Admin/viewDrivers", {}, function (html) {
        $("#view-container").html(html);
        $("#view-item-table").tablesorter();
    });
}

function viewStops() {
    $.post("/Admin/viewStops", {}, function (html) {
        $("#view-container").html(html);
        $("#view-item-table").tablesorter();
    });
}

function viewSystemUsage() {
    $.post("/Admin/ViewSystemUsage", {}, function (html) {
        $("#view-container").html(html);
        $("#system-usage-table").tablesorter();
        calculateActivityAmounts();
    });
}