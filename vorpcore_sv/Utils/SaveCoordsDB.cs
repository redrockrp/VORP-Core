using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace vorpcore_sv.Utils
{
    public class SaveCoordsDB : BaseScript
    {
        public static Dictionary<Player, Tuple<Vector3, float>> LastCoordsInCache = new Dictionary<Player, Tuple<Vector3, float>>();
        public SaveCoordsDB()
        {
            EventHandlers["vorp:saveLastCoords"] += new Action<Player, Vector3, float>(SaveLastCoords);

            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);

            Tick += saveLastCoordsTick;
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

                Debug.WriteLine(pos);

                Exports["ghmattimysql"].execute("UPDATE characters SET coords=? WHERE identifier=?", new[] { pos, sid });

                LastCoordsInCache.Remove(player);
            }
            catch
            {
                Debug.WriteLine($"Se ha intentado guadar las coordenadas de {player.Name}");
            }
        }

        private void SaveLastCoords([FromSource] Player source, Vector3 lastCoords, float lastHeading)
        {
            LastCoordsInCache[source] = new Tuple<Vector3, float>(lastCoords, lastHeading);
        }

        [Tick]
        private async Task saveLastCoordsTick()
        {
            await Delay(30000);
            foreach (var source in LastCoordsInCache) 
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

                    Debug.WriteLine(pos);

                    Exports["ghmattimysql"].execute("UPDATE characters SET coords=? WHERE identifier=?", new[] { pos, sid });
                }
                catch { continue; }
            }

        }

    }
}
