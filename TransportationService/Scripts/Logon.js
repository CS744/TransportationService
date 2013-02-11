// Simon Weisse


function logIn() {
    var username = $("#usernameText").val();
    var password = $("#passwordText").val();
    var request = {
        username: username,
        password: password
    }
    $.post("/Home/LogIn", request, function (data) {
        userManager.currentUser = data.user;
        $("#mainArea").replaceWith(data.html);
    });
}