using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using vorpcore_cl.Utils;
using CitizenFX.Core.Native;

namespace vorpcore_cl.Scripts
{
    public class DiscRichPresence : BaseScript
    {
        public static bool drp_active = false;

        //public DiscRichPresence()
        //{
        //    Tick += setPresence;
        //}

        //[Tick]
        //public async Task setPresence()
        //{
        //    await Delay(60000);
        //    if (drp_active)
        //    {
        //        //Debug.WriteLine(GetConfig.Config["DiscordAppId"].ToString());
        //        //SetDiscordAppId(GetConfig.Config["DiscordAppId"].ToString());
        //        //SetRichPresence(GetConfig.Config["RichPresenceText"].ToString());
        //        //SetDiscordRichPresenceAsset(GetConfig.Config["DiscordRichPresenceAsset"].ToString());
        //        //SetDiscordRichPresenceAssetText(GetConfig.Config["DiscordRichPresenceAssetText"].ToString());
        //        //SetDiscordRichPresenceAssetSmall(GetConfig.Config["DiscordRichPresenceAssetSmall"].ToString());
        //        //SetDiscordRichPresenceAssetSmallText(GetConfig.Config["DiscordRichPresenceAssetSmallText"].ToString());
        //    }
        //}

    }
}
