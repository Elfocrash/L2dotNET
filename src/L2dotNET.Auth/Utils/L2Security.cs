using System;
using System.Security.Cryptography;
using System.Text;

namespace L2dotNET.LoginService.Utils
{
    public class L2Security
    {
        public static string HashPassword(string pass)
        {
            SHA1 sha1 = SHA1.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] combined = encoding.GetBytes(pass);
            sha1.ComputeHash(combined);
            return Convert.ToBase64String(sha1.Hash);
        }
    }
}