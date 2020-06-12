using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using vorpcore_sv.Utils;

namespace vorpcore_sv.Scripts
{
    class Whitelist : BaseScript
    {
        public Whitelist()
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
                deferrals.done(LoadConfig.Langs["NoSteam"]);
                setKickReason(LoadConfig.Langs[""]);
            }

            deferrals.done();
        }
    }
}
