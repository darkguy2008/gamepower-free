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

function write_int(fontId, x, y, center, obj, v)
{
	var p = new GPText();
	p.x = x;
	p.y = y;
	p.z = -128;
	p.center = center;
	p.idFont = fontId;
	p.id = _gp.NewPID();
	p.text = obj[v].toString();
	p._code[0] = function() { p.text = obj[v].toString(); };
	p._code[1] = function() { p.Frame(); };
	p._code[2] = function() { p._instLocal = 0; };
	return _gp.AddProcess(p);
}

function write(fontId, x, y, center, text)
{
	var p = new GPText();
	p.x = x;
	p.y = y;
	p.z = -128;
	p.center = center;
	p.idFont = fontId;
	p.id = _gp.NewPID();
	p.text = text;
	p._code[0] = function() { p.text = text; };
	p._code[1] = function() { p.Frame(); };
	p._code[2] = function() { p._instLocal = 0; };
	return _gp.AddProcess(p);
}

// TODO: all_text = remove all texts.
function delete_text(idText)
{
	signal(idText, s_kill);
}
