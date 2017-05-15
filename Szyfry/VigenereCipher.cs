using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szyfry
{
    public class VigenereCipher
    {
        public static string Encrypt(string msg, string key)
        {
            int range = 256;
            StringBuilder sb = new StringBuilder(msg.Length);
            for (int i = 0, k = 0; i < msg.Length; i++)
            {
                int value = ((int)msg[i] + (int)key[k]) % range;
                sb.Append((char)value);
                k = (k + 1) % key.Length;
            }

            return sb.ToString();
        }

        public static string Decrypt(string msg, string key)
        {
            int range = 256;
            StringBuilder sb = new StringBuilder(msg.Length);
            for (int i = 0, k = 0; i < msg.Length; i++)
            {
                for (int j = 0; j < range; j++)
                {
                    if (((int)key[k] + j) % range == (int)msg[i])
                    {
                        sb.Append((char)j);
                        break;
                    }
                }
                k = (k + 1) % key.Length;
            }

            return sb.ToString();
        }
    }
}
