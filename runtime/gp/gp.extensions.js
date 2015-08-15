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

// http://tobyho.com/2012/10/21/javascript-OO-without-constructors/
function mix(base, obj){
    for (var key in obj)
        base[key] = obj[key]
}

// http://stackoverflow.com/questions/1804991/remove-dom-element-without-knowing-its-parent
function removeElement(el) {
	if(el)
		el.parentNode.removeChild(el);
}

function shimRAF() {
	var w=window;
	return w.requestAnimationFrame       ||
	       w.webkitRequestAnimationFrame ||
	       w.msRequestAnimationFrame     ||
	       w.mozRequestAnimationFrame    ||
	       w.oRequestAnimationFrame      ||
	       function(cb) { setTimeout(cb, 1e3/60); };
}

function rand(min, max) {
	if(!max) {
		max = min;
		min = 0;
	}
	return Math.floor(Math.random()*(max-min+1)+min);
}

// http://stackoverflow.com/questions/280634/endswith-in-javascript
String.prototype.endsWith = function(suffix) {
    return this.indexOf(suffix, this.length - suffix.length) !== -1;
};

String.prototype.startsWith = function(searchString, position) {
	position = position || 0;
	return this.indexOf(searchString, position) === position;
};

// http://stackoverflow.com/questions/8153725/how-to-use-strings-with-javascript-typed-arrays
DataView.prototype.getUTF8String = function(offset, length) {
    var utf16 = new ArrayBuffer(length * 2);
    var utf16View = new Uint16Array(utf16);
    for (var i = 0; i < length; ++i) {
    	var code = this.getUint8(offset + i);
    	if(code == 0)
    		code = 124;
         utf16View[i] = code;
    }
    return String.fromCharCode.apply(null, utf16View);
};

/*
 * base64-arraybuffer
 * https://github.com/niklasvh/base64-arraybuffer
 *
 * Copyright (c) 2012 Niklas von Hertzen
 * Licensed under the MIT license.
 */
function Base64ToArrayBuffer(base64) {
    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
    var bufferLength = base64.length * 0.75,
    len = base64.length, i, p = 0,
    encoded1, encoded2, encoded3, encoded4;

    if (base64[base64.length - 1] === "=") {
        bufferLength--;
        if (base64[base64.length - 2] === "=") {
            bufferLength--;
        }
    }

    var arraybuffer = new ArrayBuffer(bufferLength),
    bytes = new Uint8Array(arraybuffer);

    for (i = 0; i < len; i+=4) {
        encoded1 = chars.indexOf(base64[i]);
        encoded2 = chars.indexOf(base64[i+1]);
        encoded3 = chars.indexOf(base64[i+2]);
        encoded4 = chars.indexOf(base64[i+3]);

        bytes[p++] = (encoded1 << 2) | (encoded2 >> 4);
        bytes[p++] = ((encoded2 & 15) << 4) | (encoded3 >> 2);
        bytes[p++] = ((encoded3 & 3) << 6) | (encoded4 & 63);
    }

    return arraybuffer;
}

function hasFlag(e, f)
{
    return (e & f) == f;
}

function setFlag(e, f)
{
    e = e | f;
}

function unsetFlag(e, f)
{
    e = e ^ f;
}

function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++)
        if ((new Date().getTime() - start) > milliseconds)
            break;
}

// http://yehudakatz.com/2011/08/11/understanding-javascript-function-invocation-and-this/
var bind = function(func, thisValue) {
    return function() {
        return func.apply(thisValue, arguments);
    }
}

// https://gist.github.com/addyosmani/5434533
var limitLoop = function (fn, fps) {

    // Use var then = Date.now(); if you
    // don't care about targetting < IE9
    var then = new Date().getTime();

    // custom fps, otherwise fallback to 60
    fps = fps || 60;
    var interval = 1000 / fps;

    return (function loop(time){
        requestAnimationFrame(loop);

        // again, Date.now() if it's available
        var now = new Date().getTime();
        var delta = now - then;

        if (delta > interval) {
            // Update time
            // now - (delta % interval) is an improvement over just
            // using then = now, which can end up lowering overall fps
            then = now - (delta % interval);

            // call the fn
            fn();
        }
    }(0));
};

// http://nokarma.org/2011/02/02/javascript-game-development-the-game-loop/
var _canvas = null;
(function() {
  var onEachFrame;
    onEachFrame = function(cb) {
        var _cb = function() { cb(); window.requestAnimationFrame(_cb, _canvas); }
        _cb();
    };

  window.onEachFrame = onEachFrame;
})();

// TODO: Which function is better?
/*
Array.prototype.filter = function(fn) {
    var results = [];
    var item;
    for (var i = 0, len = this.length; i < len; i++) {
    item = this[i];
    if (fn(item)) results.push(item);
    }
    return results;
}
*/

/*
Array.prototype.filter = function(predicate) {
  var results = [],
    len = this.length,
    i = 0;

  for (; i < len; i++) {
    var item = this[i];
    if (predicate(item)) {
      results.push(item);
    }
  }

  return results;
};
*/

/*
Array.prototype.filter = function(fn) {
    var i = 0;
    var ln = this.length;
    var results = [];

    while (i < ln)
    {
        var item = this[i];
        if (fn(item))
            results.push(item);
        i++;
    }

    return results;
}
*/

// Shamelessly taken from lodash's filter function
Array.prototype.filter = function(fn) {
    var i = -1,
    ri = -1,
    ln = this.length,
    rv = [];
    while (++i < ln)
    {
        var v = this[i];
        if (fn(v))
            rv[++ri] = v;
    }
    return rv;
};

// http://stackoverflow.com/questions/2239567/jquery-check-if-element-has-css-attribute
getAttrCSS = function(e, attr) {
    return $(e).filter(function() { return $(e).css(attr).length > 0; }).eq(0).css(attr);
};
