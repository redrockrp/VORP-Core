using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using vorpcore_sv.Scripts;
using vorpcore_sv.Utils;
using CitizenFX.Core;

namespace vorpcore_sv
{
    public class vorpcore_sv : BaseScript
    {
        public vorpcore_sv()
        {
            EventHandlers["callbackdelegate"] += new Action<NetworkCallbackDelegate>(Event);
            // In class constructor
            Debug.WriteLine(@"" + "\n" +
                            @" /$$    /$$  /$$$$$$  /$$$$$$$  /$$$$$$$   /$$$$$$   /$$$$$$  /$$$$$$$  /$$$$$$$$ /$$    /$$ |" + "\n" +
                            @"| $$   | $$ /$$__  $$| $$__  $$| $$__  $$ /$$__  $$ /$$__  $$| $$__  $$| $$_____/ | $$   | $$|" + "\n" +
                            @"| $$   | $$| $$  \ $$| $$  \ $$| $$  \ $$| $$  \__/| $$  \ $$| $$  \ $$| $$       | $$   | $$|" + "\n" +
                            @"|  $$ / $$/| $$  | $$| $$$$$$$/| $$$$$$$/| $$      | $$  | $$| $$$$$$$/| $$$$$    |  $$ / $$/ " + "\n" +
                            @" \  $$ $$/ | $$  | $$| $$__  $$| $$____/ | $$      | $$  | $$| $$__  $$| $$__/    \  $$ $$ /  " + "\n" +
                            @"  \  $$$/  | $$  | $$| $$  \ $$| $$      | $$    $$| $$  | $$| $$  \ $$| $$        \  $$$/    " + "\n" +
                            @"   \  $/   |  $$$$$$/| $$  | $$| $$      |  $$$$$$/|  $$$$$$/| $$  | $$| $$$$$$$$   \  $/     " + "\n" +
                            @"    \_/     \______/ |__/  |__/|__/       \______/  \______/ |__/  |__/|________/    \_/      " + "\n" +
                            @"                                                                                              " + "\n" +
                            "");
            
        }
        

        private void Event(NetworkCallbackDelegate cb)
        {
            Debug.WriteLine("Ejecutando callback");
            cb.Invoke("Hola");
        }
    }

}
