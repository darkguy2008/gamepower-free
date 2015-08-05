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
// Ref: http://jsfiddle.net/oopwngoo/9j37ptr6/

var _esc         = 1 ; // [ESC] o escape
var _f1          = 59; // [F1] o función 1
var _f2          = 60; // [F2] o función 2
var _f3          = 61; // [F3] o función 3
var _f4          = 62; // [F4] o función 4
var _f5          = 63; // [F5] o función 5
var _f6          = 64; // [F6] o función 6
var _f7          = 65; // [F7] o función 7
var _f8          = 66; // [F8] o función 8
var _f9          = 67; // [F9] o función 9
var _f10         = 68; // [F10] o función 10
var _f11         = 87; // [F11] o función 11
var _f12         = 88; // [F12] o función 12 (TRAZADOR)
var _prn_scr     = 55; // [IMPR PANT] o impresión pantalla
var _scroll_lock = 70; // [BLOQ DESPL] o bloqueo desplazamiento
var _wave        = 41; // Tecla [º] o [ª]
var _1           = 2 ; // Tecla con el número "1"
var _2           = 3 ; // Tecla con el número "2"
var _3           = 4 ; // Tecla con el número "3"
var _4           = 5 ; // Tecla con el número "4"
var _5           = 6 ; // Tecla con el número "5"
var _6           = 7 ; // Tecla con el número "6"
var _7           = 8 ; // Tecla con el número "7"
var _8           = 9 ; // Tecla con el número "8"
var _9           = 10; // Tecla con el número "9"
var _0           = 11; // Tecla con el número "0"
var _minus       = 12; // Tecla con el símbolo "?"
var _plus        = 13; // Tecla con el símbolo "¿"
var _backspace   = 14; // Tecla de borrado ( <- )
var _tab         = 15; // Tecla de tabulador [TAB]
var _q           = 16; // Tecla con la letra "Q"
var _w           = 17; // Tecla con la letra "W"
var _e           = 18; // Tecla con la letra "E"
var _r           = 19; // Tecla con la letra "R"
var _t           = 20; // Tecla con la letra "T"
var _y           = 21; // Tecla con la letra "Y"
var _u           = 22; // Tecla con la letra "U"
var _i           = 23; // Tecla con la letra "I"
var _o           = 24; // Tecla con la letra "O"
var _p           = 25; // Tecla con la letra "P"
var _l_brachet   = 26; // Tecla [^] o [`]
var _r_brachet   = 27; // Tecla [*] o [+]
var _enter       = 28; // [ENTER] (Intro o Retorno)
var _caps_lock   = 58; // [BLOQ MAYUS] o bloqueo mayúsculas
var _a           = 30; // Tecla con la letra "A"
var _s           = 31; // Tecla con la letra "S"
var _d           = 32; // Tecla con la letra "D"
var _f           = 33; // Tecla con la letra "F"
var _g           = 34; // Tecla con la letra "G"
var _h           = 35; // Tecla con la letra "H"
var _j           = 36; // Tecla con la letra "J"
var _k           = 37; // Tecla con la letra "K"
var _l           = 38; // Tecla con la letra "L"
var _semicolon   = 39; // Tecla con la letra "Ñ"
var _apostrophe  = 40; // Tecla [{{}]
var _backslash   = 43; // Tecla [{}}]
var _l_shift     = 42; // [SHIFT] o mayúsculas izquierdo
var _z           = 44; // Tecla con la letra "Z"
var _x           = 45; // Tecla con la letra "X"
var _c           = 46; // Tecla con la letra "C"
var _v           = 47; // Tecla con la letra "V"
var _b           = 48; // Tecla con la letra "B"
var _n           = 49; // Tecla con la letra "N"
var _m           = 50; // Tecla con la letra "M"
var _comma       = 51; // Tecla [;] o [,]
var _point       = 51; // Tecla [:] o [.]
var _slash       = 51; // Tecla [_] o [-]
var _r_shift     = 54; // [SHIFT] o mayúsculas derecho
var _control     = 29; // Teclas [CONTROL]
var _alt         = 56; // Tecla [ALT] o [ALT GR]
var _space       = 57; // [SPACE] o barra espaciadora
var _ins         = 82; // [INSERT] o insertar
var _home        = 71; // [INICIO] o inicio de página
var _pgup        = 73; // [RE PAG] o retroceso de página
var _del         = 83; // [SUPR] o suprimir
var _end         = 79; // [FIN] o fin de página
var _pgdn        = 81; // [AV PAG] o avance de página
var _up          = 72; // Cursor para arriba
var _down        = 80; // Cursor para abajo
var _left        = 75; // Cursor para izquierda
var _right       = 77; // Cursor para derecha
var _num_lock    = 69; // [BLOQ NUM] o bloqueo numérico
var _c_backslash = 53; // Símbolo [/] del teclado numérico
var _c_asterisk  = 55; // Símbolo [*] del teclado numérico
var _c_minus     = 74; // Símbolo [-] del teclado numérico
var _c_home      = 71; // [INICIO] del teclado numérico
var _c_up        = 72; // Cursor arriba del teclado numérico
var _c_pgup      = 73; // [RE PAG] del teclado numérico
var _c_left      = 75; // Cursor izquierda del teclado numérico
var _c_center    = 76; // Tecla [5] del teclado numérico
var _c_right     = 77; // Cursor derecha del teclado numérico
var _c_end       = 79; // [FIN] del teclado numérico
var _c_down      = 80; // Cursor abajo del teclado numérico
var _c_pgdn      = 81; // [AV PAG] del teclado numérico
var _c_ins       = 82; // [INS] del teclado numérico
var _c_del       = 83; // [SUPR] del teclado numérico
var _c_plus      = 78; // Símbolo [+] del teclado numérico
var _c_enter     = 28; // [ENTER] del teclado numérico

