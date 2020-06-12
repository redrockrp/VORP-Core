using System.Runtime.Serialization;

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
