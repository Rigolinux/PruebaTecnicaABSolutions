using System;
using System.Collections.Generic;

namespace PruebaTecnicaABSolutions.Models
{
    public partial class UserType
    {
        public UserType()
        {
            Users = new HashSet<User>();
        }

        public int UserTypeId { get; set; }
        public string? TypeName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