var _keyTable = {};
_keyTable[27] = _esc;          // [ESC] o escape   
_keyTable[112] = _f1;          // [F1] o función 1
_keyTable[113] = _f2;          // [F2] o función 2
_keyTable[114] = _f3;          // [F3] o función 3
_keyTable[115] = _f4;          // [F4] o función 4
_keyTable[116] = _f5;          // [F5] o función 5
_keyTable[117] = _f6;          // [F6] o función 6
_keyTable[118] = _f7;          // [F7] o función 7
_keyTable[119] = _f8;          // [F8] o función 8
_keyTable[120] = _f9;          // [F9] o función 9
_keyTable[121] = _f10;         // [F10] o función 10
_keyTable[122] = _f11;         // [F11] o función 11
_keyTable[123] = _f12;         // [F12] o función 12 (TRAZADOR)
//_keyTable[] = _prn_scr;      // [IMPR PANT] o impresión pantalla
_keyTable[145] = _scroll_lock; // [BLOQ DESPL] o bloqueo desplazamiento
_keyTable[220] = _wave;        // Tecla [º] o [ª]
_keyTable[49] = _1;            // Tecla con el número "1"
_keyTable[50] = _2;            // Tecla con el número "2"
_keyTable[51] = _3;            // Tecla con el número "3"
_keyTable[52] = _4;            // Tecla con el número "4"
_keyTable[53] = _5;            // Tecla con el número "5"
_keyTable[54] = _6;            // Tecla con el número "6"
_keyTable[55] = _7;            // Tecla con el número "7"
_keyTable[56] = _8;            // Tecla con el número "8"
_keyTable[57] = _9;            // Tecla con el número "9"
_keyTable[48] = _0;            // Tecla con el número "0"
_keyTable[219] = _minus;       // Tecla con el símbolo "?"
_keyTable[221] = _plus;        // Tecla con el símbolo "¿"
_keyTable[8] = _backspace;     // Tecla de borrado ( <- )
_keyTable[9] = _tab;           // Tecla de tabulador [TAB]
_keyTable[81] = _q;            // Tecla con la letra "Q"
_keyTable[87] = _w;            // Tecla con la letra "W"
_keyTable[69] = _e;            // Tecla con la letra "E"
_keyTable[82] = _r;            // Tecla con la letra "R"
_keyTable[84] = _t;            // Tecla con la letra "T"
_keyTable[89] = _y;            // Tecla con la letra "Y"
_keyTable[85] = _u;            // Tecla con la letra "U"
_keyTable[73] = _i;            // Tecla con la letra "I"
_keyTable[79] = _o;            // Tecla con la letra "O"
_keyTable[80] = _p;            // Tecla con la letra "P"
_keyTable[222] = _l_brachet;   // Tecla [^] o [`]
_keyTable[191] = _r_brachet;   // Tecla [*] o [+]
_keyTable[13] = _enter;        // [ENTER] (Intro o Retorno)
_keyTable[20] = _caps_lock;    // [BLOQ MAYUS] o bloqueo mayúsculas
_keyTable[65] = _a;            // Tecla con la letra "A"
_keyTable[83] = _s;            // Tecla con la letra "S"
_keyTable[68] = _d;            // Tecla con la letra "D"
_keyTable[70] = _f;            // Tecla con la letra "F"
_keyTable[71] = _g;            // Tecla con la letra "G"
_keyTable[72] = _h;            // Tecla con la letra "H"
_keyTable[74] = _j;            // Tecla con la letra "J"
_keyTable[75] = _k;            // Tecla con la letra "K"
_keyTable[76] = _l;            // Tecla con la letra "L"
_keyTable[192] = _semicolon;   // Tecla con la letra "Ñ"
_keyTable[186] = _apostrophe;  // Tecla [{{}]
_keyTable[187] = _backslash;   // Tecla [{}}]
_keyTable[16] = _l_shift;      // [SHIFT] o mayúsculas izquierdo
_keyTable[90] = _z;            // Tecla con la letra "Z"
_keyTable[88] = _x;            // Tecla con la letra "X"
_keyTable[67] = _c;            // Tecla con la letra "C"
_keyTable[86] = _v;            // Tecla con la letra "V"
_keyTable[66] = _b;            // Tecla con la letra "B"
_keyTable[78] = _n;            // Tecla con la letra "N"
_keyTable[77] = _m;            // Tecla con la letra "M"
_keyTable[188] = _comma;       // Tecla [;] o [,]
_keyTable[190] = _point;       // Tecla [:] o [.]
_keyTable[189] = _slash;       // Tecla [_] o [-]
_keyTable[16] = _r_shift;      // [SHIFT] o mayúsculas derecho
_keyTable[17] = _control;      // Teclas [CONTROL]
_keyTable[18] = _alt;          // Tecla [ALT] o [ALT GR]
_keyTable[32] = _space;        // [SPACE] o barra espaciadora
_keyTable[45] = _ins;          // [INSERT] o insertar
_keyTable[36] = _home;         // [INICIO] o inicio de página
_keyTable[33] = _pgup;         // [RE PAG] o retroceso de página
_keyTable[46] = _del;          // [SUPR] o suprimir
_keyTable[35] = _end;          // [FIN] o fin de página
_keyTable[34] = _pgdn;         // [AV PAG] o avance de página
_keyTable[38] = _up;           // Cursor para arriba
_keyTable[40] = _down;         // Cursor para abajo
_keyTable[37] = _left;         // Cursor para izquierda
_keyTable[39] = _right;        // Cursor para derecha
_keyTable[144] = _num_lock;    // [BLOQ NUM] o bloqueo numérico
_keyTable[111] = _c_backslash; // Símbolo [/] del teclado numérico
_keyTable[106] = _c_asterisk;  // Símbolo [*] del teclado numérico
_keyTable[109] = _c_minus;     // Símbolo [-] del teclado numérico
_keyTable[103] = _c_home;      // [INICIO] del teclado numérico
_keyTable[104] = _c_up;        // Cursor arriba del teclado numérico
_keyTable[105] = _c_pgup;      // [RE PAG] del teclado numérico
_keyTable[100] = _c_left;      // Cursor izquierda del teclado numérico
_keyTable[101] = _c_center;    // Tecla [5] del teclado numérico
_keyTable[102] = _c_right;     // Cursor derecha del teclado numérico
_keyTable[97] = _c_end;        // [FIN] del teclado numérico
_keyTable[98] = _c_down;       // Cursor abajo del teclado numérico
_keyTable[99] = _c_pgdn;       // [AV PAG] del teclado numérico
_keyTable[96] = _c_ins;        // [INS] del teclado numérico
_keyTable[110] = _c_del;       // [SUPR] del teclado numérico
_keyTable[107] = _c_plus;      // Símbolo [+] del teclado numérico
_keyTable[13] = _c_enter;      // [ENTER] del teclado numérico

