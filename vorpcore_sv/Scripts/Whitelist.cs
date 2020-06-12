using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vorpcore_sv.Utils;

namespace vorpcore_sv.Scripts
{
    class Whitelist : BaseScript
    {
        public static bool whitelistActive;

        public static List<string> whitelist = new List<string>();

        public Whitelist()
        {
            EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(OnPlayerConnecting);
            LoadWhitelist();
        }

        public async Task LoadWhitelist()
        {
            Exports["ghmattimysql"].execute("SELECT * FROM whitelist", new[] { "" }, new Action<dynamic>((result) =>
            {
                if (result.Count > 0)
                {
                    foreach (var r in result)
                    {
                        whitelist.Add(r.identifier);
                    }
                }

            }));
        }

        private async void OnPlayerConnecting([FromSource]Player player, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            deferrals.defer();

            await Delay(0);

            foreach (var r in whitelist)
            {
                Debug.WriteLine(r);
            }

            var steamIdentifier = player.Identifiers["steam"];

            if (steamIdentifier == null)
            {
                deferrals.done(LoadConfig.Langs["NoSteam"]);
                setKickReason(LoadConfig.Langs["NoSteam"]);
            }

            if (whitelistActive)
            {
                if (whitelist.Contains(steamIdentifier))
                {
                    deferrals.done();
                }
                else
                {
                    deferrals.done(LoadConfig.Langs["NoInWhitelist"]);
                    setKickReason(LoadConfig.Langs["NoInWhitelist"]);
                }
            }
            else
            {
                deferrals.done();
            }


            await Delay(0);


            Debug.WriteLine($"{playerName} is connecting with (Identifier: [{steamIdentifier}])");
        }
    }
}
