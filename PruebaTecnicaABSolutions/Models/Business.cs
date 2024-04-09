using System;
using System.Collections.Generic;

namespace PruebaTecnicaABSolutions.Models
{
    public partial class Business
    {
        public Business()
        {
            MenuItems = new HashSet<MenuItem>();
            Users = new HashSet<User>();
        }

        public int BusinessId { get; set; }
        public string? BusinessName { get; set; }
        public string? Description { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
