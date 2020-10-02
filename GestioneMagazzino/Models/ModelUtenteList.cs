using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestioneMagazzino.Models
{
    public class ModelUtenteList
    {
        [Required]      
        public string Idutente { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cognome { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int IsAdmin { get; set; }
    }
}