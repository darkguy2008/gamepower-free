var m320x200 = [320, 200];
var m320x240 = [320, 240];
var m320x400 = [320, 400];
var m360x240 = [360, 240];
var m360x360 = [360, 360];
var m376x282 = [376, 282];
var m640x400 = [640, 400];
var m640x480 = [640, 480];
var m800x600 = [800, 600];
var m1024x768 = [1024, 768];
var m1400x700 = [1400, 700];

function set_mode(mode) {
	_gp.CScreen.options.size = mode;
	_gp.CScreen.recreate();
	_gp.CScreen.clear();
}

function set_fps() {}
