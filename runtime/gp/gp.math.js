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

function xadvance(p, angle, dist) {
    p.x += dist * Math.cos(-(angle / 1000) * Math.PI / 180);
    p.y += dist * Math.sin(-(angle / 1000) * Math.PI / 180);
}

function rand(min, max) {
	return Math.floor(Math.random() * (max - min + 1)) + min;
}

function abs(value) {
	return Math.abs(value);
}

function pow(val, by)
{
    return Math.pow(val, by);
}

function sqrt(val)
{
    return Math.sqrt(val);
}

// https://gist.github.com/conorbuck/2606166
function fget_angle(x1, y1, x2, y2)
{
    return Math.atan2(y2 - y1, x2 - x1) * 180 / Math.PI;
}

function fget_dist(x1, y1, x2, y2)
{
    return Math.sqrt( (x2-=x1)*x2 + (y2-=y1)*y2 );
}

// TODO: Set caller in GPCompiler
function get_angle(caller, idP)
{
    var dest = idP;
    if(typeof idP === 'number') {
        dest = _gp.aProcess.filter(function(p) { return p.id == idP; })[0];
    }
    return fget_angle(caller.x, caller.y, dest.x, dest.y);
}

// TODO: Set caller in GPCompiler
function get_dist(caller, idP)
{
    var dest = idP;
    if(typeof idP === 'number') {
        dest = _gp.aProcess.filter(function(p) { return p.id == idP; })[0];
    }
    return fget_dist(caller.x, caller.y, dest.x, dest.y);
}

function rand(min, max) {
    if(!max) {
        max = min;
        min = 0;
    }
    return Math.floor(Math.random()*(max-min+1)+min);
}

function rand_seed(seed)
{
    // TODO: Change random number generator with:
    // http://davidbau.com/archives/2010/01/30/random_seeds_coded_hints_and_quintillions.html#more
}
