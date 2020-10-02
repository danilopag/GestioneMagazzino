using System;
using System.Runtime.Serialization;

namespace WCFServer
{
    [DataContract]
    public class ClienteClass
    {
        [DataMember]
        public String Idcliente { get; set; }
        [DataMember]
        public String Nome { get; set; }
        [DataMember]
        public String Partitaiva { get; set; }
        [DataMember]
        public String Codicefiscale { get; set; }
        [DataMember]
        public String Indirizzo { get; set; }
        [DataMember]
        public String Email { get; set; }
        }
    }
