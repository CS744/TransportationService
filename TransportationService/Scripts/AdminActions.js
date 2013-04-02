function addRoute() {
    $.post("/Admin/AddRoute", {}, function (data) {
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

$(document).ready(function () {
    $.post("/Admin/RefreshAdmin", {}, function (data) {
        if (data.error == undefined || data.error != true) {
            userManager.currentUser = data.user;
            $("#mainArea-header").html(data.headerText);
            $("#sign-out-text").removeClass('hidden');
        }
    });
    $(".delete-item").click(function (event) { return deleteItemClick(event, $(this)); });
});

function deleteItemClick(event, elem) {
    var item = elem.parent();
    var type = item.attr("data-type");
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
    $.post("/Utility/GetConfirmationHTML", {}, function (html) {
        $("#modal").replaceWith(html);
        $("#modal").modal();
        $("#confirm-button").click(function (event) {
            $.post("/Admin/" + action, { id: item.attr("data-id") }, function (data) {
                rollUp(item, 300, function () { item.remove(); });
                $("#modal").modal("hide");
                //Additional logic to remove the item from all other views.
            });
        });
    });
    event.preventDefault();
    return false;
}