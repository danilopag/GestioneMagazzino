using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GestioneMagazzino.Models
{
    public class ModelFattureAcquisto
    {
        [Required]
        public string Nfattura { get; set; }
        public DateTime DataAcquisto { get; set; }
        [Required]
        public double PrezzoAcquisto { get; set; }
        [Required]
        public int Quantita { get; set; }
        [Required]
        public string CodFornitore { get; set; }
        [Required]
        public string CodUtente { get; set; }
        [Required]
        public string CodProdotto { get; set; }

    }
}