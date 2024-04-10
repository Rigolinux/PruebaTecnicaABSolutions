namespace PruebaTecnicaABSolutions.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? UserTypeId { get; set; }
        public int? BusinessId { get; set; }
        public string? Email { get; set; }


        public virtual Business? Business { get; set; }
        public virtual UserType? UserType { get; set; }

    }
}