var _gpKeyUp = function(e) {
    if(e.target != _gp.CScreen._canvas) { return false; }
    var key = e.which || e.keyCode;
    var rv = -1;

    if(key == 16 && e.location == 1)
        rv = _l_shift;
    else if(key == 16 && e.location == 2)
        rv = _r_shift;
    else if(key == 13 && e.location == 0)
        rv = _enter;
    else if(key == 13 && e.location == 3)
        rv = _c_enter;
    else
        rv = _keyTable[key];

    _gp.CKeyboard.keys[rv] = false;
}

var _gpKeyDown = function(e) {
    if(e.target != _gp.CScreen._canvas) { return false; }
    var key = e.which || e.keyCode;
    var rv = -1;

    if(key == 16 && e.location == 1)
        rv = _l_shift;
    else if(key == 16 && e.location == 2)
        rv = _r_shift;
    else if(key == 13 && e.location == 0)
        rv = _enter;
    else if(key == 13 && e.location == 3)
        rv = _c_enter;
    else
        rv = _keyTable[key];

    _gp.CKeyboard.keys[rv] = true;
};

var GPKeyboard = Class.extend({

    _init: function() {
        this.keys = [];
        this.Init();
    },

    Init: function() {

        document.removeEventListener("keyup", _gpKeyUp);
        document.removeEventListener("keydown", _gpKeyDown);
        document.addEventListener("keyup", _gpKeyUp, false);
        document.addEventListener("keydown", _gpKeyDown, false);

    }

});
