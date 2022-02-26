using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace Project.Common
{
    public static class Helpers
    {
        public static readonly string PWD_SALT = "XYZ";

        public static string HashPassword(string passwordToHash)
        {
            byte[] salt = Encoding.ASCII.GetBytes(PWD_SALT);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordToHash,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
