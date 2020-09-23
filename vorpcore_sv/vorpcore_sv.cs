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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(@"" + "\n" +
                            @" /$$    /$$  /$$$$$$  /$$$$$$$  /$$$$$$$   /$$$$$$    /$$ /$$   /$$$$$$$  /$$$$$$$$" + "\n" +
                            @"| $$   | $$ /$$__  $$| $$__  $$| $$__  $$ /$$__  $$  / $$/ $$  | $$__  $$| $$_____/" + "\n" +
                            @"| $$   | $$| $$  \ $$| $$  \ $$| $$  \ $$| $$  \__/ /$$$$$$$$$$| $$  \ $$| $$      " + "\n" +
                            @"|  $$ / $$/| $$  | $$| $$$$$$$/| $$$$$$$/| $$      |   $$  $$_/| $$$$$$$/| $$$$$   " + "\n" +
                            @" \  $$ $$/ | $$  | $$| $$__  $$| $$____/ | $$       /$$$$$$$$$$| $$__  $$| $$__/   " + "\n" +
                            @"  \  $$$/  | $$  | $$| $$  \ $$| $$      | $$    $$|_  $$  $$_/| $$  \ $$| $$      " + "\n" +
                            @"   \  $/   |  $$$$$$/| $$  | $$| $$      |  $$$$$$/  | $$| $$  | $$  | $$| $$$$$$$$" + "\n" +
                            @"    \_/     \______/ |__/  |__/|__/       \______/   |__/|__/  |__/  |__/|________/" + "\n" +
                            "");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

}
