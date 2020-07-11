using CitizenFX.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using vorpcore_sv.Class;
using vorpcore_sv.Utils;

namespace vorpcore_sv.Scripts
{
    public class LoadCharacter : BaseScript
    {
        public static Dictionary<string, Character> characters = new Dictionary<string, Character>();

        public LoadCharacter()
        {
            EventHandlers["vorp:playerSpawn"] += new Action<Player>(PlayerSpawnFunction);
            EventHandlers["vorp:UpdateCharacter"] += new Action<string, string, string>(UpdateCharacter);
        }

        private void UpdateCharacter(string steamId, string firstname, string lastname)
        {
            characters[steamId].Firstname = firstname;
            characters[steamId].Lastname = lastname;
        }

        private void PlayerSpawnFunction([FromSource] Player source)
        {
            string sid = ("steam:" + source.Identifiers["steam"]);

            if (!characters.ContainsKey(sid))
            {
                characters[sid] = new Character();
                source.TriggerEvent("vorpcharacter:createPlayer");
            }
            else
            {

                JObject pos = JObject.Parse(characters[sid].Coords);
                if (pos.ContainsKey("x"))
                {
                    Vector3 pcoords = new Vector3(pos["x"].ToObject<float>(), pos["y"].ToObject<float>(), pos["z"].ToObject<float>());
                    source.TriggerEvent("vorp:initPlayer", pcoords, pos["heading"].ToObject<float>(), characters[sid].IsDead);
                }


                // Send Nui Update UI all
                JObject postUi = new JObject();
                postUi.Add("type", "ui");
                postUi.Add("action", "update");
                postUi.Add("moneyquanty", characters[sid].Money);
                postUi.Add("goldquanty", characters[sid].Gold);
                postUi.Add("rolquanty", characters[sid].Rol);
                postUi.Add("serverId", source.Handle);
                postUi.Add("xp", characters[sid].Xp);


                source.TriggerEvent("vorp:updateUi", postUi.ToString());
            }

        }

    }
}
