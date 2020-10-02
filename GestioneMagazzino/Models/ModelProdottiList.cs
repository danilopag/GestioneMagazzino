using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestioneMagazzino.Models
{
    public class ModelProdottiList
    {
        [Required]
        public string Idprodotto { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Descrizione { get; set; }
        [Required]
        public double Prezzo { get; set; }
        [Required]
        public int Quantita { get; set; }
        public bool Inesaurimento { get; set; }
    }
}