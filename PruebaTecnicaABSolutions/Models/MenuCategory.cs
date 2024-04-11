using System;
using System.Collections.Generic;

namespace PruebaTecnicaABSolutions.Models
{
    public partial class MenuCategory
    {
        public MenuCategory()
        {
            MenuItems = new HashSet<MenuItem>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? BusinessId { get; set; }

        public virtual Business? Business { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
