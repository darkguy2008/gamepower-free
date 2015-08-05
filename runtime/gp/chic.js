/*
    This file is part of GamePower Engine FREE version.

    GamePower Engine FREE version is distributed in the hope that it
    will be useful, but WITHOUT ANY WARRANTY; without even the implied
    warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
    See the LICENSE file for more details.

    You should have received a copy of the LICENSE file
    along with GamePower Engine FREE version.
    If not, see < http://www.gamepower.com/Info.aspx >.

    Author: Rowan Manning. Forked by DARKGuy at: https://github.com/darkguy2008/chic
*/

/* global define */
(function (root, chic) {
    'use strict';

    // Utilities
    // ---------

    // Loop over object properties
    function forEachProp (obj, callback, thisArg) {
        var name, value;
        for (name in obj) {
            if (obj.hasOwnProperty(name)) {
                value = obj[name];
                callback.call(thisArg || value, value, name, obj);
            }
        }
    }

    // Check whether a value is a function
    function isFn (val) {
        return (typeof val === 'function');
    }

    // Apply a super-method to a function.
    // `this.sup` is set to `sup` inside calls to `fn`.
    function applySuperMethod (fn, sup) {
        return function () {
            var prev, result;
            prev = this.sup;
            this.sup = sup;
            result = fn.apply(this, arguments);
            this.sup = prev;
            if (typeof this.sup === 'undefined') {
                delete this.sup;
            }
            return result;
        };
    }

    // Base class
    function Class () {}

    // Extend
    Class.extend = function (props) {
        var Parent = this;
        var extendingFlag = '*extending*';
        var proto, reopenFlag;

        // Extension
        Parent[extendingFlag] = true;
        proto = new Parent();
        delete Parent[extendingFlag];
        reopenFlag = !isFn(props["_init"]);

        // Add new properties
        forEachProp(props, function (value, name) {
            if (isFn(value)) {
                reopenFlag ? Object.getPrototypeOf(proto)[name] = applySuperMethod(value, Parent.prototype[name]) : proto[name] = applySuperMethod(value, Parent.prototype[name]);
                return;
            }
            reopenFlag ? Object.getPrototypeOf(proto)[name] = value : proto[name] = value;
        });

        // Construct
        function Class () {
            if (!Class[extendingFlag] && isFn(proto._init)) {
                proto._init.apply(this, arguments);
            }
        }

        // Add prototypal properties
        Class.prototype = proto;
        Class.prototype.constructor = Class;

        // Add extend method and return
        Class.extend = Parent.extend;
        return Class;

    };
    chic.Class = Class;

    // Exports
    // -------

    // AMD
    if (typeof define !== 'undefined' && define.amd) {
        define([], function () {
            return chic;
        });
    }
    // CommonJS
    else if (typeof module !== 'undefined' && module.exports) {
        module.exports = chic;
    }
    // Script tag
    else {
        root.chic = chic;
    }

} (this, {}));
var Class = chic.Class;
