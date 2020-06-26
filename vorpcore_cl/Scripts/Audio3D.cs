using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace vorpcore_cl.Scripts
{
    class Audio3D : BaseScript
    {
        public Audio3D()
        {
            //RegisterCommand("mute", new Action<int, List<object>, string, string>((source, args, cl, raw) =>
            //{
            //    int targetId = int.Parse(args[0].ToString());
            //    float volume = 0.0f;

            //    MumbleSetVolumeOverride(API.GetPlayerFromServerId(targetId), volume);

            //}), false);

            //RegisterCommand("unmute", new Action<int, List<object>, string, string>((source, args, cl, raw) =>
            //{
            //    int targetId = int.Parse(args[0].ToString());
            //    float volume = -1.0f;

            //    Debug.WriteLine(MumbleGetVoiceChannelFromServerId(targetId).ToString());

            //}), false);
        }
    }
}
