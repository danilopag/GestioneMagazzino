using System;
using System.Runtime.Serialization;

namespace WCFServer
{
    [DataContract]
    public class UtenteClass
    {
        [DataMember]
        public string Idutente { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public string Cognome { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public int IsAdmin { get; set; }
    }
}
