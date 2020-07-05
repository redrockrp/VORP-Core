using CitizenFX.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace vorpcore_sv.Utils
{
    class Database : BaseScript
    {
        public Database()
        {
            EventHandlers["vorp:getCharacter"] += new Action<int, dynamic>(getCharacter);
            EventHandlers["vorp:addMoney"] += new Action<int, int, double>(addMoney);
            EventHandlers["vorp:removeMoney"] += new Action<int, int, double>(removeMoney);

            EventHandlers["vorp:addXp"] += new Action<int, int>(addXp);
            EventHandlers["vorp:removeXp"] += new Action<int, int>(removeXp);

            EventHandlers["vorp:setJob"] += new Action<int, string>(setJob);
            EventHandlers["vorp:setGroup"] += new Action<int, string>(setGroup);
        }

        public static Player getSource(int handle)
        {
            PlayerList pl = new PlayerList();
            Player p = pl[handle];
            return p;
        }

        private void removeMoney(int handle, int typeCash, double quanty)
        {
            TriggerEvent("vorp:getCharacter", handle, new Action<dynamic>((user) =>
            {
                Player player = getSource(handle);

                string sid = ("steam:" + player.Identifiers["steam"]);
                string Cash = "money"; // default is money (0 is money, 1 is gold)

                double lessMoney = user.money;
                double lessGold = user.gold;
                int lessRol = Convert.ToInt32(user.rol);

                switch (typeCash)
                {
                    case 0:
                        Cash = "money";
                        lessMoney -= quanty;
                        break;
                    case 1:
                        Cash = "gold";
                        lessGold -= quanty;
                        break;
                    case 2:
                        Cash = "rol";
                        lessRol -= Convert.ToInt32(quanty);
                        break;
                }

                Exports["ghmattimysql"].execute($"UPDATE characters SET {Cash}={Cash} - {quanty} WHERE identifier=?", new[] { sid });

                Debug.WriteLine($"Removed {quanty} of {Cash} to {player.Name}");

                JObject nuipost = new JObject();
                nuipost.Add("type", "ui");
                nuipost.Add("action", "update");
                nuipost.Add("moneyquanty", lessMoney);
                nuipost.Add("goldquanty", lessGold);
                nuipost.Add("rolquanty", lessRol);
                nuipost.Add("xp", user.xp);
                nuipost.Add("serverId", handle);

                player.TriggerEvent("vorp:updateUi", nuipost.ToString());

            }));

        }

        private void addMoney(int handle, int typeCash, double quanty)
        {
            TriggerEvent("vorp:getCharacter", handle, new Action<dynamic>((user) =>
            {
                Player player = getSource(handle);

                string sid = ("steam:" + player.Identifiers["steam"]);
                string Cash = "money"; // default is money (0 is money, 1 is gold)

                double lessMoney = user.money;
                double lessGold = user.gold;
                int lessRol = Convert.ToInt32(user.rol);

                switch (typeCash)
                {
                    case 0:
                        Cash = "money";
                        lessMoney += quanty;
                        break;
                    case 1:
                        Cash = "gold";
                        lessGold += quanty;
                        break;
                    case 2:
                        Cash = "rol";
                        lessRol += Convert.ToInt32(quanty);
                        break;
                }

                Exports["ghmattimysql"].execute($"UPDATE characters SET {Cash} = {Cash} + {quanty} WHERE identifier=?", new[] { sid });

                Debug.WriteLine($"Added {quanty} of {Cash} to {player.Name}");

                JObject nuipost = new JObject();
                nuipost.Add("type", "ui");
                nuipost.Add("action", "update");
                nuipost.Add("moneyquanty", lessMoney);
                nuipost.Add("goldquanty", lessGold);
                nuipost.Add("rolquanty", lessRol);
                nuipost.Add("xp", user.xp);
                nuipost.Add("serverId", handle);

                player.TriggerEvent("vorp:updateUi", nuipost.ToString());

            }));
        }

        private void addXp(int handle, int quanty)
        {
            TriggerEvent("vorp:getCharacter", handle, new Action<dynamic>((user) =>
            {
                Player player = getSource(handle);

                string sid = ("steam:" + player.Identifiers["steam"]);

                Exports["ghmattimysql"].execute($"UPDATE characters SET xp = xp + {quanty} WHERE identifier=?", new[] { sid });

                Debug.WriteLine($"Added {quanty} of Xp to {player.Name}");

                int totalxp = user.xp + quanty;

                // Send Nui Update UI
                JObject nuipost = new JObject();
                nuipost.Add("type", "ui");
                nuipost.Add("action", "setxp");
                nuipost.Add("xp", totalxp);

                player.TriggerEvent("vorp:updateUi", nuipost.ToString());
            }));
        }

        private void removeXp(int handle, int quanty)
        {
            TriggerEvent("vorp:getCharacter", handle, new Action<dynamic>((user) =>
            {
                Player player = getSource(handle);

                string sid = ("steam:" + player.Identifiers["steam"]);

                Exports["ghmattimysql"].execute($"UPDATE characters SET xp = xp - {quanty} WHERE identifier=?", new[] { sid });

                Debug.WriteLine($"Removed {quanty} of Xp to {player.Name}");
                int totalxp = user.xp - quanty;

                // Send Nui Update UI
                JObject nuipost = new JObject();
                nuipost.Add("type", "ui");
                nuipost.Add("action", "setxp");
                nuipost.Add("xp", totalxp);

                player.TriggerEvent("vorp:updateUi", nuipost.ToString());
            }));
        }

        private void setJob(int handle, string job)
        {
            TriggerEvent("vorp:getCharacter", handle, new Action<dynamic>((user) =>
            {
                Player player = getSource(handle);

                string sid = ("steam:" + player.Identifiers["steam"]);

                Exports["ghmattimysql"].execute($"UPDATE characters SET job = ? WHERE identifier=?", new[] { job, sid });
            }));
        }

        private void setGroup(int handle, string group)
        {
            TriggerEvent("vorp:getCharacter", handle, new Action<dynamic>((user) =>
            {
                Player player = getSource(handle);

                string sid = ("steam:" + player.Identifiers["steam"]);

                Exports["ghmattimysql"].execute($"UPDATE characters SET group = ? WHERE identifier=?", new[] { group, sid });
            }));
        }

        private void getCharacter(int handle, dynamic cb)
        {
            Player player = getSource(handle);

            string sid = ("steam:" + player.Identifiers["steam"]);

            Exports["ghmattimysql"].execute("SELECT * FROM characters WHERE identifier = ?", new[] { sid }, new Action<dynamic>((result) =>
            {
                if (result.Count == 0)
                {
                    Debug.WriteLine("ERROR: Usuario no registrado");
                }
                else
                {
                    Dictionary<string, object> user = new Dictionary<string, object>();

                    /* Seteamos todos los paraametros que nos puedan servir para comprobaciones*/
                    user.Add("identifier", sid);
                    user.Add("inventory", result[0].inventory);
                    user.Add("group", result[0].group);
                    user.Add("job", result[0].job);
                    user.Add("money", result[0].money);
                    user.Add("gold", result[0].gold);
                    user.Add("rol", result[0].rol);
                    user.Add("xp", result[0].xp);
                    user.Add("firstname", result[0].firstname);
                    user.Add("lastname", result[0].lastname);
                    user.Add("status", result[0].status);

                    cb.Invoke(user); //Enviamos los datos de vuelta
                }

            }));
        }
    }
}
