using System;
using System.Collections.Generic;

namespace PruebaTecnicaABSolutions.Models
{
    public partial class MenuItemViewUpdate : MenuItem
    {
     

        public IEnumerable<BusinessViewList>? businessViews { get; set; }
        public IEnumerable<MenuCategoryViewList>? menuCategoryViews { get; set; }
    }
}
