using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;

namespace vorpcore_cl.Utils
{
    public class GetConfig : BaseScript
    {
        public static JObject Config = new JObject();
        public static Dictionary<string, string> Langs = new Dictionary<string, string>();

        public GetConfig()
        {
            EventHandlers[$"{API.GetCurrentResourceName()}:SendConfig"] += new Action<string, ExpandoObject>(LoadDefaultConfig);
            TriggerServerEvent($"{API.GetCurrentResourceName()}:getConfig");
        }

        private void LoadDefaultConfig(string dc, ExpandoObject dl)
        {

            Config = JObject.Parse(dc);

            foreach (var l in dl)
            {
                Langs[l.Key] = l.Value.ToString();
            }

            InitScripts();
        }

        public void InitScripts()
        {
            Scripts.DiscRichPresence.drp_active = Config["ActiveDRP"].ToObject<bool>();
            Scripts.IDHeads.UseIDHeads = Config["HeadId"].ToObject<bool>();
            Scripts.IDHeads.UseKeyMode = Config["ModeKey"].ToObject<bool>();

            Scripts.IDHeads.keyShow = FromHex(Config["KeyShowIds"].ToString());

        }

        public static uint FromHex(string value)
        {
            if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                value = value.Substring(2);
            }
            return (uint)Int32.Parse(value, NumberStyles.HexNumber);
        }
    }
}
