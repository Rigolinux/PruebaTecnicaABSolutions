namespace PruebaTecnicaABSolutions.Services
{
    public interface IEncriptService
    {
        string Encrypt(string password);
        bool ValidatePassword(string password, string correctHash);
    }
    public class EncriptService : IEncriptService
    {
        public string Encrypt(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public bool ValidatePassword(string password, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }
    }
}

