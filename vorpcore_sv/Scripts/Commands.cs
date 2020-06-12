using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using vorpcore_sv.Utils;

namespace vorpcore_sv.Scripts
{
    class Commands : BaseScript
    {
        public Commands()
        {

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
    }
}
