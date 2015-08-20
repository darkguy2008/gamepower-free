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

var s_kill = 1;
var s_sleep = 2;
var s_freeze = 3;
var s_wakeup = 4;
var s_kill_tree = 11;
var s_sleep_tree = 12;
var s_freeze_tree = 13;
var s_wakeup_tree = 14;

var c_screen = 0;
var c_scroll = 1;
var c_m7 = 2;
var c_m8 = 3;

var c_0 = 0;
var c_1 = 1;
var c_2 = 2;
var c_3 = 3;
var c_4 = 4;
var c_5 = 5;
var c_6 = 6;
var c_7 = 7;
var c_8 = 8;
var c_9 = 9;

// TODO: Implement signal(TYPE <nombre de proceso>, <señal>)
function signal(arg1, val) {
	var dest = arg1;
	if(typeof arg1 === 'number') {
		dest = _gp.aProcess.filter(function(p) { return p.id == arg1; })[0];
	}
	dest._signal = val;
}

function let_me_alone() {
	var a = _gp.aProcess.filter(function(p) { return p.type != "mouse" && p.type != "scroll" && p.id != _gp.callerProcess.id; });
	a.forEach(function(p) {
		signal(p, s_kill);
	});
}