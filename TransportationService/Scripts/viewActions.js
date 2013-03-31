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

function viewStop(id) {
    var request = { id: id };
    $.post("/ViewInformation/ViewStop", request, function (data) {
        $("#view-container > .view-container-right").html(data.html);
    });
}

function viewDriver(id) {
    var request = { driverId: id };
    $.post("/ViewInformation/ViewDriver", request, function (data) {
        $("#view-container > .view-container-right").html(data.html);
    });
}

function viewEmployee(id) {
    var request = { employeeId: id };
    $.post("/ViewInformation/ViewEmployee", request, function (data) {
        $("#view-container > .view-container-right").html(data.html);
    });
}