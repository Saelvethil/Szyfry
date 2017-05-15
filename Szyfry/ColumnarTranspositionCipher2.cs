using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szyfry
{
    public class ColumnarTranspositionCipher2
    {
        public static string Encrypt(string msg, string key)
        {
            List<StringBuilder> columns = new List<StringBuilder>();
            for (int i = 0; i < key.Length; i++)
            {
                columns.Add(new StringBuilder(msg.Length / key.Length));
            }

            int counter = 0;
            foreach (char c in msg)
            {
                columns[counter].Append(c);
                counter = (counter + 1) % key.Length;
            }

            int[] keyOrder = new int[key.Length];
            char[] keyTemp = key.ToArray();
            char[] keyArray = key.ToArray();
            Array.Sort(keyArray);
            counter = 0;
            foreach (char c in keyArray)
                for (int i = 0; i < key.Length; i++)
                {
                    if (c.Equals(keyTemp[i]))
                    {
                        keyOrder[i] = counter + 1;
                        counter++;
                        keyTemp[i] = '\0';
                        break;
                    }
                }

            List<StringBuilder> newColumns = new List<StringBuilder>();
            for (int i = 0; i < key.Length; i++)
            {
                newColumns.Add(new StringBuilder(msg.Length / key.Length));
            }

            int index = 0;
            foreach (int i in keyOrder)
            {
                StringBuilder currColumn = columns.ElementAt(index);
                for (int j = 0; j < currColumn.Length; j++)
                {
                    newColumns[i - 1].Append(currColumn[j]);
                }        
                index++;
            }
            columns = null;
            StringBuilder sb = new StringBuilder(msg.Length);

            newColumns.ForEach(builder =>
            {
                sb.Append(builder.ToString());
            });

            return sb.ToString();
        }


        public static string Decrypt(string msg, string key)
        {
            if (msg.Length < key.Length)
            {
                key = key.Substring(0, msg.Length);
            }
            List<StringBuilder> columns = new List<StringBuilder>();
            for (int i = 0; i < key.Length; i++)
            {
                columns.Add(new StringBuilder(msg.Length / key.Length));
            }

            int[] keyOrder = new int[key.Length];
            char[] keyTemp = key.ToArray();
            char[] keyArray = key.ToArray();
            Array.Sort(keyArray);
            int counter = 0;
            foreach (char c in keyArray)
                for (int i = 0; i < key.Length; i++)
                {
                    if (c.Equals(keyTemp[i]))
                    {
                        keyOrder[i] = counter + 1;
                        counter++;
                        keyTemp[i] = '\0';
                        break;
                    }
                }

            int remainder = msg.Length % key.Length;
            int[] columnLengths = new int[key.Length];
            for (int i = 0; i < key.Length; i++)
                columnLengths[i] = msg.Length / key.Length;
            for (int i = 0; i < key.Length; i++)
                if (remainder > 0)
                {
                    columnLengths[keyOrder[i] - 1]++;
                    remainder--;
                }

            counter = 0;
            for (int i = 0; i < columns.Count; i++)
            {
                for (int j = 0; j < columnLengths[i]; j++)
                {
                    columns[i].Append(msg[counter]);
                    counter++;
                }
            }

            List<StringBuilder> newColumns = new List<StringBuilder>();
            foreach (int i in keyOrder)
            {
                newColumns.Add(columns.ElementAt(i - 1));
            }
            columns = null;

            double length = (double)msg.Length / key.Length;
            int columnLength = (int)Math.Ceiling(length);
            StringBuilder sb = new StringBuilder(msg.Length);
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
    //    public static string Encrypt(string msg, string key)
    //    {
    //        List<List<char>> columns = new List<List<char>>();
    //        for (int i = 0; i < key.Length; i++)
    //        {
    //            columns.Add(new List<char>());
    //        }

    //        int counter = 0;
    //        foreach (char c in msg)
    //        {
    //            columns[counter].Add(c);
    //            counter = (counter + 1) % key.Length;
    //        }

    //        int[] keyOrder = new int[key.Length];
    //        char[] keyTemp = key.ToArray();
    //        char[] keyArray = key.ToArray();
    //        Array.Sort(keyArray);
    //        counter = 0;
    //        foreach (char c in keyArray)
    //            for (int i = 0; i < key.Length; i++)
    //            {
    //                if (c.Equals(keyTemp[i]))
    //                {
    //                    keyOrder[i] = counter + 1;
    //                    counter++;
    //                    keyTemp[i] = '\0';
    //                    break;
    //                }
    //            }

    //        List<List<char>> newColumns = new List<List<char>>();
    //        for (int i = 0; i < key.Length; i++)
    //        {
    //            newColumns.Add(new List<char>());
    //        }

    //        int index = 0;
    //        foreach (int i in keyOrder)
    //        {
    //            List<char> currColumn = columns.ElementAt(index);
    //            foreach (char c in currColumn)
    //                newColumns[i - 1].Add(c);
    //            index++;
    //        }
    //        columns = null;
    //        StringBuilder sb = new StringBuilder();

    //        newColumns.ForEach(list =>
    //        {
    //            list.ForEach(c => sb.Append(c));
    //        });

    //        return sb.ToString();
    //    }


    //    public static string Decrypt(string msg, string key)
    //    {
    //        if (msg.Length < key.Length)
    //        {
    //            key = key.Substring(0, msg.Length);
    //        }
    //        List<List<char>> columns = new List<List<char>>();
    //        for (int i = 0; i < key.Length; i++)
    //        {
    //            columns.Add(new List<char>());
    //        }

    //        int[] keyOrder = new int[key.Length];
    //        char[] keyTemp = key.ToArray();
    //        char[] keyArray = key.ToArray();
    //        Array.Sort(keyArray);
    //        int counter = 0;
    //        foreach (char c in keyArray)
    //            for (int i = 0; i < key.Length; i++)
    //            {
    //                if (c.Equals(keyTemp[i]))
    //                {
    //                    keyOrder[i] = counter + 1;
    //                    counter++;
    //                    keyTemp[i] = '\0';
    //                    break;
    //                }
    //            }

    //        int remainder = msg.Length % key.Length;
    //        int[] columnLengths = new int[key.Length];
    //        for (int i = 0; i < key.Length; i++)
    //            columnLengths[i] = msg.Length / key.Length;
    //        for (int i = 0; i < key.Length; i++)
    //            if (remainder > 0)
    //            {
    //                columnLengths[keyOrder[i] - 1]++;
    //                remainder--;
    //            }

    //        List<char> message = new List<char>(msg);
    //        for (int i = 0; i < columns.Count; i++)
    //        {
    //            for (int j = 0; j < columnLengths[i]; j++)
    //            {
    //                columns[i].Add(message.First());
    //                message.RemoveAt(0);
    //            }
    //        }

    //        List<List<char>> newColumns = new List<List<char>>();
    //        foreach (int i in keyOrder)
    //        {
    //            newColumns.Add(columns.ElementAt(i - 1));
    //        }
    //        columns = null;

    //        double length = (double)msg.Length / key.Length;
    //        int columnLength = (int)Math.Ceiling(length);
    //        StringBuilder sb = new StringBuilder();
    //        for (int i = 0; i < columnLength; i++)
    //            for (int j = 0; j < key.Length; j++)
    //            {
    //                try
    //                {
    //                    sb.Append(newColumns[j][i]);
    //                }
    //                catch (Exception) { }
    //            }

    //        return sb.ToString();
    //    }
    //}
}
