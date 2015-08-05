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

function advance(p, dist) {
	p.x += dist * Math.cos(-(p.angle / 1000) * Math.PI / 180);
	p.y += dist * Math.sin(-(p.angle / 1000) * Math.PI / 180);
}

function xadvance(p, dist, angle) {
    p.x += dist * Math.cos(-(angle / 1000) * Math.PI / 180);
    p.y += dist * Math.sin(-(angle / 1000) * Math.PI / 180);
}

function rand(min, max) {
	return Math.floor(Math.random() * (max - min + 1)) + min;
}

function abs(value) {
	return Math.abs(value);
}