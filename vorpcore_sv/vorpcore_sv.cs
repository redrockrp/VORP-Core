using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
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

            EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(OnPlayerConnecting);
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
