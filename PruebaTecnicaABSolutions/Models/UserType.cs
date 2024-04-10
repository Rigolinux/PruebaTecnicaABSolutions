using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaABSolutions.Models
{
    public partial class UserType
    {
    

        public int UserTypeId { get; set; }

        [Required]
        public string TypeName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
