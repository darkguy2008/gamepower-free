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

var GPRegion = Class.extend({

	_init: function() {
		this.id = -1;
		this.x = 0;
		this.y = 0;
		this.x2 = 0;
		this.y2 = 0;
		this.w = 0;
		this.h = 0;
	},

	Define: function(x, y, w, h) {
		this.x = x;
		this.y = y;
		this.w = w;
		this.h = h;
		this.x2 = x + w;
		this.y2 = y + h;
	}

});
