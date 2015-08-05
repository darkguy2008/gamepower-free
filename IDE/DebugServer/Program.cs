using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DebugServer
{
    class Program
    {
        static string ApplicationPath { get { return new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName + "\\"; } }
        static bool appExit = false;
        static NotifyIcon appTray;

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();        

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern long SetForegroundWindow(IntPtr hwnd);

        private static Int32 showWindow = 0;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            Shutdown();
            return true;
        }

        private static void Shutdown()
        {
            appTray.Visible = false;
            if (File.Exists(ApplicationPath + "kill"))
                File.Delete(ApplicationPath + "kill");
            if (File.Exists(ApplicationPath + "refresh"))
                File.Delete(ApplicationPath + "refresh");
            Console.WriteLine("Exiting system due to external CTRL-C, or process kill, or shutdown");
            myServer.Stop();
            appExit = true;
            Environment.Exit(0);
        }

        private static void ThreadKill()
        {
            while(!appExit)
            {
                Thread.Sleep(1000);
                if(File.Exists(ApplicationPath + "kill"))
                {
                    File.Delete(ApplicationPath + "kill");
                    Shutdown();
                }
            }
        }

        private static void ThreadReset()
        {
            while (!appExit)
            {
                Thread.Sleep(500);
                if (File.Exists(ApplicationPath + "refresh"))
                {
                    String newPath = File.ReadAllLines(ApplicationPath + "refresh")[0].Trim();
                    File.Delete(ApplicationPath + "refresh");
                    RefreshServer(newPath, HttpPort);
                }
            }
        }

        private static void RefreshServer(String newPath, int port)
        {
            if (myServer != null)
            {
                Console.Write("Stopping SimpleHTTPServer... ");
                myServer.Stop();
                Console.WriteLine("Stopped.");
            }
            myServer = new SimpleHTTPServer(newPath, port);
            Console.WriteLine("SimpleHTTPServer started at http://127.0.0.1:" + port + " serving " + newPath);
        }

        private static int HttpPort = 0;
        private static Thread thKill;
        private static Thread thRefresh;
        private static SimpleHTTPServer myServer;
        static void Main(string[] args)
        {
            appTray = new NotifyIcon();
            appTray.Icon = Icon.FromHandle(Properties.Resources.icnCog.Handle);
            appTray.Visible = true;
            appTray.MouseClick += AppTray_MouseClick;
            ShowWindow(ThisConsole, showWindow);

            appTray.ShowBalloonTip(5000, "GamePower Debug Server", "GamePower Debug Server started", ToolTipIcon.Info);

            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            thKill = new Thread(new ThreadStart(ThreadKill));
            thKill.Start();

            thRefresh = new Thread(new ThreadStart(ThreadReset));
            thRefresh.Start();

            HttpPort = int.Parse(args[1]);
            RefreshServer(args[0], HttpPort);

            Application.Run();
        }

        private static void AppTray_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                showWindow = ++showWindow % 2;
                ShowWindow(ThisConsole, showWindow);
                SetForegroundWindow(ThisConsole);
            }
        }
    }
}
