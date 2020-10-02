using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace GestioneMagazzino.Models
{
    public class ModelRegistrazione
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
        [Range(0, 3)]
        public int IsAdmin { get; set; }
    }
}