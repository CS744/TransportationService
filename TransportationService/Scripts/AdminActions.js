// Simon Weisse


function addRoute() {
    console.log("supposed to be adding route");
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

function addNewStop() {
    var streetName = $("#streetText").val();
    var streetNumber = $("#numberText").val();
    var request = {
        streetName: streetName,
        streetNumber: streetNumber
    }
    $.post("/Admin/AddNewStop", request, function (data) {
        $("#streetText").val("");
        $("#numberText").val("");
    });
}