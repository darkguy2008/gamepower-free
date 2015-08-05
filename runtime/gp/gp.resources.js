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

function load_fpg(filename, id) {
	var rv = _gp._lastFPG;
	if(id == undefined)
		id = rv;
	_gp._listFPG[id] = filename;
	_gp._lastFPG++;
	return rv;
}

function load_fnt(filename) {
	var rv = _gp._lastFNT;
	_gp._listFNT[rv] = filename;
	_gp._lastFNT++;
	return rv;
}

function load_song(filename, loop) {
	var rv = _gp._lastSONG;
	_gp._listSONG[rv] = filename;
	_gp._lastSONG++;
	_gp.res.song[filename].loop = loop;
	return rv;
}

function load_wav(filename, loop) {
	var rv = _gp._lastAUDIO;
	_gp._listAUDIO[rv] = filename;
	_gp._lastAUDIO++;
	_gp.res.audio[filename].loop = loop;
	return rv;
}

function load_pcm(filename, loop) {
	load_wav(filename, loop);
}

function unload_song(id)
{
	// TODO: This should "unload" the song from memory. What to do here though?
}


