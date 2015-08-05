/*
    This file is part of GamePower Engine FREE version.

    GamePower Engine FREE version is distributed in the hope that it
    will be useful, but WITHOUT ANY WARRANTY; without even the implied
    warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
    See the LICENSE file for more details.

    You should have received a copy of the LICENSE file
    along with GamePower Engine FREE version.
    If not, see < http://www.gamepower.com/Info.aspx >.
*/

put_screen = function(fpgId, id) {
	var img = _gp.res.fpg[_gp._listFPG[fpgId]].data.filter(function(m) { return m.id == id })[0]._img;
	_gp.CScreen.options.img = img;
	_gp.CScreen.clear();
}

// Dirty cheating with JQuery...
function fade_on() {
    $(_gp.CScreen._canvas).fadeIn();
}

function fade_off() {
    $(_gp.CScreen._canvas).fadeOut();
}

// TODO: Temp functions. get_pixel must return palette color index, not RGB value.
function rgbToHex(r, g, b) {
    if (r > 255 || g > 255 || b > 255)
        throw "Invalid color component";
    return ((r << 16) | (g << 8) | b).toString(16);
}

function get_pixel(x, y) {
	var pixel = _gp.CScreen._bgCtx.getImageData(x, y, 1, 1).data;
	var hex = ("000000" + rgbToHex(pixel[0], pixel[1], pixel[2])).slice(-6);
	return parseInt(hex,16);
}
