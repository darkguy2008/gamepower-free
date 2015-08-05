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

var errors = [];
if($("#debug").length > 0)
{

    $(window).on("error", function(evt) {

        var e = evt.originalEvent;
        var text = e.message ?
        	(e.message + " | Line: " + e.lineno + " | " + e.filename) :
        	(e.type + " | Element: " + (e.srcElement || e.target));

        errors.push(text);
        $("#debug").empty();
        errors.forEach(function(t) {
            $("<p>" + t + "</p>").appendTo('#debug');
        })

        $('#debug').css("display", "block");
    });

};

function AppExit(errCode)
{
    window.close();
}

function AppDebugger()
{
    alert("Unavailable for now");
}

// TODO: Use this for in-browser fullscreen. Involves a bit of a change, setting
// the game inside an iframe and such. Hacky but it looks like it could work.
// http://stackoverflow.com/a/30970886/1598811

function AppSetFullscreen()
{
    alert("Unavailable for now");
}

function AppSetWindowed()
{
    alert("Unavailable for now");
}

function AppToggleFullscreen()
{
    alert("Unavailable for now");
}
