using System;
using System.Windows.Forms;

namespace GPKEditor
{
    static class Program
    {
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmMain frm = new frmMain();
            if (args.Length > 0)
                frm.FilenameToOpen = args[0];
            Application.Run(frm);
        }
    }
}
