using System;
using System.Collections.Generic;

namespace PruebaTecnicaABSolutions.Models
{
    public partial class MenuItem
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public int? BusinessId { get; set; }
        public DateTime? AddedDate { get; set; }

        public virtual Business? Business { get; set; }
        public virtual MenuCategory? Category { get; set; }
    }
}
