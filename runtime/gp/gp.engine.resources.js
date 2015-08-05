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

var GPResourceManager = Class.extend({

	_init: function() {

		this.preloading = false;
		this.preloader = new GPPreloader();

	},

 	Init: function() {
 		var self = this;
 		this.preloading = true;
 		this.preloader.Init(function() {
			self.LoadNextResource();
 		});
 	},

 	Refresh: function() 
 	{
		this.preloader.Draw();
 	},

 	CheckResourceFinished: function(host)
 	{
		for(r in host)
		{
			if(host[r].cur >= host[r].total)
				host[r].status = 2;
			if(host[r].status != 2)
				this._finishedLoading = false;
		}
 	},

	LoadNextResource: function()
	{
		var self = this;
		self._finishedLoading = true;
		zip.useWebWorkers = false;

		this.CheckResourceFinished(_gp.res.fpg);
		this.CheckResourceFinished(_gp.res.fnt);
		for(f in _gp.res.fnt)
			if(_gp.res.fnt[f].status == 2)
			{
				function sortWidth(a, b){ return a.w < b.w ? 0 : 1; }
				function sortHeight(a, b){ return a.h < b.h ? 0 : 1; }
				var fw = _gp.res.fnt[f].glyphs.sort(sortWidth);
				var fh = _gp.res.fnt[f].glyphs.sort(sortHeight);
				_gp.res.fnt[f].w = fw[fw.length - 1].w;
				_gp.res.fnt[f].h = fw[fw.length - 1].h;
			}

		this.CheckResourceFinished(_gp.res.song);
		this.CheckResourceFinished(_gp.res.audio);

		// TODO: Optimize
		var max = 0;
		var cur = 0;
		var addMax = function(a) { for(f in a) { max += a[f].total || 0; } };
		var addCur = function(a) { for(f in a) { cur += a[f].cur || 0; } };
		addMax(_gp.res.fpg);
		addMax(_gp.res.fnt);
		addMax(_gp.res.song);
		addMax(_gp.res.audio);
		addCur(_gp.res.fpg);
		addCur(_gp.res.fnt);
		addCur(_gp.res.song);
		addCur(_gp.res.audio);

		if(max > 0)
		{
			this.preloader.value = cur;
			this.preloader.total = max;
		}		
		this.Refresh();

		if(!self._finishedLoading)
		{
			for(f in _gp.res.fpg)
				if(_gp.res.fpg[f].status == 0)
					this.LoadMAP(_gp.res.fpg[f], _gp.res.fpg[f].data, f, true);

			for(f in _gp.res.fnt)
				if(_gp.res.fnt[f].status == 0)
				{
					if(_gp.res.fnt[f]._format == "fnt")
					{
						_gp.res.fnt[f].glyphs = [];
						this.LoadMAP(_gp.res.fnt[f], _gp.res.fnt[f].glyphs, f, true, function(m, fn) {
							m.id = parseInt(m.description);
						});
					}
				}

			for(f in _gp.res.song)
				if(_gp.res.song[f].status == 0)
					this.LoadSong(_gp.res.song[f], f);

			for(f in _gp.res.audio)
				if(_gp.res.audio[f].status == 0)
					this.LoadAudio(_gp.res.audio[f], f);

			setTimeout(function() { self.LoadNextResource() }, 100);
		}
		else
		{
			this.Refresh();
			this.preloader.End(function() {
				self.preloading = false;
				_gp.Ready();
			});			
		}

		return;
	},

	LoadSong: function(host, filename) {
		host.status = 1;
		host.cur = 0;
		host.total = 1;
		_gp.CSong.Load(filename, function() {
			host.cur++;
			host.status = 2;
		});
	},

	LoadAudio: function(host, filename) {
		host.status = 1;
		host.cur = 0;
		host.total = 1;
		_gp.CAudio.Load(filename, function(audio) {
			host.cur++;
			host.data = audio;
			host.status = 2;
		});
	},

	LoadMAP: function(host, arr, filename, generateMasks, callback) {
		host.status = 1;
		$.ajax({
			dataType: 'native',
			url: filename,
			xhrFields: { responseType: 'blob' },
			mimeType: "application/zip",
			success: function(data) {
				zip.createReader(new zip.BlobReader(data), function(reader) {
					reader.getEntries(function(entries) {
						if (entries.length)
						{
							host.cur = 0;
							host.total = entries.length;
							entries.forEach(function(e) {
								if(!e.filename.toLowerCase().endsWith("png"))
									host.total--;
								else
								{
									var m = {};
									e.getData(new zip.Data64URIWriter("image/png"), function(b64) {
										if(e.filename.indexOf('_') > 0)
											m.id = parseInt(e.filename.substr(0, e.filename.indexOf('_')));
										else
											m.id = parseInt(e.filename);

										m._img = new Image();
										m._img.addEventListener("load", function() {
										    m.w = m._img.width;
										    m.h = m._img.height;

										    if(generateMasks)
										    {
												var tmp = document.createElement("canvas");
												var tmpCx = tmp.getContext('2d');
											    tmp.width = m._img.width;
											    tmp.height = m._img.height;

											    tmpCx.drawImage(m._img,0,0);
								    			var imgData = tmpCx.getImageData(0,0,tmp.width,tmp.height);
											    var pix = imgData.data;
											    for (var i=0, n = pix.length; i < n; i+= 4){
											        pix[i] = 0;
											        pix[i+1] = 0;
											        pix[i+2] = 0;
											        pix[i+3] = pix[i+3];
											    }
								    			tmpCx.putImageData(imgData,0,0);
								    			m._mask = new Image();
								    			m._mask.src = tmp.toDataURL();

								    			tmp = null;
								    			tmpCx = null;
								    		}

							    			var png = ReadPNG(b64);
											for(c in png)
											{
												if(png[c].fourCC == "tEXt") {
													var str = png[c].data;
													if(str.startsWith("ControlPoints"))
													{
														str = str.substr("ControlPoints".length + 1);
														var CPs = str.split("|");
														for(iCP in CPs)
														{
															var cp = CPs[iCP].split("=");
															m.controlPoints = [];
															m.controlPoints[cp[0]] = { x: cp[1].split(",")[0], y: cp[1].split(",")[1] };
															if(cp[0] == 0)
															{
																m.cx = cp[1].split(",")[0];
																m.cy = cp[1].split(",")[1];
															}
														}
													}
													else if(str.startsWith("OffsetPos"))
													{
														str = str.substr("OffsetPos".length + 1);
														m.ox = str.split(",")[0];
														m.oy = str.split(",")[1];
													}
													else if(str.startsWith("OffsetSize"))
													{
														str = str.substr("OffsetSize".length + 1);
														m.ow = str.split(",")[0];
														m.oh = str.split(",")[1];
													}
													else if(str.startsWith("Description"))
													{
														str = str.substr("Description".length + 1);
														m.description = str;
													}
												}
											}
											
											if(callback)
												callback(m, filename);

										}, true);
										m._img.src = b64;
										arr.push(m);
										host.cur++;
									}, function(current, total) { });
								}
							});
							reader.close(function() { });
						}
					});
				}, function(error) { console.log("Error", error); /* Error */  });
			}
		});
	}

});