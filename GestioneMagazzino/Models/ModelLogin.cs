using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestioneMagazzino.Models
{
    public class ModelLogin
    {
        [Required]
        public string Idutente { get; set; }
        [Required]
        public string Password { get; set; }
    }
}