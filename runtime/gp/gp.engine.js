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

// CRITICAL: Littera char "2" incorrect size??? it's invisible!

var GPEngine = Class.extend({

	_init: function() {

		this.aProcess = [];
		this.aGlobals = [];
		this.inst = 0;
		this.frameTime = 0;
		this._program = null;
		this.paused = false;
		this.debug = false;

		this.parentProcess = null;
		this.callerProcess = null;
		this.ResMgr = new GPResourceManager();

		this._lastFPG = 0;
		this._lastFNT = 1;
		this._lastSONG = 0;
		this._lastAUDIO = 0;

		this._listFPG = [];
		this._listSONG = [];
		this._listAUDIO = [];
		this._listFNT = [
			"default.fnt"
		];
		this.res = {
			fpg: [],
			fnt: [],
			song: [],
			audio: []
		};
		this._finishedLoading = true;

		this.CScreen = new GPScreen();
		this.CMouse = new GPMouse();
		this.CCollision = new GPCollision();
		this.CKeyboard = new GPKeyboard();
		this.CAudio = new GPAudio();
		this.CSong = new GPSong();
		this.CRegions = [];
		this.CScrolls = [];
		for(var i = 0; i < 10; i++)
			this.CRegions[i] = new GPRegion();
		for(var i = 0; i < 10; i++)
			this.CScrolls[i] = new GPScroll();
	},

	EntryPoint: function(p) {
		this.AddProcess(p);
	},

	Init: function(programProcess) {

		_gp._program = programProcess;
		_gp.parentProcess = _gp.aProcess[0];

		set_mode(m320x240);

		var iCurrent = 0;
		var iTotal = 0;

		this.ResMgr.Init();
	},

	Ready: function() {

		_gp.idx = 0;
		_gp.procToRun = _gp.aProcess;
		_gp.findNew = true;
		_gp.curProcess = null;

		_gp.CMouse.Init();
		_gp.CCollision.Init();
		_gp._program();

		// TODO: Stabilize FPS step
		$(_gp.CScreen._canvas).fadeOut(0);
		fade_on();
		_gp.MainLoop();
		//window.onEachFrame(function() { _gp.MainLoop(); });
	},

	_getNextProcess: function()
	{
		var rv =
			_gp.aProcess
			.sort(function(a, b) { return b.priority - a.priority; })
			.filter(function(e) { return e._framePercent < 100 && e._alive; });
		if(rv.length > 0)
			return rv[0];
		else
			return undefined;
	},

	_getNextUndrawn: function()
	{
		var rv = _gp.aProcess.filter(function(p) {
			return !p._drawn && p._framePercent >= 100 && p.ctype == c_screen;
		}).sort(function(a, b) {
			return b.z - a.z;
		});
		if(rv.length > 0)
			return rv[0];
		else
			return undefined;
	},

	// TODO: cnumber = FLAGS (1, 2, 4, 8, 16..512)
	_getNextUndrawnScroll: function(idscroll)
	{
		var rv = _gp.aProcess.filter(function(p) {
			return !p._drawn && p._framePercent >= 100 && p.ctype == c_scroll;
		}).sort(function(a, b) {
			return b.z - a.z;
		});
		if(rv.length > 0)
			return rv[0];
		else
			return undefined;
	},

	DrawMouse: function() {
		var m = _gp.aProcess.filter(function(p) { return p._type == "mouse" })[0];
		m._framePercent -= 100;
		m._drawn = true;
		m.Draw();
	},

	MainLoop: function() {
		var proc = null;
		var self = this;

		// TODO
		// http://stackoverflow.com/questions/15457829/how-to-drop-frames-in-html5-canvas
		// http://www.gamedev.net/topic/623596-javascriptcanvas-constan-frame-rate/
		// http://www.paulirish.com/2011/requestanimationframe-for-smart-animating/
		// http://gafferongames.com/game-physics/fix-your-timestep/
		window.requestAnimationFrame(function() { self.MainLoop.call(self); });

		_gp.frameTime -= 100;
		if(_gp.frameTime < 0) {
			_gp.frameTime = 100;

			_gp.aProcess.forEach(function(p) { p._drawn = false; });
			_gp.aProcess.sort(function(a, b) {
				return b.priority - a.priority;
			}).forEach(function(p) {
				switch(p._signal) {
					case s_kill:
						p._alive = false;
						break;
					case s_kill_tree:
						proc = p;
						p._alive = false;
						while(proc.son != null) {
							proc._alive = false;
							proc = proc.son;
						}
						break;
				}
			});

			//if(_gp.aProcess.filter(function(e) { return !e._alive; }).length > 0)
				//_gp.aProcess = _gp.aProcess.filter(function(e) { return e._alive; });
			if(_gp.aProcess.filter(function(e) { return !e._alive; }).length > 0)
			{
				var i = 0;
				var imax = _gp.aProcess.length;
				while(i < imax)
				{
					if(_gp.aProcess[i]._alive == 0)
					{
						_gp.aProcess[i] = null;
						_gp.aProcess.splice(i, 1);
						imax = _gp.aProcess.length;
					}
					i++;
				}
			}

			// TODO: Optimize. Maybe run stuff in RAF and THEN update if frameTime < 0.
			//       Maybe queue updates? do something!!! :(, else, all code runs when
			//       a frame is being drawn, thus, slowing it all down :/
			_gp.curProcess = _gp._getNextProcess();
			if(_gp.curProcess != undefined) {

				while(true)
				{
					if(_gp.curProcess == undefined) { break; }

					_gp.parentProcess = _gp.curProcess;
					var inst = _gp.curProcess._code[_gp.curProcess._instLocal];

					if(inst == undefined) {
						_gp.curProcess._alive = false;
						_gp.curProcess = _gp._getNextProcess();
					}
					if(_gp.curProcess != undefined)
					{
						inst = _gp.curProcess._code[_gp.curProcess._instLocal];
						_gp.curProcess._instLocal++;
						_gp.curProcess.Update();
						if(inst != undefined) {
							if(!_gp.debug)
								inst.call(_gp.curProcess);
							else
							{
								try {
									inst.call(_gp.curProcess);
								}
								catch(e) {
									console.log(e);
								}
							}
						}
						if(_gp.curProcess._framePercent >= 100)
							_gp.curProcess = _gp._getNextProcess();
					}

					_gp.CKeyboard.keys.forEach(function(e) { e = false; });
				}
			}

			_gp.CScreen.clear();

			proc = _gp._getNextUndrawn();
			while(proc != undefined) {
				proc._framePercent -= 100;
				proc._drawn = true;
				proc.Draw();
				if(proc._type == "scroll") {
					var procScroll = _gp._getNextUndrawnScroll(proc.id);
					while(procScroll != undefined) {
						procScroll._framePercent -= 100;
						procScroll._drawn = true;
						procScroll.Draw();
						procScroll = _gp._getNextUndrawnScroll(proc.id);
					}
				}
				proc = _gp._getNextUndrawn();
			}

			_gp.DrawMouse();
		}
	},

	AddProcess: function(p) {
		var id = _gp.aProcess.push(p);
		_gp.curProcess = p;
		_gp.curProcess._instLocal = 0;

		var rv = undefined;
		while(true)
		{
			if(_gp.curProcess == undefined) { break; }

			_gp.parentProcess = _gp.curProcess;
			var inst = _gp.curProcess._code[_gp.curProcess._instLocal];

			if(inst == undefined) {
				_gp.curProcess._alive = false;
				break;
			}
			if(_gp.curProcess != undefined)
			{
				inst = _gp.curProcess._code[_gp.curProcess._instLocal];
				_gp.curProcess._instLocal++;
				_gp.curProcess.Update();
				if(inst != undefined) {
					if(!_gp.debug)
					{
						rv = inst.call(_gp.curProcess);
						if(rv != undefined) {
							_gp.curProcess._alive = false;
							break;
						}
					}
					else
					{
						try {
							rv = inst.call(_gp.curProcess);
							if(rv != undefined) {
								_gp.curProcess._alive = false;
								break;
							}
						}
						catch(e) {
							console.log(e);
						}
					}
				}
				if(_gp.curProcess._framePercent >= 100)
					break;
			}
		}

		// TODO: Chequear bien esto
		/*
		if(rv == undefined)
			if(_gp.curProcess._type == "process")
				rv = _gp.curProcess;
			else
				rv = p.id;
		*/
		rv = _gp.aProcess[id - 1];
		return rv;
	},

	SetParent: function(p) {
		_gp.parentProcess = p;
	},

	SetCaller: function(p) {
		_gp.callerProcess = p;
	},

	GetProcess: function(arg) {
		if(typeof arg === 'number')
			return _gp.aProcess.filter(function(p) { return p.id == arg; })[0];
		else
			return arg;
	},

	// http://stackoverflow.com/questions/13096408/make-math-random-return-positive-odd-integer
	NewPID: function() {
	    var rv = rand(0, 9999);
	    if(rv % 2 == 0)
			if(rv == 9999)
				rv = rv - 1;
			else
				rv = rv + 1;
	    return rv;
	}

});
