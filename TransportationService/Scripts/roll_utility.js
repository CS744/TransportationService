//native replacement for jquery.slideUp and jquery.slideDown

//prerequisite: elem is display:none
function rollDown(elem, duration, callback) {

	if (!$.support.transition) {
		elem.slideDown();
		return;
	}

	if (duration == undefined) {
		duration = 400;
	}
	if (callback == undefined) {
		callback = $.noop;
	}
	var height = elem.height();
	var borderTopWidth = elem.css("borderTopWidth");
	var borderBottomWidth = elem.css("borderBottomWidth");
	var marginTop = elem.css("marginTop");
	var marginBottom = elem.css("marginBottom");
	var paddingTop = elem.css("paddingTop");
	var paddingBottom = elem.css("paddingBottom");

	elem.css("height", 0);
	elem.css("marginTop", 0);
	elem.css("marginBottom", 0);
	elem.css("paddingTop", 0);
	elem.css("paddingBottom", 0);
	elem.css("borderTop", 0);
	elem.css("borderBottom", 0);
	elem.css("overflow", 0);
	elem.css("display", "block");
	elem.css("overflow", "hidden");

	elem.transition(
      {
      	height: height,
      	marginTop: marginTop,
      	marginBottom: marginBottom,
      	paddingTop: paddingTop,
      	paddingBottom: paddingBottom,
      	borderTopWidth: borderTopWidth,
      	borderBottomWidth: borderBottomWidth,
      }, duration, "ease",
      function() {
      	elem.css("height", "");
      	elem.css("marginTop", "");
      	elem.css("marginBottom", "");
      	elem.css("paddingTop", "");
      	elem.css("paddingBottom", "");
      	elem.css("borderTop", "");
      	elem.css("borderBottom", "");
      	elem.css("overflow", "");
      	callback();
      }
   );
}

function rollUp(elem, duration, callback) {

	if (!$.support.transition) {
		elem.slideUp();
		return;
	}

	if (duration == undefined) {
		duration = 400;
	}
	if (callback == undefined) {
		callback = $.noop;
	}
	var startHeight = elem.height();
	elem.css("overflow", "hidden");
	elem.css("height", startHeight);
	elem.css("min-height", 0);

	elem.transition(
      {
      	height: 0,
      	marginTop: 0,
      	marginBottom: 0,
      	paddingTop: 0,
      	paddingBottom: 0,
      	borderTopWidth: 0,
      	borderBottomWidth: 0,
      }, duration, "snap",
      function() {
      	elem.css("display", "none");
      	elem.css("height", "");
      	elem.css("marginTop", "");
      	elem.css("marginBottom", "");
      	elem.css("paddingTop", "");
      	elem.css("paddingBottom", "");
      	elem.css("borderTop", "");
      	elem.css("borderBottom", "");
      	elem.css("overflow", "");
      	elem.css("min-height", "");
      	callback();
      }
   );
}
