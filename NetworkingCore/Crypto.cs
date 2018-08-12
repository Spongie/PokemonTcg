using System.Security.Cryptography;
using System.Text;

namespace NetworkingCore
{
    public class Crypto
    {
        public static string hashSHA512(string unhashedValue)
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                byte[] hash = shaM.ComputeHash(Encoding.ASCII.GetBytes(unhashedValue));

                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hash)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }

                return stringBuilder.ToString();
            }
        }
    }
}
