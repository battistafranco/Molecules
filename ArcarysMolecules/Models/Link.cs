using System.ComponentModel.DataAnnotations;

namespace Arcarys.Models
{
    public enum Order { Simple = 1, Doble = 2, Triple = 3 };

    public class Link : Base
    {
        private Order _order;

        [Display(Name = "Orden")]
        public Order Order
        {
            get { return _order; }
            set { _order = value; }
        }

        [Display(Name = "Enlace de hidrógeno?")]
        public bool IsHydrogenLink { get; set; }
    }
}
