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

// TODO:
// Try http://lowlag.alienbill.com/, need to implement looping and events though. What about its license?

var GPAudio = Class.extend({

	_init: function() {

	},

	Load: function(filename, callback)
	{
		var rv = new Howl({
			urls: [filename], // TODO: Add mp3 if ogg specified, add ogg if mp3 specified, etc.
			autoplay: false,
			loop: false,
			volume: 0.5,
			onload: function() {
				if(callback)
					callback(this);
			}
		})
	}

});
