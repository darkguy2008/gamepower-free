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

// TODO: Try this library too -> https://github.com/konsumer/Protracker
var GPSong = Class.extend({

	_init: function() {

		this.codefPlayer = new music();
		this.codefPlayer.stereo(false);
		this.codefPlayer.LoadAndRun = function(e) { this.codefPlayer.Load(e, true); };
		this.codefPlayer.Load = function(e, run, cb)
		{
			var t = this;
			this.loader = window.neoart.FileLoader;
			if (typeof AudioContext != 'undefined') {
				var n = new XMLHttpRequest;
				n.open('GET', e);
				n.overrideMimeType('text/plain; charset=x-user-defined');
				n.responseType = 'arraybuffer';
				n.onreadystatechange = function () {
					if (this.readyState == 4 && (this.status == 200 || this.status == 0)) {
						t.loader.player = null;
						t.loader.load(this.response);
						if(run) {
							t.loader.player.reset();
							t.loader.player.stereo = t.stereo_value;
							t.loader.player.play()
						}
						if(cb)
							cb();
					}
				};
			}
			n.send();
		};

	},

	Load: function(filename, callback) {
		this.codefPlayer.Load(filename, false, function() {
			if(callback)
				callback();
		});
	},

	Play: function(id) {
		var self = this;
		var song = _gp.res.song[_gp._listSONG[id]];

		if(typeof this.codefPlayer.loader != "undefined")
			this.codefPlayer.loader.player.stop();

		this.codefPlayer.Load(_gp._listSONG[id], false, function() {
			self.codefPlayer.loader.player.loopSong = song.loop;
			self.codefPlayer.loader.player.stereo = false;
			self.codefPlayer.loader.player.quality = 0;
			self.codefPlayer.loader.player.play();
		});
	},

	Stop: function() {
		this.codefPlayer.loader.player.stop();
	},

	SetPos: function(pattern) {
		this.codefPlayer.loader.player.order = pattern;
		this.codefPlayer.loader.player.position = 0;
	},

	GetPos: function() {
		return _gp.CSong.codefPlayer.loader.player.order;
	},

	GetLine: function() {
		return _gp.CSong.codefPlayer.loader.player.position;
	},

	IsPlaying: function() {
		return _gp.CSong.codefPlayer.loader.player.node != undefined;
	}

});