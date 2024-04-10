namespace PruebaTecnicaABSolutions.Models
{
    public class UserViewCreation
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public int BusinessId { get; set; }
        public int UserType { get; set; }
        
        public string Password { get; set; }

        public IEnumerable<BusinessViewList>? businessViews { get; set; }
    }
}

