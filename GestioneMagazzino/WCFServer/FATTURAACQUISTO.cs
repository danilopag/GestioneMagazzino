//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WCFServer
{
    using System;
    using System.Collections.Generic;
    
    public partial class FATTURAACQUISTO
    {
        public string NFATTURA { get; set; }
        public System.DateTime DATAACQUISTO { get; set; }
        public int QUANTITA { get; set; }
        public double PREZZOACQUISTO { get; set; }
        public string CODFORNITORE { get; set; }
        public string CODPRODOTTO { get; set; }
        public string CODUTENTE { get; set; }
    
        public virtual FORNITORE FORNITORE { get; set; }
        public virtual PRODOTTO PRODOTTO { get; set; }
        public virtual UTENTE UTENTE { get; set; }
    }
}
