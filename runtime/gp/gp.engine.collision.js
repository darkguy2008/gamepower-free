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

var GPCollision = Class.extend({

	_init: function() {
		this.bmp1 = null;
		this.bmp2 = null;
		this.cx1 = null;
		this.cx2 = null;
	},

	Init: function() {
		this.bmp1 = document.createElement("canvas");
		this.bmp1.width = 1;
		this.bmp1.height = 1;

		this.bmp2 = document.createElement("canvas");
		this.bmp2.width = 1;
		this.bmp2.height = 1;

		this.cx1 = this.bmp1.getContext('2d');
		this.cx1.webkitImageSmoothingEnabled = false;
		this.cx1.mozImageSmoothingEnabled = false;
		this.cx1.imageSmoothingEnabled = false;

		this.cx2 = this.bmp2.getContext('2d');
		this.cx2.webkitImageSmoothingEnabled = false;
		this.cx2.mozImageSmoothingEnabled = false;
		this.cx2.imageSmoothingEnabled = false;

		//$(this.bmp1).css("border", "2px solid blue"); $(this.bmp1).appendTo("#debug");
		//$(this.bmp2).css("border", "2px solid green"); $(this.bmp2).appendTo("#debug");
		//$("#debug").css("display", "block");
	},

	GetAABB: function(p) {
		var radians = -(p.angle / 1000) * Math.PI / 180;

		var cx = p._map.cx * p.size / 100;
		var cy = p._map.cy * p.size / 100;
		var mapW = p._map.w / 2;
		var mapH = p._map.h / 2;

		var nwX = cx + (mapW) * Math.cos(radians) - (mapH) * Math.sin(radians);
		var nwY = cy + (mapH) * Math.cos(radians) + (mapH) * Math.sin(radians);
		var neX = cx - (mapW) * Math.cos(radians) - (mapH) * Math.sin(radians);
		var neY = cy + (mapH) * Math.cos(radians) - (mapH) * Math.sin(radians);
		var swX = cx + (mapW) * Math.cos(radians) + (mapH) * Math.sin(radians);
		var swY = cy - (mapH) * Math.cos(radians) + (mapH) * Math.sin(radians);
		var seX = cx - (mapW) * Math.cos(radians) + (mapH) * Math.sin(radians);
		var seY = cy - (mapH) * Math.cos(radians) - (mapH) * Math.sin(radians);

		nwX = nwX * p.size / 100;
		nwY = nwY * p.size / 100;
		neX = neX * p.size / 100;
		neY = neY * p.size / 100;
		swX = swX * p.size / 100;
		swY = swY * p.size / 100;
		seX = seX * p.size / 100;
		seY = seY * p.size / 100;

		nwX = nwX + p.x - cx * (p.size / 100);
		nwY = nwY + p.y - cy * (p.size / 100);
		neX = neX + p.x - cx * (p.size / 100);
		neY = neY + p.y - cy * (p.size / 100);
		swX = swX + p.x - cx * (p.size / 100);
		swY = swY + p.y - cy * (p.size / 100);
		seX = seX + p.x - cx * (p.size / 100);
		seY = seY + p.y - cy * (p.size / 100);

		var tlX = Math.min(nwX, neX, seX, swX);
		var tlY = Math.min(nwY, neY, seY, swY);
		var brX = Math.max(nwX, neX, seX, swX);
		var brY = Math.max(nwY, neY, seY, swY);

		rv = { x: tlX, y: tlY, w: brX - tlX, h: brY - tlY };

		//_gp.CScreen._ctx.save();
		//_gp.CScreen._ctx.translate(0, 0);
		//_gp.CScreen._ctx.strokeStyle = "#F00";
		//_gp.CScreen._ctx.beginPath();
		//_gp.CScreen._ctx.moveTo(nwX, nwY);
		//_gp.CScreen._ctx.lineTo(neX, neY);
		//_gp.CScreen._ctx.moveTo(neX, neY);
		//_gp.CScreen._ctx.lineTo(seX, seY);
		//_gp.CScreen._ctx.moveTo(seX, seY);
		//_gp.CScreen._ctx.lineTo(swX, swY);
		//_gp.CScreen._ctx.moveTo(swX, swY);
		//_gp.CScreen._ctx.lineTo(nwX, nwY);
		//_gp.CScreen._ctx.stroke();
		//_gp.CScreen._ctx.strokeStyle = "#0F0"; _gp.CScreen._ctx.strokeRect(rv.x, rv.y, rv.w, rv.h);
		//_gp.CScreen._ctx.restore();

		return rv;
	},

	SingleCollision: function(p1, p2) {
		var collide = false;
		if(p1._map != null && p2._map != null)
		{
			var aabb1 = this.GetAABB(p1);
			var aabb2 = this.GetAABB(p2);

			// https://www.youtube.com/watch?v=ghqD3e37R7E
			var Ax = aabb1.x;
			var Ay = aabb1.y;
			var AX = aabb1.x + aabb1.w;
			var AY = aabb1.y + aabb1.h;
			var Bx = aabb2.x;
			var By = aabb2.y;
			var BX = aabb2.x + aabb2.w;
			var BY = aabb2.y + aabb2.h;

			if(!(AX < Bx || BX < Ax || AY < By || BY < Ay))
			{
				var region = {};
				region.x1 = Math.min(aabb1.x, aabb2.x);
				region.y1 = Math.min(aabb1.y, aabb2.y);
				region.x2 = Math.max(aabb1.x + aabb1.w, aabb2.x + aabb2.w);
				region.y2 = Math.max(aabb1.y + aabb1.h, aabb2.y + aabb2.h);
				region.w = region.x2 - region.x1;
				region.h = region.y2 - region.y1;

				this.bmp1.width = region.w;
				this.bmp1.height = region.h;
				this.bmp2.width = region.w;
				this.bmp2.height = region.h;

				/*
				this.cx1.fillStyle = "#FFF";
				this.cx2.fillStyle = "#FFF";
				this.cx1.fillRect(0, 0, this.bmp1.width, this.bmp1.height);
				this.cx2.fillRect(0, 0, this.bmp2.width, this.bmp2.height);
				*/
				this.cx1.clearRect(0, 0, this.bmp1.width, this.bmp1.height);
				this.cx2.clearRect(0, 0, this.bmp2.width, this.bmp2.height);

				p1.DrawMask(this.cx1, -region.x1, -region.y1);
				p2.DrawMask(this.cx2, -region.x1, -region.y1);

				var data1 = this.cx1.getImageData(0, 0, this.bmp1.width, this.bmp1.height);
				var data2 = this.cx2.getImageData(0, 0, this.bmp2.width, this.bmp2.height);

				for(var i = 0; i < data1.data.length; i++)
					if(data1.data[i] > 0 && data2.data[i] > 0) {
						collide = true;
						break;
					}
			}

		}
		return collide;
	},

	Collision: function(src, type)
	{
		var rv = 0;
		var self = this;
		_gp.aProcess.filter(function(p) {
			return p._alive && p.type == type
		}).forEach(function(e) {
			if(rv == 0)
				if(self.SingleCollision(src, e))
					rv = e.id;
		});
		return rv;
	}

});
