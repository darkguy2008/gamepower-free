namespace Littera2GPK
{

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class font
    {
        private fontInfo infoField;

        private fontCommon commonField;

        private fontPage[] pagesField;

        private fontChars charsField;

        private fontKernings kerningsField;

        /// <remarks/>
        public fontInfo info
        {
            get
            {
                return this.infoField;
            }
            set
            {
                this.infoField = value;
            }
        }

        /// <remarks/>
        public fontCommon common
        {
            get
            {
                return this.commonField;
            }
            set
            {
                this.commonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("page", IsNullable = false)]
        public fontPage[] pages
        {
            get
            {
                return this.pagesField;
            }
            set
            {
                this.pagesField = value;
            }
        }

        /// <remarks/>
        public fontChars chars
        {
            get
            {
                return this.charsField;
            }
            set
            {
                this.charsField = value;
            }
        }

        /// <remarks/>
        public fontKernings kernings
        {
            get
            {
                return this.kerningsField;
            }
            set
            {
                this.kerningsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class fontInfo
    {

        private string faceField;

        private byte sizeField;

        private byte boldField;

        private byte italicField;

        private string charsetField;

        private string unicodeField;

        private byte stretchHField;

        private byte smoothField;

        private byte aaField;

        private string paddingField;

        private string spacingField;

        private byte outlineField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string face
        {
            get
            {
                return this.faceField;
            }
            set
            {
                this.faceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte bold
        {
            get
            {
                return this.boldField;
            }
            set
            {
                this.boldField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte italic
        {
            get
            {
                return this.italicField;
            }
            set
            {
                this.italicField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string charset
        {
            get
            {
                return this.charsetField;
            }
            set
            {
                this.charsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string unicode
        {
            get
            {
                return this.unicodeField;
            }
            set
            {
                this.unicodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte stretchH
        {
            get
            {
                return this.stretchHField;
            }
            set
            {
                this.stretchHField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte smooth
        {
            get
            {
                return this.smoothField;
            }
            set
            {
                this.smoothField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte aa
        {
            get
            {
                return this.aaField;
            }
            set
            {
                this.aaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string padding
        {
            get
            {
                return this.paddingField;
            }
            set
            {
                this.paddingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string spacing
        {
            get
            {
                return this.spacingField;
            }
            set
            {
                this.spacingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte outline
        {
            get
            {
                return this.outlineField;
            }
            set
            {
                this.outlineField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class fontCommon
    {

        private byte lineHeightField;

        private byte baseField;

        private ushort scaleWField;

        private ushort scaleHField;

        private byte pagesField;

        private byte packedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte lineHeight
        {
            get
            {
                return this.lineHeightField;
            }
            set
            {
                this.lineHeightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte @base
        {
            get
            {
                return this.baseField;
            }
            set
            {
                this.baseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort scaleW
        {
            get
            {
                return this.scaleWField;
            }
            set
            {
                this.scaleWField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort scaleH
        {
            get
            {
                return this.scaleHField;
            }
            set
            {
                this.scaleHField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte pages
        {
            get
            {
                return this.pagesField;
            }
            set
            {
                this.pagesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte packed
        {
            get
            {
                return this.packedField;
            }
            set
            {
                this.packedField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class fontPage
    {

        private byte idField;

        private string fileField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string file
        {
            get
            {
                return this.fileField;
            }
            set
            {
                this.fileField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class fontChars
    {

        private fontCharsChar[] charField;

        private int countField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("char")]
        public fontCharsChar[] @char
        {
            get
            {
                return this.charField;
            }
            set
            {
                this.charField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class fontCharsChar
    {

        private ushort idField;

        private ushort xField;

        private ushort yField;

        private byte widthField;

        private byte heightField;

        private sbyte xoffsetField;

        private byte yoffsetField;

        private byte xadvanceField;

        private byte pageField;

        private byte chnlField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public sbyte xoffset
        {
            get
            {
                return this.xoffsetField;
            }
            set
            {
                this.xoffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte yoffset
        {
            get
            {
                return this.yoffsetField;
            }
            set
            {
                this.yoffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte xadvance
        {
            get
            {
                return this.xadvanceField;
            }
            set
            {
                this.xadvanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte page
        {
            get
            {
                return this.pageField;
            }
            set
            {
                this.pageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte chnl
        {
            get
            {
                return this.chnlField;
            }
            set
            {
                this.chnlField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class fontKernings
    {

        private fontKerningsKerning[] kerningField;

        private int countField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("kerning")]
        public fontKerningsKerning[] kerning
        {
            get
            {
                return this.kerningField;
            }
            set
            {
                this.kerningField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class fontKerningsKerning
    {

        private byte firstField;

        private byte secondField;

        private sbyte amountField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte first
        {
            get
            {
                return this.firstField;
            }
            set
            {
                this.firstField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte second
        {
            get
            {
                return this.secondField;
            }
            set
            {
                this.secondField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public sbyte amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }
    }


}
