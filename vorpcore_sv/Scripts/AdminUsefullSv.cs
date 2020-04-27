using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpcore_sv.Scripts
{
    class AdminUsefullSv : BaseScript
    {
        public AdminUsefullSv(){
            EventHandlers["vorp:ownerCoordsToBring"] += new Action<Vector3, int>(CoordsToBringPlayer);
            EventHandlers["vorp:askCoordsToTPPlayerDestiny"] += new Action<Player, int>(CoordsToPlayerDestiny);
            EventHandlers["vorp:callbackCoords"] += new Action<string, Vector3>(CoordsToStart);
        }
        
        /// <summary>
        /// Method that send source coords for bring method
        /// </summary>
        /// <param name="ply">Player source</param>
        /// <param name="coordToSend"> Vector3 coordsFromSource </param>
        /// <param name="destinataryID"> int idDestinatary </param>
        private void CoordsToBringPlayer(Vector3 coordToSend, int destinataryID)
        {
            PlayerList pl = new PlayerList();
            Player p = pl[destinataryID];
            
           
            Debug.WriteLine("entra");
            TriggerClientEvent(p,"vorp:sendCoordsToDestinyBring", coordToSend);
        }

        /// <summary>
        /// Method that ask for the coords of the player destinatary
        /// </summary>
        /// <param name="ply"> Player source </param>
        /// <param name="destinataryID"> int idDestinatary </param>
        private void CoordsToPlayerDestiny([FromSource]Player ply, int destinataryID)
        {
            Debug.WriteLine("2");

            
            PlayerList pl = new PlayerList();
            Player p = pl[destinataryID];
            TriggerClientEvent(p,"vorp:askForCoords", ply.Handle);
        }

        /// <summary>
        /// Method that make a callback whit the desired coords
        /// </summary>
        /// <param name="sourceID">Player sourceToResponse</param>
        /// <param name="coordsDestiny"> Vector3 coordOfDestiny</param>
        private void CoordsToStart(string sourceID, Vector3 coordsDestiny)
        {
            Debug.WriteLine("4");
            
            PlayerList pl = new PlayerList();
            Player p = pl[int.Parse(sourceID)];
            TriggerClientEvent(p,"vorp:coordsToStart", coordsDestiny);
        }
    }
}
