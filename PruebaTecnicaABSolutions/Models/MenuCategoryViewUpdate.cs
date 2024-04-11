using System;
using System.Collections.Generic;

namespace PruebaTecnicaABSolutions.Models
{
    public partial class MenuCategoryViewUpdate
    {
       

        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? BusinessId { get; set; }

        public IEnumerable<BusinessViewList>? businessViews { get; set; }
    }
}
