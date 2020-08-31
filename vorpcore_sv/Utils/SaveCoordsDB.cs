using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vorpcore_sv.Scripts;

namespace vorpcore_sv.Utils
{
    public class SaveCoordsDB : BaseScript
    {
        public static Dictionary<Player, Tuple<Vector3, float>> LastCoordsInCache = new Dictionary<Player, Tuple<Vector3, float>>();
        public SaveCoordsDB()
        {
            EventHandlers["vorp:saveLastCoords"] += new Action<Player, Vector3, float>(SaveLastCoords);

            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);

            EventHandlers["vorp:ImDead"] += new Action<Player, bool>(OnPlayerDead);

            Tick += saveLastCoordsTick;
        }

        private void OnPlayerDead([FromSource]Player player, bool isDead)
        {
            string sid = ("steam:" + player.Identifiers["steam"]);

            if (LoadUsers._users.ContainsKey(sid))
            {
                LoadUsers._users[sid].GetUsedCharacter().setDead(isDead);
            }
        }

        private void OnPlayerDropped([FromSource]Player player, string reason)
        {
            string sid = ("steam:" + player.Identifiers["steam"]);

            

            try
            {
                Vector3 lastCoords = LastCoordsInCache[player].Item1;
                float lastHeading = LastCoordsInCache[player].Item2;

                UPlayerCoords UPC = new UPlayerCoords()
                {
                    x = lastCoords.X,
                    y = lastCoords.Y,
                    z = lastCoords.Z,
                    heading = lastHeading
                };

                string pos = JsonConvert.SerializeObject(UPC);

                Exports["ghmattimysql"].execute("UPDATE characters SET `coords` = ? WHERE `identifier` = ? AND `charidentifier` = ?", new[] { pos, sid,LoadUsers._users[sid].GetUsedCharacter().Identifier });

                LastCoordsInCache.Remove(player);
                LoadUsers._users.Remove(sid);
            }
            catch
            {

            }
        }

        private void SaveLastCoords([FromSource] Player source, Vector3 lastCoords, float lastHeading)
        {
            LastCoordsInCache[source] = new Tuple<Vector3, float>(lastCoords, lastHeading);
        }

        [Tick]
        private async Task saveLastCoordsTick()
        {
            await Delay(300000);
            Dictionary<Player, Tuple<Vector3, float>> lastCoordToSave = LastCoordsInCache.ToDictionary(p => p.Key, p => p.Value);
            foreach (var source in lastCoordToSave)
            {
                string sid = ("steam:" + source.Key.Identifiers["steam"]);
                try
                {
                    Vector3 lastCoords = source.Value.Item1;
                    float lastHeading = source.Value.Item2;

                    UPlayerCoords UPC = new UPlayerCoords()
                    {
                        x = lastCoords.X,
                        y = lastCoords.Y,
                        z = lastCoords.Z,
                        heading = lastHeading
                    };

                    string pos = JsonConvert.SerializeObject(UPC);

                    Exports["ghmattimysql"].execute("UPDATE characters SET `coords`= ? WHERE `identifier`= ?", new[] { pos, sid });
                }
                catch { continue; }
            }
            await Delay(1000);
        }



    }
}
