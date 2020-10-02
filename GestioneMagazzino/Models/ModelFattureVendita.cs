using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestioneMagazzino.Models
{
    public class ModelFattureVendita
    {
        [Required]
        public string Nfattura { get; set; }
        public DateTime DataVendita { get; set; }
        [Required]
        public int Quantita { get; set; }
        [Required]
        public bool Pagamento { get; set; }
        [Required]
        public string CodCliente { get; set; }
        [Required]
        public string CodUtente { get; set; }
        [Required]
        public string CodProdotto { get; set; }
        public double PrezzoVendita { get; set; }
    }
}