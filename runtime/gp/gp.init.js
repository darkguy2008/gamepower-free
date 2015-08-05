var m320x200 = [320, 200];
var m320x240 = [320, 240];
var m640x480 = [640, 480];

function set_mode(mode) {
	_gp.CScreen.options.size = mode;
	_gp.CScreen.recreate();
	_gp.CScreen.clear();
}

function set_fps() {}
