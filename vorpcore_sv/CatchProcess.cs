using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using System.Runtime.InteropServices;
using System.Threading;
using vorpcore_sv.Utils;

namespace vorpcore_sv
{
    class CatchProcess : BaseScript
    {
        public CatchProcess()
        {
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

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
            Debug.WriteLine(@"" + "\n" +
                @" /$$    /$$  /$$$$$$  /$$$$$$$  /$$$$$$$   /$$$$$$   /$$$$$$  /$$$$$$$  /$$$$$$$$" + "\n" +
                @"| $$   | $$ /$$__  $$| $$__  $$| $$__  $$ /$$__  $$ /$$__  $$| $$__  $$| $$_____/" + "\n" +
                @"| $$   | $$| $$  \ $$| $$  \ $$| $$  \ $$| $$  \__/| $$  \ $$| $$  \ $$| $$      " + "\n" +
                @"|  $$ / $$/| $$  | $$| $$$$$$$/| $$$$$$$/| $$      | $$  | $$| $$$$$$$/| $$$$$   " + "\n" +
                @" \  $$ $$/ | $$  | $$| $$__  $$| $$____/ | $$      | $$  | $$| $$__  $$| $$__/   " + "\n" +
                @"  \  $$$/  | $$  | $$| $$  \ $$| $$      | $$    $$| $$  | $$| $$  \ $$| $$      " + "\n" +
                @"   \  $/   |  $$$$$$/| $$  | $$| $$      |  $$$$$$/|  $$$$$$/| $$  | $$| $$$$$$$$" + "\n" +
                @"    \_/     \______/ |__/  |__/|__/       \______/  \______/ |__/  |__/|________/" + "\n" +
                @"                                                                                 " + "\n" +
                "Clearing & Saving some functions...");
            Thread.Sleep(1000);
            Debug.WriteLine("Thanks for using VORP Core <3");
            Thread.Sleep(2000);
            return false;
        }

 

    }
}
