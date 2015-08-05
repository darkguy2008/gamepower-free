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

function out_region(idP, idR) {
	var r = _gp.CRegions[idR];
	var p = _gp.aProcess.filter(function(e) { return e.id == idP; })[0];
	return (
		p.x < r.x ||
		p.y < r.y ||
		p.x > r.x + r.w ||
		p.y > r.y + r.h
	);
}