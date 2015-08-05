using System;
using System.Windows.Forms;

namespace FPG2GPK
{
    static class Program
    {
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
                Converter.BatchConvert(args);
            else
                Application.Run(new frmMain());
        }
    }
}
