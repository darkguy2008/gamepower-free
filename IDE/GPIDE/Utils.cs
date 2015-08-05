using FastColoredTextBoxNS;
using System.Drawing;

namespace GPIDE
{

    public class FCTBThemes
    {
        // http://stackoverflow.com/questions/14796162/how-to-install-a-windows-font-using-c-sharp/26284643#26284643
        // [DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
        // public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)] string lpFileName);
        // Doesn't work, locks up the TTF file somehow.

        public enum ECaretStyle {
            Normal = 1,
            Wide = 2,
            MSDOS = 3
        }

        public class FCTBTheme
        {
            public Font Font { get; set; }
            public Color Background { get; set; }
            public Color Foreground { get; set; }
            public Color ServiceLines { get; set; }
            public Color Services { get; set; }
            public Color Caret { get; set; }
            public Color LineNumbers { get; set; }
            public Color IndentBackground { get; set; }
            public Color ColorKeywords { get; set; }
            public Color ColorComments { get; set; }
            public Color ColorStrings { get; set; }
            public Color ColorNumbers { get; set; }
            public ECaretStyle CaretStyle { get; set; }
            public bool CaretBlinking { get; set; }
            public Color CaretColor { get; set; }
            public TextStyle StyleKeywords { get; set; }
            public TextStyle StyleComments { get; set; }
            public TextStyle StyleStrings { get; set; }
            public TextStyle StyleNumbers { get; set; }
        }

        public static FCTBTheme ThemeDIV2 = new FCTBTheme()
        {
            Font = new Font("Consolas", 8.0f),
            Foreground = Color.FromArgb(150, 154, 170),
            Background = Color.FromArgb(16, 20, 121),
            ServiceLines = Color.FromArgb(81, 89, 178),
            Caret = Color.FromArgb(219, 223, 247),
            LineNumbers = Color.FromArgb(81, 89, 178),

            ColorKeywords = Color.FromArgb(219, 223, 247),
            ColorComments = Color.FromArgb(81, 89, 178),
            ColorStrings = Color.FromArgb(150, 154, 170),
            ColorNumbers = Color.FromArgb(150, 154, 170),

            StyleKeywords = new TextStyle(new SolidBrush(Color.FromArgb(219, 223, 247)), null, FontStyle.Bold),
            StyleComments = new TextStyle(new SolidBrush(Color.FromArgb(81, 89, 178)), null, FontStyle.Regular),
            StyleStrings = new TextStyle(new SolidBrush(Color.FromArgb(150, 154, 170)), null, FontStyle.Regular),
            StyleNumbers = new TextStyle(new SolidBrush(Color.FromArgb(150, 154, 170)), null, FontStyle.Regular),

            CaretBlinking = true,            
            CaretColor = Color.DarkGray,
            CaretStyle = ECaretStyle.MSDOS
        };

        public static void SetTheme(FCTBTheme t, FastColoredTextBox f)
        {
            f.BackColor = t.Background;
            f.ForeColor = t.Foreground;
            f.IndentBackColor = t.IndentBackground;
            f.ServiceLinesColor = t.ServiceLines;
            f.LineNumberColor = t.LineNumbers;

            f.CaretBlinking = t.CaretBlinking;
            f.CaretColor = t.CaretColor;

            f.AutoIndentChars = false;

            //int result = AddFontResource(Program.ApplicationPath + "FSEX300.ttf");
            //int error = Marshal.GetLastWin32Error();

            //if(error == 0)
                f.Font = new Font("Fixedsys Excelsior 3.01", 16, FontStyle.Regular, GraphicsUnit.Pixel);
            //else
            //    f.Font = new Font("Consolas", 12, FontStyle.Regular, GraphicsUnit.Pixel);

            switch(t.CaretStyle) {
                case ECaretStyle.Normal:
                    f.WideCaret = false;
                    f.SmallCaret = false;
                    break;
                case ECaretStyle.Wide:
                    f.WideCaret = true;
                    f.SmallCaret = false;
                    break;
                case ECaretStyle.MSDOS:
                    f.WideCaret = true;
                    f.SmallCaret = true;
                    break;
            }

        }

    }

}
