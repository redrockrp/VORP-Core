using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace vorpcore_cl.Ui
{
    public class Uicore : BaseScript
    {
        public Uicore()
        {
            // crear trigger events y asignar a los eventos de añadir y remover dinero, tambien recordar cambiar en la db a decimal el dinero y el oro
            EventHandlers["vorp:updateUi"] += new Action<string>(updateUI);
            
        }

        public void updateUI(string stringJson)
        {
            Debug.WriteLine("UiUpdate");

            API.SendNuiMessage(stringJson);
        }

    }
}
