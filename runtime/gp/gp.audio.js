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

// TODO: Frequency (pitch) is not used in howler.js. Anybody up for implementing an audio pitch feature? ;)
// TODO: Implement audio channels
function sound(id, volume, freq) {
	var audio = _gp.res.audio[_gp._listAUDIO[id]];
	audio.data.volume = volume / 256;
	audio.data.loop = audio.loop;
	audio.data.play();
	return id;
}
