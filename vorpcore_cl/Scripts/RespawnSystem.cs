using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpcore_cl.Scripts
{
    class RespawnSystem : BaseScript
    {
        public RespawnSystem()
        {
            Tick += OnPlayerDead;
        }

        [Tick]
        public async Task OnPlayerDead()
        {
            await Delay(0);
            while (Function.Call<bool>((Hash)0x2E9C3FCB6798F397, API.PlayerId()))
            {
                await Delay(0);
                int timer = Function.Call<int>((Hash)0x4F67E8ECA7D3F667) + Utils.GetConfig.Config["RespawnTime"].ToObject<int>();
                while (timer >= Function.Call<int>((Hash)0x4F67E8ECA7D3F667))
                {
                    Function.Call((Hash)0xFA08722A5EA82DA7, "CrossLine01");
                    Function.Call((Hash)0xFDB74C9CC54C3F37, 1.0f);
                    Function.Call((Hash)0x405224591DF02025, 0.50f, 0.475f, 1.0f, 0.22f, 1, 1, 1, 100, true, true);
                    await DrawTxt(Utils.GetConfig.Langs["TitleOnDead"], 0.50F, 0.40F, 1.0F, 1.0F, 161, 3, 0, 255, true, true);
                    await DrawTxt(string.Format(Utils.GetConfig.Langs["SubTitleOnDead"], (((Function.Call<int>((Hash)0x4F67E8ECA7D3F667) - timer) * -1)/1000).ToString()), 0.50f, 0.50f, 0.7f, 0.7f, 255, 255, 255, 255, true, true);
                    Function.Call((Hash)0xD63FE3AF9FB3D53F, false);
                    Function.Call((Hash)0x1B3DA717B9AFF828, false);
                    Exports["spawnmanager"].setAutoSpawn(false);
                }
                await resurrectPlayer();
            }
        }

        public async Task resurrectPlayer()
        {
            Function.Call((Hash)0x71BC8E838B9C6035, API.PlayerPedId());
            Function.Call((Hash)0x0E3F4AF2D63491FB);
            Function.Call((Hash)0xD63FE3AF9FB3D53F, true);
            Function.Call((Hash)0x1B3DA717B9AFF828, true);
        }

        public async Task DrawTxt(string text, float x, float y, float fontscale, float fontsize, int r, int g, int b, int alpha, bool textcentred, bool shadow)
        {
            long str = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", text);
            Function.Call(Hash.SET_TEXT_SCALE, fontscale, fontsize);
            Function.Call(Hash._SET_TEXT_COLOR, r, g, b, alpha);
            Function.Call(Hash.SET_TEXT_CENTRE, textcentred);
            if (shadow) { Function.Call(Hash.SET_TEXT_DROPSHADOW, 1, 0, 0, 255); }
            Function.Call(Hash.SET_TEXT_FONT_FOR_CURRENT_COMMAND, 1);
            Function.Call(Hash._DISPLAY_TEXT, str, x, y);
        }
    }
}
