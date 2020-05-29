using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Threading.Tasks;
using vorpcore_cl.Utils;
using CitizenFX.Core.Native;

namespace vorpcore_sv.Scripts
{
    public class SpawnPlayer : BaseScript
    {
        public static bool firstSpawn = true;
        public static bool iSPvpOn = false;

        public SpawnPlayer()
        {
            //Iniciamos un Tick para setear el PVP
            Tick += setPVPTick;
            //Iniciamos un Tick para guardar cada 10 segundos las coords del jugador en el servidor
            Tick += saveLastCoordsTick;

            EventHandlers["vorp:initPlayer"] += new Action</*string, string, string, int, int, string,*/ Vector3, float>(InitPlayer);
            Function.Call(Hash.SET_MINIMAP_HIDE_FOW, true);

            EventHandlers["playerSpawned"] += new Action<object>(InitTpPlayer);

        }

        private async void InitTpPlayer(object spawnInfo)
        {
            if (firstSpawn) { 
                Debug.WriteLine("INIT_PLAYER");
                await Delay(4000);
                TriggerServerEvent("vorp:playerSpawn"); // --> vorpcore_sv/vorpcore_sv.cs
                firstSpawn = false;
            }
        }

        private void InitPlayer(/*string characterName, string characterSurname, string group, int xp, int level, string job, */ Vector3 coords, float heading)
        { 
            Function.Call(Hash.SET_MINIMAP_HIDE_FOW, true);
            //Teleportamos al jugador a la posicion que se quedo
            PlayerActions.TeleportToCoords(coords.X, coords.Y, coords.Z, heading);
        }


        [Tick]
        private async Task setPVPTick()
        {
            await Delay(0);
            uint playerHash = (uint)API.GetHashKey("PLAYER");
            Function.Call((Hash)0xF808475FA571D823, true);
            Function.Call((Hash)0xBF25EB89375A37AD, 5, playerHash, playerHash);
            API.DisableControlAction(0, 0x580C4473, true); // Disable hud
            API.DisableControlAction(0, 0xCF8A4ECA, true); // Disable hud
        }

        [Tick]
        private async Task saveLastCoordsTick()
        {
            await Delay(3000);

            if (!firstSpawn)
            {
                int playerPedId = API.PlayerPedId();
                Vector3 playerCoords = API.GetEntityCoords(playerPedId, true, true);
                float playerHeading = API.GetEntityHeading(playerPedId);

                TriggerServerEvent("vorp:saveLastCoords", playerCoords, playerHeading);
            }



        }
    }
}
