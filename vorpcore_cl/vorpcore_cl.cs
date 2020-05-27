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
            EventHandlers["populationPedCreating"] += new Action<float, float, float, uint, dynamic>(
                    (x, y, z, model, setters) => {
                        API.CancelEvent(); 
                    });
        }

    }
}
