namespace www.Cryptography
{
    public interface ICryptoProvider
    {
        string GeneratePasswordHash(string password);
        bool VerifyPassword(string password, string correctPasswordHash);
    }
}
