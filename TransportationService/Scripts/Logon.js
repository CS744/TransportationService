// Simon Weisse


function logIn() {
    var username = $("#usernameText").val();
    var password = $("#passwordText").val();
    var request = {
        username: username,
        password: password
    }
    $.post("/Home/LogIn", request, function (data) {
        if (data.error) {
            var loginError = $("#login-error");
            loginError.addClass("showerror");
            loginError.html(data.message);
            setTimeout(function () {
                loginError.removeClass("showerror");
            }, 10000);
        }
        else {
            userManager.currentUser = data.user;
            $("#mainArea-header").html(data.headerText);
            $("#mainArea").replaceWith(data.html);
            $("#sign-out-text").removeClass('hidden');
        }
    });
}
$(document).ready(function () {
    $("#sign-out-text").click(function () {
        if (!this.classList.contains("hidden")) {
            $.post("/Home/LogOut", {}, function (data) {
                $("#mainArea").html(data);
                $("#sign-out-text").addClass('hidden');
                userManager.currentUser = undefined;
                $("#mainArea-header").html("Welcome to Transportation Service");
            });
        }
    });
});
