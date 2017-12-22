using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Arcarys.Models
{
    public class Molecule : Base
    {
        [Required]
        [Display(Name="Molécula")]
        public string Name { get; set; }
        public List<Atom> Atoms { get; set; }
        public List<Link> Links { get; set; }

        public Molecule()
        {
            Atoms = new List<Atom>();
            Links = new List<Link>();
        }
    }
}
