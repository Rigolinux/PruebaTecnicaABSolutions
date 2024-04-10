﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaABSolutions.Models
{
    public partial class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Por Favor agregue un Nombre {0} ")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public int UserTypeId { get; set; }
        public int BusinessId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual Business Business { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
