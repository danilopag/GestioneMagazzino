using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GestioneMagazzino.Models
{
    [Serializable]
    public class ModelClientiList
    {
        [Required]
        public string Idcliente { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        [StringLength(11, ErrorMessage = "Non inserire più di 11 caratteri")]
        public string Partitaiva { get; set; }
        [Required]
        [StringLength(16, ErrorMessage = "Non inserire più di 16 caratteri")]
        public string Codicefiscale { get; set; }
        [Required]
        public string Indirizzo { get; set; }
        [Required]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email Non valida.")]
        public string Email { get; set; }
    }
}