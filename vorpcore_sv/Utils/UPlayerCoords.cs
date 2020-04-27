using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace vorpcore_sv.Utils
{
    [DataContract]
    public class UPlayerCoords
    {
        [DataMember]
        public float x { get; set; }

        [DataMember]
        public float y { get; set; }

        [DataMember]
        public float z { get; set; }

        [DataMember]
        public float heading { get; set; }
    }
}
