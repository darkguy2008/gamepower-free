var m320x200 = [320, 200];
var m320x240 = [320, 240];
var m640x480 = [640, 480];
var m1400x700 = [1400, 700];

function set_mode(mode) {
	_gp.CScreen.options.size = mode;
	_gp.CScreen.recreate();
	_gp.CScreen.clear();
}

function set_fps() {}
