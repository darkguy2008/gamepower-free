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

// https://gist.github.com/borismus/1032746
var BASE64_MARKER = ';base64,';
function convertDataURIToBinary(dataURI) {
    var base64Index = dataURI.indexOf(BASE64_MARKER) + BASE64_MARKER.length;
    var base64 = dataURI.substring(base64Index);
    return Base64ToArrayBuffer(base64);
}

function ReadPNG(b64)
{
    function getFourCC(int) {
        var c = String.fromCharCode;
        return c((int & 0xff000000) >>> 24) + c((int & 0xff0000) >>> 16) + c((int & 0xff00) >>> 8) + c((int & 0xff) >>> 0);
    }

    var buffer = convertDataURIToBinary(b64);
    var chunks = [];
    var pos = 0;
    var len = buffer.byteLength;
    var view = new DataView(buffer);
    var fourCC;
    var offset;
    var size;
    var ctc;

    var m1 = view.getUint32(pos); pos += 4;
    var m2 = view.getUint32(pos); pos += 4;

    if (m1 === 0x89504E47 && m2 === 0x0D0A1A0A)
    {
        while (pos < len)
        {
            // chunk header
            size = view.getUint32(pos);
            fourCC = getFourCC(view.getUint32(pos + 4));

            // data offset
            offset = pos + 8;
            pos = offset + size;

            // crc
            crc = view.getUint32(pos);
            pos += 4;

            // store chunk
            var c = {
                fourCC: fourCC,
                size: size,
                offset: offset,
                crc: crc,
                data: null
            };
            if(fourCC == "tEXt")
                c.data = view.getUTF8String(offset, size);

            chunks.push(c);
        }
    }
    else
        console.log("Error: Not a PNG file")

    return chunks;
}
