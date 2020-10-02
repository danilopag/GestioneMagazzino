using System;
using System.Runtime.Serialization;

namespace WCFServer
{
    [DataContract]
    public class FatturaVenditaClass
    {
        [DataMember]
        public string Nfattura { get; set; }
        [DataMember]
        public DateTime DataVendita { get; set; }
        [DataMember]
        public int Quantita { get; set; }
        [DataMember]
        public bool Pagamento { get; set; }
        [DataMember]
        public string CodCliente { get; set; }
        [DataMember]
        public string CodUtente { get; set; }
        [DataMember]
        public string CodProdotto { get; set; }
        [DataMember]
        public double PrezzoVendita { get; set; } //Attributo usato solo per fare il totale della fattura
    }
}
