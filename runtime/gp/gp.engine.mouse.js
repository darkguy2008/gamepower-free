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

var GPMouse = GPProcess.extend({

	_init: function() {
		this.sup();
		this.type = "mouse";
		this._type = "mouse";
	    this.z = -50000;
	    var _sz = 1;

		var tmp = document.createElement("canvas");
		var tmpCx = tmp.getContext('2d');
	    tmp.width = _sz;
	    tmp.height = _sz;

		var imgData=tmpCx.createImageData(_sz,_sz);
		for (var i=0;i<imgData.data.length;i+=4)
		{
			imgData.data[i+0]=255;
			imgData.data[i+1]=255;
			imgData.data[i+2]=255;
			imgData.data[i+3]=255;
		}
		tmpCx.putImageData(imgData, 0, 0);

		this._map = {
			id: -1,
			description: "mouse",
			_img: new Image(),
			_mask: new Image(),
			controlPoints: [],
			cx: 0,
			cy: 0,
			w: _sz,
			h: _sz
		};
		this._map.controlPoints[0] = { x: 0, y: 0 };
		this._map._img.src = tmp.toDataURL();
		this._map._mask.src = tmp.toDataURL();
		tmp = null;
		tmpCx = null;

		var self = this;
		this.SubDraw = function(c)
		{
			if(self.graph != 0)
			{
				c.save();
				c.translate(self.x + ox, self.y + oy);
				c.rotate(-( (self.angle / 1000) * Math.PI / 180));
				c.scale(self.size / 100, self.size / 100);
			 	c.drawImage(self._map._img, this.x, this.y);
			 	c.restore();
		 	}
		 }
		this.SubDrawMask = function(c, ox, oy)
		{
			if(self.graph != 0)
			{
				c.save();
				c.translate(self.x + ox, self.y + oy);
				c.rotate(-( (self.angle / 1000) * Math.PI / 180));
				c.scale(self.size / 100, self.size / 100);
				c.drawImage(self._map._mask, 0, 0);
				c.restore();
			}
			else
			{
				c.save();
				c.translate(self.x + ox, self.y + oy);
				c.drawImage(self._map._mask, 0, 0);
				c.restore();
			}
		}

	},

	Init: function() {
		var self = this;
		self._code = [];
		self._code[0] = function() { self.Frame(); };
		self._code[1] = function() { self._instLocal = 0; };
		_gp.AddProcess(self);
		mouse = self;
		return self;
	},

	Rewire: function() {
		var cv = _gp.CScreen._canvas;
		var scr = _gp.CScreen;
		var cur = this;

		cv.addEventListener('mousemove', function(e) {
			var r = cv.getBoundingClientRect();
			cur.x = (e.clientX - r.left) / scr.options.scale;
			cur.y = (e.clientY - r.top) / scr.options.scale;
		}, true);
		cv.addEventListener('mousedown', function(e) {
			cur.left = e.button == 0;
			cur.right = e.button == 2;
		}, true);
		cv.addEventListener('mouseup', function(e) {
			cur.left = false;
			cur.right = false;
		}, true);
	},

});
