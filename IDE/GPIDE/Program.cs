using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace GPIDE
{
    static class Program
    {
        public static string ApplicationPath { get { return new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName + "\\"; } }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
