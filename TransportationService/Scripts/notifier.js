$.notify = {
	type: {
		info: "alert-info alert",
		error: "alert-error alert",
		success: "alert-success alert"
	},
	messagesInQueue: 0,
	defaultOptions:
		{
			type: "info",
			time: 5000,
			id: "",
			class: ""
		},
	// 
	// If the time is -1, then the message is persistent.
	// The user MUST click it to make it disappear.
	// 
	addMessage: function(message, options) {
		options = $.extend({}, this.defaultOptions, options);
		var type = options.type;
		var time = options.time;
		if (!(type in this.type)) {
			type = "info";
		}
		if (time == null || time == undefined || typeof(time) != "number" || (time < 2000 && time != -1)) {
			time = 2000;
		}
		var notifyElem = $("#notify-popover");
		var appendedElem = notifyElem.append("<div class='notify-child hide " + this.type[type] + "'>" + message + "</div>").children(":last-child");
		rollDown(appendedElem, 400);
		this.messagesInQueue++;
		appendedElem.click(function() {
			removeElementFromNotify(appendedElem, notifyElem);
		});
		notifyElem.removeClass("aboveScreen");
		// Removes the item that was just added after three seconds.
		// Different logic will have to be put in place if we want some messages to stay shorter/longer.
		if (time != -1) {
			setTimeout(function() { removeElementFromNotify(appendedElem, notifyElem); }, time);
		}
	}
};
function removeElementFromNotify(appendedElem, notifyElem) {
	// If it has already been removed, don't remove it again.
	if (appendedElem != undefined && !appendedElem.hasClass("removing")) {
		appendedElem.addClass("removing")
		$.notify.messagesInQueue--;
		if ($.notify.messagesInQueue == 0) {
			notifyElem.addClass("aboveScreen");
		}
		rollUp(appendedElem, 400, function() {
			appendedElem.remove();
		});
	}
}