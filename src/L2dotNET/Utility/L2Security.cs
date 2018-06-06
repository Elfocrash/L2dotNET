using System;
using System.Security.Cryptography;
using System.Text;

namespace L2dotNET.Utility
{
    public class L2Security
    {
        public static string HashPassword(string password)
        {
            using (var crypt = new SHA256Managed())
            {
                return Convert.ToBase64String(crypt.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
    }
}