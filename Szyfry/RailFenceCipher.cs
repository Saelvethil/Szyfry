using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szyfry
{
    public class RailFenceCipher
    {
        public static string Encrypt(string msg, int n)
        {
            if (n == 1) return msg;
            char?[,] tab = new char?[msg.Length, n];
            bool downDirection = true;
            for (int i = 0, j = 0; i < msg.Length; i++)
            {
                tab[i, j] = msg[i];
                if (downDirection)
                {
                    j++;
                    if (j == n - 1) downDirection = false;

                }
                else
                {
                    j--;
                    if (j == 0) downDirection = true;
                }
            }
            StringBuilder result = new StringBuilder(msg.Length / n);
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < msg.Length; i++)
                {
                    if (tab[i, j].HasValue)
                        result.Append(tab[i, j]);
                }
            }
            return result.ToString();
        }

        public static string Decrypt(string msg, int n)
        {
            if (n == 1) return msg;
            List<List<int>> list = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                list.Add(new List<int>());
            }

            bool downDirection = true;
            for (int i = 0, j = 0; i < msg.Length; i++)
            {
                list[j].Add(i);
                if (downDirection)
                {
                    j++;
                    if (j == n - 1) downDirection = false;
                }
                else
                {
                    j--;
                    if (j == 0) downDirection = true;
                }
            }

            int index = 0;
            char[] buffer = new char[msg.Length];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < list[i].Count; j++)
                {
                    buffer[list[i][j]] = msg[index];
                    index++;
                }
            }

            return new string(buffer);
        }
    }
}
