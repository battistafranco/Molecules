using System.ComponentModel.DataAnnotations;

namespace Arcarys.Models
{
    public class Atom : Base
    {
        [Required]
        [Display(Name = "Elemento")]
        public string Element { get; set; }
        [Required]
        [Display(Name = "Valencia")]
        public int Valence { get; set; }
    }
}
