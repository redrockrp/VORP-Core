using System.Runtime.Serialization;

namespace vorpcore_sv.Utils
{
    [DataContract]
    public class JsonUiCalls
    {
        [DataMember]
        public string type { get; set; } // ui

        [DataMember]
        public string action { get; set; } // Can be set update

        [DataMember]
        public double moneyquanty { get; set; }

        [DataMember]
        public double goldquanty { get; set; }

        [DataMember]
        public double rolquanty { get; set; }

    }
}
