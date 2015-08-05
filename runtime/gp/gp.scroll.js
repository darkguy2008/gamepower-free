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

function start_scroll(scrollId, file, fg, bg, region, block) {

	stop_scroll(scrollId);
	_gp.CScrolls[scrollId] = null;
	var s = new GPScroll();

	s.file = file;
	s.fgGraph = fg;
	s.bgGraph = bg;
	s.idRegion = region;
	s.repeat = block;
	s._code[0] = function() { s.Frame(); };
	s._code[1] = function() { s._instLocal = 0; };
	_gp.AddProcess(s);

	_gp.CScrolls[scrollId] = s;

}

function stop_scroll(scrollId)
{
	signal(_gp.CScrolls[scrollId], s_kill);
}
