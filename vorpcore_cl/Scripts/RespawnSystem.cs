using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;
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
            EventHandlers["vorp:resurrectPlayer"] += new Action(ResurrectPlayer);
        }

        private async void ResurrectPlayer()
        {
            await resurrectPlayer();
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
                    await Delay(0);
                    Function.Call((Hash)0xFDB74C9CC54C3F37, 1.0f);
                    Function.Call((Hash)0x405224591DF02025, 0.50f, 0.475f, 1.0f, 0.22f, 1, 1, 1, 100, true, true);
                    await DrawTxt(Utils.GetConfig.Langs["TitleOnDead"], 0.50F, 0.40F, 1.0F, 1.0F, 161, 3, 0, 255, true, true);
                    await DrawTxt(string.Format(Utils.GetConfig.Langs["SubTitleOnDead"], (((Function.Call<int>((Hash)0x4F67E8ECA7D3F667) - timer) * -1)/1000).ToString()), 0.50f, 0.50f, 0.7f, 0.7f, 255, 255, 255, 255, true, true);
                    Function.Call((Hash)0xD63FE3AF9FB3D53F, false);
                    Function.Call((Hash)0x1B3DA717B9AFF828, false);
                    Exports["spawnmanager"].setAutoSpawn(false);
                }
                string keyPress = Utils.GetConfig.Config["RespawnKey"].ToString();
                int KeyInt = Convert.ToInt32(keyPress, 16);
                bool pressed = false;
                while (!pressed)
                {
                    await Delay(0);
                    if (Function.Call<bool>((Hash)0xC841153DED2CA89A, API.PlayerPedId()))
                    {
                        await DrawTxt(Utils.GetConfig.Langs["YouAreCarried"], 0.50f, 0.45f, 1.0f, 1.0f, 255, 255, 255, 255, true, true);
                    }
                    else
                    {
                        await DrawTxt(Utils.GetConfig.Langs["SubTitlePressKey"], 0.50f, 0.45f, 1.0f, 1.0f, 255, 255, 255, 255, true, true);
                        if (Function.Call<bool>((Hash)0x580417101DDB492F, 0, KeyInt))
                        {
                            TriggerServerEvent("vorpcharacter:getPlayerSkin");

                            TriggerServerEvent("vorp:PlayerForceRespawn");
                            TriggerEvent("vorp:PlayerForceRespawn");
                            await resspawnPlayer();
                            pressed = true;
                            await Delay(100);
                        }
                    }
                }
            }
        }

        public async Task resspawnPlayer()
        {
            JToken respawnCoords = Utils.GetConfig.Config["RespawnCoords"];
            Function.Call((Hash)0x203BEFFDBE12E96A, API.PlayerPedId(), respawnCoords[0].ToObject<float>(), respawnCoords[1].ToObject<float>(), respawnCoords[2].ToObject<float>(), respawnCoords[3].ToObject<float>(), false, false, false);
            await Delay(10);
            Function.Call((Hash)0x71BC8E838B9C6035, API.PlayerPedId());
            Function.Call((Hash)0x0E3F4AF2D63491FB);
            Function.Call((Hash)0xD63FE3AF9FB3D53F, true);
            Function.Call((Hash)0x1B3DA717B9AFF828, true);
            SpawnPlayer.setPVP();
        }

        public async Task resurrectPlayer()
        {
            Function.Call((Hash)0x71BC8E838B9C6035, API.PlayerPedId());
            Function.Call((Hash)0x0E3F4AF2D63491FB);
            Function.Call((Hash)0xD63FE3AF9FB3D53F, true);
            Function.Call((Hash)0x1B3DA717B9AFF828, true);
            SpawnPlayer.setPVP();
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
