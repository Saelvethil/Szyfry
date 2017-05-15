using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szyfry
{
    public class ColumnarTranspositionCipher
    {
        public static string Encrypt(string msg, int[] key)
        {
            List<List<char>> columns = new List<List<char>>();
            for (int i = 0; i < key.Length; i++)
            {
                columns.Add(new List<char>());
            }

            int counter = 0;
            foreach (char c in msg)
            {
                columns[counter].Add(c);
                counter = (counter + 1) % key.Length;
            }

            List<List<char>> newColumns = new List<List<char>>();
            foreach (int i in key)
            {
                newColumns.Add(columns.ElementAt(i - 1));
            }
            columns = null;
            double length = (double)msg.Length / key.Length;
            int columnLength = (int)Math.Ceiling(length);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < columnLength; i++)
                for (int j = 0; j < key.Length; j++)
                {
                    try
                    {
                        sb.Append(newColumns[j][i]);
                    }
                    catch (Exception) { }
                }
            return sb.ToString();
        }

        public static string Decrypt(string msg, int[] key)
        {
            List<List<char>> columns = new List<List<char>>();
            for (int i = 0; i < key.Length; i++)
            {
                columns.Add(new List<char>());
            }

            int counter = 0;
            int remainder = msg.Length % key.Length;
            int minimumLength = msg.Length - remainder;

            for (int i = 0; i < minimumLength; i++)
            {
                columns[counter].Add(msg[i]);
                counter = (counter + 1) % key.Length;
                
            }
            int shift = 0;
            counter = 0;
            for (int i = 0; i < key.Length; i++)
            {
                if (key[i] <= remainder)
                {
                    columns[i].Add(msg[msg.Length - remainder + shift]);
                    shift++;
                }
                if (shift == remainder) break;
            }

            List<List<char>> newColumns = new List<List<char>>();
            for (int i = 0; i < key.Length; i++)
            {
                newColumns.Add(new List<char>());
            }
            int index = 0;
            foreach (int i in key)
            {
                List<char> currColumn = columns.ElementAt(index);
                foreach (char c in currColumn)
                    newColumns[i - 1].Add(c);
                index++;
            }
            columns = null;
            double length = (double)msg.Length / key.Length;
            int columnLength = (int)Math.Ceiling(length);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < columnLength; i++)
                for (int j = 0; j < key.Length; j++)
                {
                    try
                    {
                        sb.Append(newColumns[j][i]);
                    }
                    catch (Exception) { }
                }
            return sb.ToString();
        }
    }
}
