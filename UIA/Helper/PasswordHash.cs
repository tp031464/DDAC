using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOne.Security.Cryptography.BCrypt;

namespace UIA.Helper
{
    public static class PasswordHash
    {
        
        public static string Encrypt(string password)
        {
            string salt = BCryptHelper.GenerateSalt();
            return BCryptHelper.HashPassword(password, salt);
        }

        public static bool CheckPassword(string plainText, string hashedPassword)
        {
            return BCryptHelper.CheckPassword(plainText, hashedPassword);
        }
    }
}