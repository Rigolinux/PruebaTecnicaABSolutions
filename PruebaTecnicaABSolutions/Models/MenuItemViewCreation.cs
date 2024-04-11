using System;
using System.Collections.Generic;

namespace PruebaTecnicaABSolutions.Models
{
    public partial class MenuItemViewCreation
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public int? BusinessId { get; set; }
        public DateTime? AddedDate { get; set; }
        
        public int?  Business { get; set; }
        public int?  Category { get; set; }

        public IEnumerable<BusinessViewList>? businessViews { get; set; }
        public IEnumerable<MenuCategoryViewList>? menuCategoryViews { get; set; }
    }
}
