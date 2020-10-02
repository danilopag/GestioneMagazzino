using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestioneMagazzino.Models
{
    public class ModelFornitoriList
    {
        [Required]
        public string Idfornitore { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        [StringLength(11, ErrorMessage = "Non inserire più di 11 caratteri")]
        public string Partitaiva { get; set; }
        [Required]
        public string Indirizzo { get; set; }
        [Required]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

    }
}