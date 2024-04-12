using System;
using System.Collections.Generic;

namespace PruebaTecnicaABSolutions.Models
{
    public partial class MenuItemViewCreation : MenuItem
    {
        
        public IEnumerable<BusinessViewList>? businessViews { get; set; }
        public IEnumerable<MenuCategoryViewList>? menuCategoryViews { get; set; }
    }
}
