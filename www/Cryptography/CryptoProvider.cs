using System;
using System.Security.Cryptography;

namespace www.Cryptography
{
    public class CryptoProvider : ICryptoProvider
    {
        public const int SALT_BYTES = 16;
        public const int HASH_BYTES = 16;
        public const int PBKDF2_ITERATIONS = 64000;

        public const int HASH_SECTIONS = 5;
        public const int HASH_ALGORITHM_INDEX = 0;
        public const int ITERATION_INDEX = 1;
        public const int HASH_SIZE_INDEX = 2;
        public const int SALT_INDEX = 3;
        public const int PBKDF2_INDEX = 4;

        public string GeneratePasswordHash(string password)
        {
            byte[] salt = new byte[SALT_BYTES];
            using (RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider())
            {
                csprng.GetBytes(salt);
            }

            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTES);

            return string.Join(':',
                "sha1",
                PBKDF2_ITERATIONS,
                hash.Length,
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }

        public bool VerifyPassword(string password, string correctPasswordHash)
        {
            string[] hashParts = correctPasswordHash.Split(':');

            if (hashParts.Length != HASH_SECTIONS)
                return false;

            if (hashParts[HASH_ALGORITHM_INDEX] != "sha1")
                return false;

            if (!int.TryParse(hashParts[ITERATION_INDEX], out var iterations))
                return false;

            if (iterations < 1)
                return false;

            if (!TryConvertFromBase64String(hashParts[SALT_INDEX], out var salt))
                return false;

            if (!TryConvertFromBase64String(hashParts[PBKDF2_INDEX], out var hash))
                return false;

            if (!int.TryParse(hashParts[HASH_SIZE_INDEX], out var storedHashSize))
                return false;

            if (storedHashSize != hash.Length)
                return false;

            var newHash = PBKDF2(password, salt, iterations, hash.Length);

            return SlowEquals(hash, newHash);
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return pbkdf2.GetBytes(outputBytes);
            }
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;

            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }

        private static bool TryConvertFromBase64String(string value, out byte[] bytes)
        {
            try
            {
                bytes = Convert.FromBase64String(value);
                return true;
            }
            catch
            {
                bytes = Array.Empty<byte>();
                return false;
            }
        }
    }
}
