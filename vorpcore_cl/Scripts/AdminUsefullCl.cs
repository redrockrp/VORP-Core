using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using vorpcore_cl.Models;

namespace vorpcore_cl.Scripts
{


/// <summary>
/// Author:Xmau
/// RDR2 class for server administration
/// </summary>
    class AdminUsefullCl : BaseScript
    {
        Vector3 lastTpCoords = new Vector3(0.0F, 0.0F, 0.0F);
        int blip;
        string lastpos = "LASTPOSITION";

        /// <summary>
        /// Method main that have all que commands for menu and console
        /// </summary>
        public AdminUsefullCl()
        {
            //EventHandlers

            EventHandlers["vorp:sendCoordsToDestinyBring"] += new Action<Vector3>(Bring);
            EventHandlers["vorp:askForCoords"] += new Action<string>(ResponseCoords);
            EventHandlers["vorp:coordsToStart"] += new Action<Vector3>(TpToPlayerDone);

            //Commands\\

            API.RegisterCommand("com", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                executeAdminCommand("Com", args);
            }), false);

            //Spawners

            /// <see cref="Spawnobj(List{object})"/>
            API.RegisterCommand("spawnobj", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                executeAdminCommand("Spawnobj", args);
            }), false);

            /// <see cref="Spawnped(List{object})"/>
            API.RegisterCommand("spawnped", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                executeAdminCommand("Spawnped", args);
            }), false);

            /// <see cref="Spawnveh(List{object})"/>
            API.RegisterCommand("spawnveh", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                executeAdminCommand("Spawnveh", args);
            }), false);



            //Tps\\

            API.RegisterCommand("tpwayp", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                executeAdminCommand("TpToWaypoint", args);
            }), false);

            API.RegisterCommand("tpcoords", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                executeAdminCommand("TpToCoords", args);
            }), false);

            API.RegisterCommand("tpplayer", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                executeAdminCommand("TpToPlayer", args);
            }), false);

            API.RegisterCommand("tpbring", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                executeAdminCommand("TpBring", args);
            }), false);

            API.RegisterCommand("tpbring", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                executeAdminCommand("TpBring", args);
            }), false);

            API.RegisterCommand("tpback", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                executeAdminCommand("TpBack", args);
            }), false);




            //Advantages\\

            API.RegisterCommand("golden", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                executeAdminCommand("Golden", args);
            }), false);




            //Menu\\

        }


        /// <summary>
        /// Method to control the admin role of the player
        /// </summary>
        /// <param name="command"> String nameOfFunction </param>
        /// <param name="args"> List<object> argsOfFunction </param>
        public async void executeAdminCommand(string command,List<object> args)
        {
            if (MUser.group == "admin")
            {
                MethodInfo mi = GetType().GetMethod(command);
                mi.Invoke(this, new Object[] { args });
            }
            else
            {
                Debug.WriteLine("No te pases de listo");
            }
        }

        public async void Com(List<object> args)
        {
            Debug.WriteLine("spawnobj objectModel ----> Spawn object");
            Debug.WriteLine("spawnped pedModel ----> Spawn ped");
            Debug.WriteLine("spawnveh vechicleModel ----> Spawn vechicle");

            Debug.WriteLine("tpwayp  ----> Teleport to a waypoint(Mark a waypoint before)");
            Debug.WriteLine("tpcoords [cooordX] [coordY]  ----> Teleport to coord");
            Debug.WriteLine("tpplayer idPlayer ----> Teleport to player");
            Debug.WriteLine("tpbring idPlayer ----> Bring player to your position");

            Debug.WriteLine("golden ----> You and you horse become full gold");
        }




        /// <summary>
        /// Method that create an object in front or the character
        /// </summary>
        /// <param name="args"> object model </param>
        public async void Spawnobj(List<object> args)
        {
            string objeto = args[0].ToString();
            int HashObjeto = API.GetHashKey(objeto);
            Vector3 coords = API.GetEntityCoords(API.PlayerPedId(), true, true);
            await Utils.Miscellanea.LoadModel(HashObjeto);
            int cosa = API.CreateObject((uint)HashObjeto, coords.X + 0.5f, coords.Y + 0.5f, coords.Z + 1.0f, true, true, false, true, true);
            API.PlaceObjectOnGroundProperly(cosa, 1);
        }

        /// <summary>
        /// Method that create a ped in front or the character, in case that the created ped
        /// is an horse the character is setted mounting it
        /// </summary>
        /// <param name="args"> ped model </param>
        public async void Spawnped(List<object> args)
        {
            string ped = args[0].ToString();
            int HashPed = API.GetHashKey(ped);
            Vector3 coords = API.GetEntityCoords(API.PlayerPedId(), true, true);
            await Utils.Miscellanea.LoadModel(HashPed);
            int pedCreated = API.CreatePed((uint)HashPed, coords.X + 1, coords.Y, coords.Z, 0, true, true, true, true);
            //Spawn
            Function.Call((Hash)0x283978A15512B2FE, pedCreated, true);
            //SetPedIntoVehicle
            Function.Call((Hash)0x028F76B6E78246EB, API.PlayerPedId(), pedCreated, -1, false);
        }

        /// <summary>
        /// Method that create a vehicle and the character is setted mounting it
        /// </summary>
        /// <param name="args"> vehicle model </param>
        public async void Spawnveh(List<object> args)
        {
            string veh = args[0].ToString();
            int HashVeh = API.GetHashKey(veh);
            Vector3 coords = API.GetEntityCoords(API.PlayerPedId(), true, true);
            await Utils.Miscellanea.LoadModel(HashVeh);
            int vehCreated = API.CreateVehicle((uint)HashVeh, coords.X + 1, coords.Y, coords.Z, 0,true, true, true, true);
            //Spawn
            Function.Call((Hash)0x283978A15512B2FE, vehCreated, true);
            //TaskWanderStandard
            Function.Call((Hash)0xBB9CE077274F6A1B, 10, 10);
            //SetPedIntoVehicle
            Function.Call((Hash)0x23f74c2fda6e7c61, API.PlayerPedId(), vehCreated, -1, false);
        }


        //Tps

        /// <summary>
        /// Method that teleport player to waypoint
        /// </summary>
        /// <param name="args">None</param>
        public async void TpToWaypoint(List<object> args)
        {
            Vector3 waypointCoords = API.GetWaypointCoords();
            lastTpCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);

            blip = Function.Call<int>((Hash)0x554D9D53F696D002, 203020899, lastTpCoords.X, lastTpCoords.Y, lastTpCoords.Z);
            Debug.WriteLine(blip.ToString());
            Function.Call((Hash)0x74F74D3207ED525C, blip, -1546805641, 1);
            Function.Call((Hash)0xD38744167B2FA257, blip, 0.2F);
            Function.Call((Hash)0x0A062D6D7C0B2C2C, blip, lastpos);
            Utils.AdminActions.TeleportAndFoundGroundAsync(waypointCoords);
        }

        /// <summary>
        /// Method that teleport player to coords in ground
        /// </summary>
        /// <param name="args"> string x, string y </param>

        public async void TpToCoords(List<object> args)
        {
            try
            {
                lastTpCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
                float XCoord = float.Parse(args[0].ToString());
                float YCoord = float.Parse(args[1].ToString());
                float ZCoord = 0.0f;
                Vector3 chosenCoords = new Vector3(XCoord,YCoord, ZCoord);
                Utils.AdminActions.TeleportAndFoundGroundAsync(chosenCoords);
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        /// <summary>
        /// Method that send player coords through server to bring player to own coords
        /// </summary>
        /// <param name="args">None</param>
        public async void TpBring(List<object> args)
        {
            int destinataryID = int.Parse(args[0].ToString());
            Vector3 ownCoords = API.GetEntityCoords(API.PlayerPedId(),true,true);

            TriggerServerEvent("vorp:ownerCoordsToBring",ownCoords,destinataryID);
        }

        /// <summary>
        /// Method that receive coords from the server to make an teleport requested by admin
        /// </summary>
        /// <param name="bringCoords"></param>
        public async void Bring(Vector3 bringCoords)
        {
            Debug.WriteLine(bringCoords.X.ToString());
            Utils.AdminActions.TeleportAndFoundGroundAsync(bringCoords);
        }


        /// <summary>
        /// Method that request coords from the player destiny
        /// </summary>
        /// <param name="args"></param>
        public async void TpToPlayer(List<object> args)
        {
            lastTpCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
            Debug.WriteLine("1");
            int destinataryID = int.Parse(args[0].ToString());
            TriggerServerEvent("vorp:askCoordsToTPPlayerDestiny", destinataryID);
        }

        /// <summary>
        /// Method that response to the petition of coords made by TpToPlayer through server
        /// </summary>
        /// <param name="sourceID"></param>
        private void ResponseCoords(string sourceID)
        {
            Vector3 responseCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
            TriggerServerEvent("vorp:callbackCoords", sourceID, responseCoords);
        }

        public async void TpBack(List<object> args)
        {
            Utils.AdminActions.TeleportAndFoundGroundAsync(lastTpCoords);
        }

        /// <summary>
        /// Method that teleport the source to player destinatary
        /// </summary>
        /// <param name="coordsToTp"></param>
        private void TpToPlayerDone(Vector3 coordsToTp)
        {
            Utils.AdminActions.TeleportAndFoundGroundAsync(coordsToTp);
        }

        public async void Golden(List<object> args)
        {
            int pPedId = API.PlayerPedId();
            //Jugador cores
        Function.Call((Hash)0xC6258F41D86676E0, pPedId, 0, 100);
        Function.Call((Hash)0xC6258F41D86676E0, pPedId, 1, 100);
        Function.Call((Hash)0xC6258F41D86676E0, pPedId, 2, 100);
            //Jugador circles                   
        Function.Call((Hash)0x4AF5A4C7B9157D14, pPedId, 0, 5000.0) ;
        Function.Call((Hash)0x4AF5A4C7B9157D14, pPedId, 1, 5000.0) ;
        Function.Call((Hash)0x4AF5A4C7B9157D14, pPedId, 2, 5000.0);

        Function.Call((Hash)0xF6A7C08DF2E28B28, pPedId, 1, 5000.0);
        Function.Call((Hash)0xF6A7C08DF2E28B28, pPedId, 2, 5000.0);
        Function.Call((Hash)0xF6A7C08DF2E28B28, pPedId, 0, 5000.0);


        int entity = Function.Call<int>(Hash.GET_ENTITY_ATTACHED_TO,pPedId);
       

        Function.Call((Hash)0x09A59688C26D88DF, entity, 0, 1100);
        Function.Call((Hash)0x09A59688C26D88DF, entity, 1, 1100);
        Function.Call((Hash)0x09A59688C26D88DF, entity, 2, 1100);

        Function.Call((Hash)0x75415EE0CB583760, entity, 0, 1100);
        Function.Call((Hash)0x75415EE0CB583760, entity, 1, 1100);
        Function.Call((Hash)0x75415EE0CB583760, entity, 2, 1100);

        Function.Call((Hash)0x5DA12E025D47D4E5, entity, 0, 10);
        Function.Call((Hash)0x5DA12E025D47D4E5, entity, 1, 10);
        Function.Call((Hash)0x5DA12E025D47D4E5, entity, 2, 10);

        Function.Call((Hash)0x920F9488BD115EFB, entity, 0, 10);
        Function.Call((Hash)0x920F9488BD115EFB, entity, 1, 10);
        Function.Call((Hash)0x920F9488BD115EFB, entity, 2, 10);

        Function.Call((Hash)0xF6A7C08DF2E28B28, entity, 0, 5000.0);
        Function.Call((Hash)0xF6A7C08DF2E28B28, entity, 1, 5000.0);
        Function.Call((Hash)0xF6A7C08DF2E28B28, entity, 2, 5000.0);

        Function.Call((Hash)0x4AF5A4C7B9157D14, entity, 0, 5000.0);
        Function.Call((Hash)0x4AF5A4C7B9157D14, entity, 1, 5000.0);
        Function.Call((Hash)0x4AF5A4C7B9157D14, entity, 2, 5000.0);
        }


       


    }
}
// int tiempo = 1;
// if (args.Count == 1)
// {
//     tiempo = 10;
// }
// else
// {
//     tiempo = int.Parse(args[1].ToString());
// }


/**int parar = 10000;
            int r = 1;
            int g = 100;
            int b = 150;
            while (parar > 1)
            {
                if (r > 254)
                {
                    r = 1;
                }
                else
                {
                    r += 1;
                }
                if (g > 254)
                {
                    g = 1;
                }
                else
                {
                    g += 1;
                }
                if (b > 254)
                {
                    b = 1;
                }
                else
                {
                    b += 1;
                }
                await Delay(10);
                parar = parar - 1;
                API.DrawLightWithRange(coords.X + 0.5f, coords.Y + 0.5f, coords.Z + 1.0f, r, g, b, 20.0f, 100000000.0f);
            }
            await Delay(tiempo * 1000);
            API.DeleteObject(ref cosa);*/
