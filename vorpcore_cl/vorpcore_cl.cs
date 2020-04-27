using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpcore_cl
{
    public class vorpcore_cl : BaseScript
    {
        public vorpcore_cl()
        {
            API.RegisterCommand("givehorse", new Action(GiveHorse), false);
        }


        private async void GiveHorse()
        {
            int playerPedId = API.PlayerPedId();
            Vector3 pos = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, playerPedId);
            Vector3 forwardPos = Function.Call<Vector3>(Hash.GET_ENTITY_FORWARD_VECTOR, playerPedId);
            pos += (forwardPos * 5);
            float hdg = Function.Call<float>(Hash.GET_ENTITY_HEADING, playerPedId);

            int hash = API.GetHashKey("dawdaw");

            await Utils.Miscellanea.LoadModel(hash); //Ejemplo de un utils asyncronico

            

            /**int veh = Function.Call<int>(Hash.CREATE_VEHICLE, hash, pos.X, pos.Y, pos.Z, )**/
        }
    }
}
