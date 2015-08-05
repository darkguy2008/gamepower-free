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

var GPText = GPProcess.extend({

	_init: function() {
		this.sup();
		this.file = undefined;
		this.text = "";
		this._type = "text";
		this._textOld = "";
		this.z = -128;
		this.center = 0;
		this.SubDraw = this._SubDraw;
		this.canvas = document.createElement("canvas");
		this.ctx = this.canvas.getContext("2d");
		this.idFont = 0;
		this.font = null;
		this.firstRun = true;
	},

	_SubDraw: function(cx) {
		var glyph;
		var self = this;

		if(this.font == null)
			this.font = _gp.res.fnt[_gp._listFNT[this.idFont]];

		var fx = this.x;
		var fy = this.y;
		switch(this.center) {
			case 0:
				break;
			case 1:
				fx -= this.canvas.width / 2;
				break;
			case 2:
				fx -= this.canvas.width;
				break;

			case 3:
				fy -= this.canvas.height / 2;
				break;
			case 4:
				fx -= this.canvas.width / 2;
				fy -= this.canvas.height / 2;
				break;
			case 5:
				fx -= this.canvas.width;
				fy -= this.canvas.height / 2;
				break;

			case 6:
				fy -= this.canvas.height;
				break;
			case 7:
				fx -= this.canvas.width / 2;
				fy -= this.canvas.height;
				break;
			case 8:
				fx -= this.canvas.width;
				fy -= this.canvas.height;
				break;
		}


		if( (this._textOld != this.text) || this.firstRun )
		{
			this.canvas.width = this.GetLength();
			this.canvas.height = this.GetHeight();
			this.ctx.fillStyle = "#F00"; this.ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);

			this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);

			var g;
			var x = 0;
			for (var i = 0, len = this.text.length; i < len; i++) {
				if(self.text.charCodeAt(i) == 32) { x += this.font.w / 2; continue; }
				glyph = self.font.glyphs.filter(function(f) { return f.id == self.text.charCodeAt(i); });
				if(glyph.length > 0) {
					g = glyph[0];
					this.ctx.drawImage(g._img, x, 0 + g.oh);
					x += g.w;
				}
			}

			this._textOld = this.text;
			this.firstRun = false;
		}

		cx.save();
		cx.drawImage(this.canvas, fx, fy);
		cx.restore();
	},

	GetLength: function() {
		var rv = 0;
		var self = this;
		for (var i = 0, len = this.text.length; i < len; i++) {
			if(self.text.charCodeAt(i) == 32) { rv += this.font.w / 2; continue; }
			glyph = self.font.glyphs.filter(function(f) { return f.id == self.text.charCodeAt(i); });
			if(glyph.length > 0)
				rv += glyph[0].w;
		}
		return rv;
	},

	GetHeight: function() {
		return this.font.h;
	}

});
