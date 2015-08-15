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

var GPScreen = Class.extend({

	_init: function() {
		this._ctx = null;
		this._div = null;
		this._canvas = null;
		this.options = {
			div: "game",
			size: [320, 200],
			scale: 1,
			antialias: false,
			color: "#000",
			img: null,
			fullscreen: true,
		};
		this._bgCtx = null;
		this._bgCanvas = null;
	},

	// http://ejohn.org/blog/dom-documentfragments/
	recreate: function() {
		var self = this;

		removeElement(document.getElementById("game_canvas"));
		this._div = document.getElementById(this.options.div)

		var fragment = document.createDocumentFragment();
		this._canvas = document.createElement('canvas');
		this._canvas.id = "game_canvas";
		this._canvas.width = this.options.size[0] * this.options.scale;
		this._canvas.height = this.options.size[1] * this.options.scale;
		this._canvas.setAttribute("moz-opaque", null);

		this._canvas.addEventListener("contextmenu", function(ev) {
				ev.stopPropagation();
				ev.preventDefault();
				return false;
		});

		this._ctx = this._canvas.getContext("2d");
		this._ctx.scale(this.options.scale, this.options.scale);

		if(_gp.Flags.FSAA != undefined)
			this.options.antialias = _gp.Flags.FSAA;

		this._ctx.webkitImageSmoothingEnabled = this.options.antialias;
		this._ctx.mozImageSmoothingEnabled = this.options.antialias;
		this._ctx.imageSmoothingEnabled = this.options.antialias;

		fragment.appendChild(this._canvas);
		this._div.appendChild(fragment);

		this._canvas.setAttribute('tabindex','0');
		this._canvas.focus();

		_gp.CMouse.Rewire();
		_gp.CKeyboard.Init();
		_gp.CRegions[0].Define(0, 0, this.options.size[0], this.options.size[1]);

		this.refresh();

		$(window).resize(function() {
			self.refresh();
		});
	},

	clearCanvas: function(ctx) {
		ctx.save();
		ctx.fillStyle = this.options.color;
		ctx.fillRect(0, 0, this.options.size[0], this.options.size[1]);
		if(this.options.img)
			ctx.drawImage(this.options.img, 0, 0);
		ctx.restore();
	},

	clear: function() {
		if(!this._ctx)
		{
			this.recreate();
			return;
		}
		this.clearCanvas(this._ctx);
	},

	_propScale: function(originalSize, newSize)
	{
	    var ratio = originalSize[0] / originalSize[1];

	    var maximizedToWidth = [newSize[0], newSize[0] / ratio];
	    var maximizedToHeight = [newSize[1] * ratio, newSize[1]];

	    if(maximizedToWidth[1] > newSize[1]) { return maximizedToHeight; }
	    else { return maximizedToWidth; }
	},

	refresh: function() {
		if(!this.options.fullscreen)
			return;

		this._canvas.width = 1;
		this._canvas.height = 1;

		var viewWidth = $(window).width();
		var viewHeight = $(window).height();

		if(_gp.Flags.FullscreenMode != undefined)
			if(_gp.Flags.FullscreenMode == 3)
			{
				if(viewWidth > this.options.size[0])
					viewWidth = this.options.size[0];
				if(viewHeight > this.options.size[1])
					viewHeight = this.options.size[1];
			}

		var scale = this._propScale([this.options.size[0], this.options.size[1]], [viewWidth, viewHeight]);
		this._canvas.width = scale[0];
		this._canvas.height = scale[1];

		var s = 1 * this._canvas.width / this.options.size[0];
		this.options.scale = s;
		this._ctx.scale(s, s);
		this._ctx.webkitImageSmoothingEnabled = this.options.antialias;
		this._ctx.mozImageSmoothingEnabled = this.options.antialias;
		this._ctx.imageSmoothingEnabled = this.options.antialias;

        if(viewHeight > this._canvas.height)
        {
            $(this._canvas).css({
                "position" : "absolute",
                "top" : (viewHeight/2 - this._canvas.height/2) + "px",
                "left" : "0px",
                'width' : "100%"
            });
        }
        else
            $(this._canvas).css({
                "position" : "initial",
                'width' : "auto"
            });

        if(_gp.ResMgr.preloading)
        	_gp.ResMgr.Refresh();
	}

});
