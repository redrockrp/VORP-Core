using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace vorpcore_sv.Utils
{
    class Database : BaseScript
    {
        public Database()
        {
            EventHandlers["vorp:getCharacter"] += new Action<int, dynamic>(getCharacter);
            EventHandlers["vorp:addMoney"] += new Action<int, int, int>(addMoney);
            EventHandlers["vorp:removeMoney"] += new Action<int, int, int>(removeMoney);

            EventHandlers["vorp:addXp"] += new Action<int, int>(addXp);
            EventHandlers["vorp:removeXp"] += new Action<int, int>(removeXp);
        }

        public static Player getSource(int handle)
        {
            PlayerList pl = new PlayerList();
            Player p = pl[handle];
            return p;
        }

        private void removeMoney(int handle, int typeCash, int quanty)
        {
            Player player = getSource(handle);

            string sid = ("steam:" + player.Identifiers["steam"]);
            string Cash = "money"; // default is money (0 is money, 1 is gold)
            switch (typeCash)
            {
                case 0:
                    Cash = "money";
                    break;
                case 1:
                    Cash = "gold";
                    break;
                case 2:
                    Cash = "rol";
                    break;
            }
            
            Exports["ghmattimysql"].execute($"UPDATE characters SET {Cash}={Cash} - {quanty} WHERE identifier=?", new[] { sid });
            
            Debug.WriteLine($"Removed {quanty} of {Cash} to {player.Name}");

            // Send Nui Update UI
            TriggerEvent("vorp:getCharacter", handle, new Action<dynamic>((user) =>
            {

                // Send Nui Update UI all
                JsonUiCalls JUC = new JsonUiCalls()
                {
                    type = "ui",
                    action = "update",
                    moneyquanty = user.money,
                    goldquanty = user.gold,
                    rolquanty = user.rol
                };

                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(JsonUiCalls));
                MemoryStream msObj = new MemoryStream();
                js.WriteObject(msObj, JUC);
                msObj.Position = 0;
                StreamReader sr = new StreamReader(msObj);

                string strjson = sr.ReadToEnd();

                player.TriggerEvent("vorp:updateUi", strjson);

            }));

        }

        private void addMoney(int handle, int typeCash, int quanty)
        {
            Player player = getSource(handle);

            string sid = ("steam:" + player.Identifiers["steam"]);
            string Cash = "money"; // default is money (0 is money, 1 is gold)
            switch (typeCash)
            {
                case 0:
                    Cash = "money";
                    break;
                case 1:
                    Cash = "gold";
                    break;
                case 2:
                    Cash = "rol";
                    break;
            }

            Exports["ghmattimysql"].execute($"UPDATE characters SET {Cash} = {Cash} + {quanty} WHERE identifier=?", new[] { sid });

            Debug.WriteLine($"Added {quanty} of {Cash} to {player.Name}");

            // Send Nui Update UI
            TriggerEvent("vorp:getCharacter", handle, new Action<dynamic>((user) =>
            {

                // Send Nui Update UI all
                JsonUiCalls JUC = new JsonUiCalls()
                {
                    type = "ui",
                    action = "update",
                    moneyquanty = user.money,
                    goldquanty = user.gold,
                    rolquanty = user.rol
                };

                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(JsonUiCalls));
                MemoryStream msObj = new MemoryStream();
                js.WriteObject(msObj, JUC);
                msObj.Position = 0;
                StreamReader sr = new StreamReader(msObj);

                string strjson = sr.ReadToEnd();

                player.TriggerEvent("vorp:updateUi", strjson);
                
            }));
        }

        private void addXp(int handle, int quanty)
        {
            Player player = getSource(handle);

            string sid = ("steam:" + player.Identifiers["steam"]);

            Exports["ghmattimysql"].execute($"UPDATE characters SET xp = xp + {quanty} WHERE identifier=?", new[] { sid });

            Debug.WriteLine($"Added {quanty} of Xp to {player.Name}");

            // Send Nui Update UI
           
        }

        private void removeXp(int handle, int quanty)
        {
            Player player = getSource(handle);

            string sid = ("steam:" + player.Identifiers["steam"]);

            Exports["ghmattimysql"].execute($"UPDATE characters SET xp = xp - {quanty} WHERE identifier=?", new[] { sid });

            Debug.WriteLine($"Removed {quanty} of Xp to {player.Name}");

            // Send Nui Update UI

        }

        private void getCharacter(int handle, dynamic cb)
        {
            Player player = getSource(handle);

            string sid = ("steam:" + player.Identifiers["steam"]);
            Debug.WriteLine(sid);

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
                    user.Add("money", result[0].money);
                    user.Add("gold", result[0].gold);
                    user.Add("rol", result[0].rol);
                    user.Add("xp", result[0].xp);
                    user.Add("firstname", result[0].firstname);
                    user.Add("lastname", result[0].lastname);

                    cb.Invoke(user); //Enviamos los datos de vuelta
                }

            }));
        }
    }
}
