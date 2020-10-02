using System;
using System.Runtime.Serialization;

namespace WCFServer
{
    [DataContract]
    public class FornitoreClass
    {
        [DataMember]
        public string Idfornitore { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public string Partitaiva { get; set; }
        [DataMember]
        public string Indirizzo { get; set; }
        [DataMember]
        public string Email { get; set; }
    }
}
