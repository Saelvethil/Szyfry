using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Szyfry;

namespace SzyfryConsole
{
    class Program
    {
        private static BlockEngine engine;
        private static string fileKey = "ÉÏ9ÝÌ9{Û$àíp?ó¤K	ã;U";
        private static AESProvider AESProvider = new AESProvider();
        static void Main(string[] args)
        {
            //args[0] plik z konfiguracja
            //args[1] plik wejsciowy
            //args[2] plik wyjsciowy
            //args[3] szyfrowanie czy deszyfrowanie e d         
            if (args.Length < 4)
            {
                Console.WriteLine("Za mało parametrów wejściowych.");
                Console.WriteLine("Poprawny format: {plik_konfiguracyjny} {plik_wejsciowy} {plik_wyjsciowy} {e|d} gdzie e-szyfrowanie d-deszyfrowanie");
            }
            else
            {
                engine = new BlockEngine();
                LoadConfig(args[0]);
                try
                {
                    byte[] message = File.ReadAllBytes(args[1]);
                    StringBuilder temp = new StringBuilder(message.Length);
                    for (int i = 0; i < message.Length; i++)
                    {
                        temp.Append((char)message[i]);
                    }
                    engine.Message = temp.ToString();
                    Console.Write("Wczytano plik. ");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Błąd związany z plikiem wejściowym: ");
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                    return;
                }
                if (args[3].CompareTo("e") == 0)
                {
                    string result = engine.EncryptBlocks();
                    byte[] bytes = new byte[result.Length];
                    for (int i = 0; i < result.Length; i++)
                    {
                        bytes[i] = (byte)result[i];
                    }
                    File.WriteAllBytes(args[2], bytes);
                    Console.WriteLine("Plik " + args[1] + " został zaszyfrowany do pliku " + args[2]);
                }
                else if (args[3].CompareTo("d") == 0)
                {
                    string result = engine.DecryptBlocks();
                    byte[] bytes = new byte[result.Length];
                    for (int i = 0; i < result.Length; i++)
                    {
                        bytes[i] = (byte)result[i];
                    }
                    File.WriteAllBytes(args[2], bytes);
                    Console.WriteLine("Plik " + args[1] + "został odszyfrowany do pliku " + args[2]);
                }
            }
            Console.ReadKey();
        }

        private static void LoadConfig(string filename)
        {
            try
            {
                string input;
                using (StreamReader sr = new StreamReader(filename))
                {
                    input = sr.ReadToEnd();

                }

                byte[] inputAES = stringToByteArray(input);
                byte[] outputAES = AESProvider.Decrypt(inputAES, fileKey);
                int ct = 0, k = input.Length - 1;
                while (outputAES[k] == 0 && k >= 0)
                {
                    ct++;
                    k--;
                }

                using (StringReader sr = new StringReader(byteArrayToString(outputAES).Substring(0, input.Length - ct)))
                {
                    int railStep, CaesarKey0, CaesarKey1, blockCount;
                    int reflektor, Rotor3, Rotor3Sett, Rotor3Pos, Rotor2, Rotor2Sett, Rotor2Pos;
                    int Rotor1, Rotor1Sett, Rotor1Pos;
                    string plugs;
                    int[] ColumnarKey;
                    Cipher[] algorithms;
                    string ColumnarKey2, VigenereKey;
                    byte[] DES3Key;
                    string AESKey;
                    railStep = int.Parse(sr.ReadLine());
                    string key = sr.ReadLine();
                    ColumnarKey = new int[key.Length];
                    RSAParameters RSAPrivateParameters;
                    RSAParameters RSAPublicParameters;

                    for (int j = 0; j < key.Length; j++)
                    {
                        ColumnarKey[j] = key[j] - 48;
                    }
                    ColumnarKey2 = sr.ReadLine();
                    CaesarKey0 = int.Parse(sr.ReadLine());
                    CaesarKey1 = int.Parse(sr.ReadLine());
                    VigenereKey = sr.ReadLine();
                    DES3Key = stringToByteArray(sr.ReadLine());
                    AESKey = sr.ReadLine();

                    using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                    {
                        RSA.FromXmlString(sr.ReadLine());
                        RSAPublicParameters = RSA.ExportParameters(false);
                        RSA.FromXmlString(sr.ReadLine());
                        RSAPrivateParameters = RSA.ExportParameters(true);
                    }

                    reflektor = int.Parse(sr.ReadLine());
                    Rotor3 = int.Parse(sr.ReadLine());
                    Rotor3Sett = int.Parse(sr.ReadLine());
                    Rotor3Pos = int.Parse(sr.ReadLine());
                    Rotor2 = int.Parse(sr.ReadLine());
                    Rotor2Sett = int.Parse(sr.ReadLine());
                    Rotor2Pos = int.Parse(sr.ReadLine());
                    Rotor1 = int.Parse(sr.ReadLine());
                    Rotor1Sett = int.Parse(sr.ReadLine());
                    Rotor1Pos = int.Parse(sr.ReadLine());
                    plugs = sr.ReadLine();

                    engine.setEnigmaParts(reflektor, Rotor3, Rotor3Sett, Rotor3Pos, Rotor2, Rotor2Sett, Rotor2Pos,
                    Rotor1, Rotor1Sett, Rotor1Pos, plugs);
                    blockCount = int.Parse(sr.ReadLine());
                    algorithms = new Cipher[blockCount];
                    for (int i = 0; i < algorithms.Length; i++)
                    {
                        algorithms[i] = (Cipher)Enum.Parse(typeof(Cipher), sr.ReadLine());
                    }
                    engine.setKeys(railStep, ColumnarKey, ColumnarKey2, CaesarKey0, CaesarKey1, VigenereKey, DES3Key, AESKey);
                    engine.setRSAParameters(RSAPublicParameters, RSAPrivateParameters);
                    engine.setAlgorithms(blockCount, algorithms);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Błąd związany z plikiem konfiguracyjnym: ");
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return;
            }
        }

        static public byte[] stringToByteArray(String s)
        {
            byte[] byteArray = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                byteArray[i] = (byte)s[i];
            }
            return byteArray;
        }
        static public string byteArrayToString(byte[] b)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                str.Append((char)b[i]);
            }
            return str.ToString();
        }
    }
}
