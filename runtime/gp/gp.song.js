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

function song(id) {
	_gp.CSong.Play(id);
}

function stop_song() {
	_gp.CSong.Stop();
}

function set_song_pos(pattern)
{
	_gp.CSong.SetPos(pattern);
}

function get_song_pos(pattern)
{
	return _gp.CSong.GetPos();
}

function get_song_line(pattern)
{
	return _gp.CSong.GetLine();
}

function is_playing_song() {
	return _gp.CSong.IsPlaying();
}
