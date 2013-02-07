// Simon Weisse


function pressedRegister() {
    var username = $("#usernameText").val();
    var password = $("#passwordText").val();
    var request = {
        username: username,
        password: password
    }
    $.post("/Home/RegisterUser", request, function(data){
        userManager.currentUser = data.user;
        $("#mainArea").replaceWith(data.html);
        $("#modal").modal("hide");
    });
}


$(document).ready(function () {
    $("#buttonToRegister").click(function () {
        $.post("/Utility/GetRegisterModal", {}, function (data) {
            console.log("something");
            $("#modal").replaceWith(data);
            $("#modal").modal();
        });
    });
});