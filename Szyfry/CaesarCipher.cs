using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szyfry
{
    public class CaesarCipher
    {
        public static string Encrypt(string msg, int k0, int k1)
        {
            int n = 256;
            StringBuilder sb = new StringBuilder(msg.Length);

            foreach (int c in msg)
            {
                int newChar = (c * k1 + k0) % n;
                sb.Append((char)newChar);
            }

            return sb.ToString();
        }

        public static string Decrypt(string msg, int k0, int k1)
        {
            int n = 256, EulerN = 128;
            StringBuilder sb = new StringBuilder(msg.Length);

            foreach (int c in msg)
            {
                int newChar = (int)(((c + n - k0) * QuickModuloPower(k1, EulerN - 1)) % n);
                sb.Append((char)newChar);
            }

            return sb.ToString();
        }

        public static int QuickModuloPower(int k1, int pow){
            int value = k1;
            for (int i = 2; i <= pow; i++)
            {
                value = (value * k1) % 256;
            }
            return value;
        }
    }
}
