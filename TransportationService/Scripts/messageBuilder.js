var MessageBuilder = function () {
    var reasons = [];
    this.addMessage = function (r) {
        if (r != "")
            reasons.push(r);
    }
    this.getMessage = function (preText) {
        if (reasons.length == 0) {
            return "";
        }
        if (reasons.length == 2) {
            return preText + " " + reasons[0] + " and " + reasons[1] + ".";
        }
        var message = "";
        var lastMessage = reasons.pop(reasons.length - 1);
        for (var ndx in reasons) {
            var reason = reasons[ndx];
            message = message + reason + ", ";
        }
        if (reasons.length >= 1) {
            message = message + "and ";
        }
        message = message + lastMessage;
        reasons.push(lastMessage);
        return preText + " " + message + ".";
    }
    this.IsEmpty = function () {
        return reasons.length == 0;
    }
}