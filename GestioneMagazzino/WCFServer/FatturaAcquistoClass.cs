using System;
using System.Runtime.Serialization;

namespace WCFServer
{
    [DataContract]
    public class FatturaAcquistoClass
    {
        [DataMember]
        public string Nfattura { get; set; }
        [DataMember]
        public DateTime DataAcquisto { get; set; }
        [DataMember]
        public double PrezzoAcquisto { get; set; } //Nel MVC sarà Prezzo totale dell'acquisto
        [DataMember]
        public int Quantita { get; set; } //Nel MVC sarà Quantita totale dell'acquisto
        [DataMember]
        public string CodFornitore { get; set; }
        [DataMember]
        public string CodUtente { get; set; }
        [DataMember]
        public string CodProdotto { get; set; }
    }
}
