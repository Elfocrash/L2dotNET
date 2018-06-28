using System.Security.Cryptography;
using System.Text;

namespace L2dotNET.Utility
{
    public class L2Security
    {
        public static byte[] HashPassword(string password)
        {
            using (SHA256Managed crypt = new SHA256Managed())
            {
                return crypt.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}