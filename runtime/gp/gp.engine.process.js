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

var GPProcess = Class.extend({

	_init: function() {
		this._code = [];
		this._alive = 1;
		this._instLocal = 0;
		this._over100 = false;
		this._framePercent = 0;
		this._signal = 0;
		this._drawn = false;
		this.SubDraw = undefined;
		this.SubDrawMask = undefined;
		this.SubUpdate = undefined;
		this._type = "process";
		this._altMask = undefined;

		this._map = null;
		this.id = -1;

		if(!(typeof _gp === "undefined"))
			this.father = _gp.parentProcess;

		this.x = 0;
		this.y = 0;
		this.z = 0;
		this.file = 0;
		this.graph = 0;
		this.type = null;
		this.size = 100;
		this.angle = 0;
		this.priority = 0;
		this.resolution = 0;
		this.son = null;
		this.ctype = 0;
		this.cnumber = 0;

		this._oldGraph = 0;
	},

	Update: function() {
		if(this.SubUpdate != undefined) {
			this.SubUpdate();
			return;
		}
		//this.x = Math.round(this.x); this.y = Math.round(this.y);
	},

	Frame: function(wait) {
		this._framePercent += wait || 100;
		this._over100 = this._framePercent >= 100;
	},

	Draw: function(ctx) {
		if(this.SubDraw != undefined) {
			this.SubDraw(_gp.CScreen._ctx);
			return;
		}
		if(this.file != undefined) {
			if(this.graph > 0)
			{
				var self = this;
				if(self._oldGraph != self.graph)
				{
					var map = _gp.res.fpg[_gp._listFPG[self.file]].data.filter(function(m) { return m.id == self.graph })[0];
					self._map = map;
					self._oldGraph = self.graph;
				}

				var s = _gp.CScreen;

				var dx = self.x / (self.resolution == 0 ? 1 : self.resolution);
				var dy = self.y / (self.resolution == 0 ? 1 : self.resolution);

				if(self.ctype == c_scroll)
				{
					var scr = scroll[self.cnumber];
					dx = -scr.x0 + self.x / (self.resolution == 0 ? 1 : self.resolution);
					dy = -scr.y0 + self.y / (self.resolution == 0 ? 1 : self.resolution);
				}

				// See: http://jsperf.com/drawimage-vs-canvaspattern/9

				s._ctx.save();
				s._ctx.translate(dx, dy);
				if(self.angle != 0)
					s._ctx.rotate(-(self.angle / 1000) * Math.PI / 180);
				if(self.size != 100)
					s._ctx.scale(self.size / 100, this.size / 100);
				s._ctx.drawImage(self._map._img, -self._map.cx, -self._map.cy);
				s._ctx.restore();
			}
		}
	},

	DrawMask: function(cx, ox, oy) {
		if(this.SubDrawMask != undefined) {
			this.SubDrawMask(cx, ox, oy);
			return;
		}
		if(this.file != undefined) {
			if(this.graph > 0) {
				var self = this;
				var mask = this._altMask === undefined ? self._map._mask : this._altMask;

				cx.save();
				cx.translate(this.x + ox, this.y + oy);
				if(self.angle != 0)
					cx.rotate(-( (this.angle / 1000) * Math.PI / 180));
				if(self.size != 100)
					cx.scale(this.size / 100, this.size / 100);
				cx.drawImage(mask, -self._map.cx, -self._map.cy);
				cx.restore();
			}
		}
	},

});
