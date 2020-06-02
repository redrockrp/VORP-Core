using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using vorpcore_sv.Resources;
using vorpcore_sv.Utils;

namespace vorpcore_sv 
{
    public class vorpcore_sv : BaseScript
    {
        public vorpcore_sv()
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
                "");

            EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(OnPlayerConnecting);


            TriggerEvent("chat:addSuggestion", "/addmoney", "add money to user\n Example: /addmoney playerid moneytype quantity");
            API.RegisterCommand("addmoney", new Action<int, List<object>, string>((source, args, rawCommand) =>
            {
                if (source > 0) // it's a player.
                {
                    Player _source = Database.getSource(source);
                    TriggerEvent("vorp:getCharacter", source, new Action<dynamic>((user) =>
                    {
                        if (user.group == "admin")
                        {
                            try
                            {
                                int target = int.Parse(args[0].ToString());
                                int montype = int.Parse(args[1].ToString());
                                double quantity = double.Parse(args[2].ToString());

                                TriggerEvent("vorp:addMoney", target, montype, quantity);
                                _source.TriggerEvent("vorp:Tip", $"Added {quantity} to {target}", 4000);
                            }
                            catch
                            {
                                _source.TriggerEvent("vorp:Tip", "ERROR: Use Correct Sintaxis", 4000);
                            }
                        }
                        else
                        {
                            _source.TriggerEvent("vorp:Tip", "ERROR: You don't have enough permissions", 4000);
                        }
                    }));
                }
                else
                {
                    Debug.WriteLine("This only can be executed from client side.");
                }
            }), false);

            TriggerEvent("chat:addSuggestion", "/delmoney", "remove money to user\n Example: /delmoney playerid moneytype quantity");
            API.RegisterCommand("delmoney", new Action<int, List<object>, string>((source, args, rawCommand) =>
            {
                if (source > 0) //it's a player.
                {
                    Player _source = Database.getSource(source);
                    TriggerEvent("vorp:getCharacter", source, new Action<dynamic>((user) =>
                    {
                        if (user.group == "admin")
                        {
                            try
                            {
                                int target = int.Parse(args[0].ToString());
                                int montype = int.Parse(args[1].ToString());
                                double quantity = double.Parse(args[2].ToString());

                                TriggerEvent("vorp:removeMoney", target, montype, quantity);
                                _source.TriggerEvent("vorp:Tip", $"Removed {quantity} from {target}", 4000);
                            }
                            catch
                            {
                                _source.TriggerEvent("vorp:Tip", "ERROR: Use Correct Sintaxis", 4000);
                            }
                        }
                        else
                        {
                            _source.TriggerEvent("vorp:Tip", "ERROR: You don't have enough permissions", 4000);
                        }
                    }));
                }
                else
                {
                    Debug.WriteLine("This only can be executed from client side.");
                }
            }), false);



        }

        private async void OnPlayerConnecting([FromSource]Player player, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            deferrals.defer();

            await Delay(0);

            var steamIdentifier = player.Identifiers["steam"];

            Debug.WriteLine($"{playerName} esta conectando con (Identificador: [{steamIdentifier}])");

                        
            if (steamIdentifier == null)
            {
                deferrals.done(RStrings.console["errorNoSteam"]);
                setKickReason(RStrings.console["errorNoSteam"]);
            }

            deferrals.done();
        }

    }

}
