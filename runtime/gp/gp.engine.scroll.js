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

var GPScroll = GPProcess.extend({

	// TODO: Works good, needs cleaning up
	_init: function() {
		this.sup();

		this._type = "scroll";

		this.id = 0;
		this.file = undefined;
		this.fgGraph = 0;
		this.bgGraph = 0;
		this.idRegion = 0;
		this.region = null;
		this.repeat = 0;
		// if graph.size < region.size then repeat = all
		this.x0 = 0;
		this.y0 = 0;
		this.x1 = 0;
		this.y1 = 0;
		this.z = 512;
		this.idCamera = 0;
		this.camera = null;
		this.ratio = 0;
		this.speed = 0;
		this.idRegion1 = -1;
		this.idRegion2 = -1;

		this.SubDraw = this._SubDraw;

		this._oldFgGraph = 0;
		this._oldBgGraph = 0;
		this._maxSize = [];
		this._eScrollCycle = {
			fgX: 1,
			fgY: 2,
			bgX: 4,
			bgY: 8
		}

		this.ox = 0;
		this.oy = 0;
	},

	_SubDraw: function(cx) {
		var self = this;

		var fgMap = _gp.res.fpg[_gp._listFPG[this.file]].data.filter(function(m) { return m.id == self.fgGraph })[0];
		var bgMap = _gp.res.fpg[_gp._listFPG[this.file]].data.filter(function(m) { return m.id == self.bgGraph })[0];
		var sw = _gp.CRegions[0].w;
		var sh = _gp.CRegions[0].h;

		if(this.fgGraph != this._oldFgGraph || this.bgGraph != this._oldBgGraph)
		{
			this._maxSize[0] = fgMap.w > bgMap.w ? fgMap.w : bgMap.w;
			this._maxSize[1] = fgMap.h > bgMap.h ? fgMap.h : bgMap.h;
			this._oldFgGraph = this.fgGraph;
			this._oldBgGraph = this.bgGraph;
		}

		// TODO: Redo flag system?
		if(fgMap.w < sw)
			setFlag(this.repeat, this._eScrollCycle.fgX);
		if(fgMap.h < sh)
			setFlag(this.repeat, this._eScrollCycle.fgY);
		if(bgMap.w < sw)
			setFlag(this.repeat, this._eScrollCycle.bgX);
		if(bgMap.h < sh)
			setFlag(this.repeat, this._eScrollCycle.bgY);

		var cam = _gp.GetProcess(this.camera);
		if(cam != null)
		{
			this.x0 = cam.x - sw / 2;
			this.y0 = cam.y - sh / 2;
		}

		// TODO: Blocking algorithm doesn't work very well, TODO handling of manual scrolling method
		if(!hasFlag(this.repeat, this._eScrollCycle.fgX) && !hasFlag(this.repeat, this._eScrollCycle.bgX))
		{
			if(this.x0 < 0)
				this.x0 = 0;
			if(this.x0 > this._maxSize[0] - sw)
				this.x0 = this._maxSize[0] - sw;
		}
		if(!hasFlag(this.repeat, this._eScrollCycle.fgY) && !hasFlag(this.repeat, this._eScrollCycle.bgY))
		{
			if(this.y0 < 0)
				this.y0 = 0;
			if(this.y0 > this._maxSize[1] - sh)
				this.y0 = this._maxSize[1] - sh;
		}

		if(cam != null)
		{
			this.x1 = this.x0 / 2;
			this.y1 = this.y0 / 2;
		}

		this.x = this.x0;
		this.y = this.y0;

		var dx = -this.x1;
		var dy = -this.y1;
		var w = bgMap.w;
		var h = bgMap.h;
		while(dx > 0) { dx -= w; }
		while(dy > 0) { dy -= h; }
		for(var y = dy; y < sh/2 + h; y+=h) {
			for(var x = dx; x < sw/2 + w; x+=w) {
				if(x > 0-w && y > 0-h) {
					cx.save();
					cx.translate(x, y);
					cx.drawImage(bgMap._img, 0, 0);
					cx.restore();
				}
			}
		}

		var dx = -this.x0;
		var dy = -this.y0;
		var w = fgMap.w;
		var h = fgMap.h;
		while(dx > 0) { dx -= w; }
		while(dy > 0) { dy -= h; }
		for(var y = dy; y < sh/2 + h; y+=h) {
			for(var x = dx; x < sw/2 + w; x+=w) {
				if(x > 0-w && y > 0-h) {
					cx.save();
					cx.translate(x, y);
					cx.drawImage(fgMap._img, 0, 0);
					cx.restore();
				}
			}
		}

		return;
	}

});
