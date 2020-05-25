using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace vorpcore_cl.Scripts
{
    public class Instance : BaseScript
    {
        private static bool intancePlayer = false;
        public Instance()
        {
            EventHandlers["vorp:setInstancePlayer"] += new Action<bool>(SetInstancePlayer);
            Tick += InstancePlayer;
        }

        private async Task DeInstancePlayer()
        {
            for (int i = 0; i < 255; i++)
            {
                if (API.NetworkIsPlayerActive(i))
                {
                    if (API.GetPlayerPed(i) != API.PlayerPedId())
                    {
                        API.SetEntityAlpha(API.GetPlayerPed(i), 255, false);
                        API.SetEntityNoCollisionEntity(API.PlayerPedId(), API.GetPlayerPed(i), true);
                        await Delay(1);
                    }

                }

            }
        }

        private void SetInstancePlayer(bool inst)
        {
            if (inst) 
            {
                intancePlayer = true;
            }
            else
            {
                intancePlayer = false;
                DeInstancePlayer();
            }
        }

        [Tick]
        private async Task InstancePlayer()
        {
            if (intancePlayer)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (API.NetworkIsPlayerActive(i))
                    {
                        if (API.GetPlayerPed(i) != API.PlayerPedId())
                        {
                            API.SetEntityAlpha(API.GetPlayerPed(i), 0, false);
                            API.SetEntityNoCollisionEntity(API.PlayerPedId(), API.GetPlayerPed(i), false);
                            await Delay(1);
                        }

                    }

                }
            }
            await Delay(1000);
        }
    }
}
