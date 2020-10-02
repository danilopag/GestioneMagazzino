using System;
using System.Runtime.Serialization;

namespace WCFServer
{
    [DataContract]
    public class ProdottoClass { 
        [DataMember]
        public string Idprodotto { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public string Descrizione { get; set; }
        [DataMember]
        public double Prezzo { get; set; }
        [DataMember]
        public int Quantita { get; set; }
        [DataMember]
        public bool Inesaurimento { get; set; }
    }
}
