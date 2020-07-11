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

            Exports["ghmattimysql"].execute("SELECT * FROM characters WHERE identifier = ?", new[] { sid }, new Action<dynamic>((result) =>
            {
                if (result.Count == 0)
                {
                    characters[sid] = new Character();
                    source.TriggerEvent("vorpcharacter:createPlayer");
                }
                else
                {
                    characters[sid] = new Character(sid, result[0].group.ToString(), result[0].job.ToString(), result[0].jobgrade.ToString(), result[0].firstname.ToString(), result[0].lastname.ToString(), double.Parse(result[0].money.ToString()), double.Parse(result[0].gold.ToString()), double.Parse(result[0].rol.ToString()), int.Parse(result[0].xp.ToString()));
                    bool isdead = Boolean.Parse(result[0].isdead.ToString());
                    string last_coords = result[0].coords;

                    JObject pos = JObject.Parse(last_coords);
                    if (pos.ContainsKey("x"))
                    {
                        Vector3 pcoords = new Vector3(pos["x"].ToObject<float>(), pos["y"].ToObject<float>(), pos["z"].ToObject<float>());
                        source.TriggerEvent("vorp:initPlayer", pcoords, pos["heading"].ToObject<float>(), isdead);
                    }


                    // Send Nui Update UI all
                    JObject postUi = new JObject();
                    postUi.Add("type", "ui");
                    postUi.Add("action", "update");
                    postUi.Add("moneyquanty", result[0].money);
                    postUi.Add("goldquanty", result[0].gold);
                    postUi.Add("rolquanty", result[0].rol);
                    postUi.Add("serverId", source.Handle);
                    postUi.Add("xp", result[0].xp);


                    source.TriggerEvent("vorp:updateUi", postUi.ToString());
                }

            }));

        }

    }
}
