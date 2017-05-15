using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Szyfry
{
    public class RSACipher
    {
        static public string RSAEncrypt(string input, RSAParameters RSAKeyInfo)
        {
            try
            {
                bool DoOAEPPadding = false;
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    int index = 0;
                    int range = input.Length;
                    StringBuilder message = new StringBuilder(range);
                    int blockSize = 16;
                    while (index < range)
                    {
                        string tmp;
                        if (range - index > blockSize - 1)
                        {
                            tmp = input.Substring(index, blockSize);
                            encryptedData = RSA.Encrypt(stringToByteArray(tmp), DoOAEPPadding);
                        }
                        else
                        {
                            tmp = input.Substring(index, range - index);
                            encryptedData = RSA.Encrypt(stringToByteArray(tmp), DoOAEPPadding);
                        }
                        message.Append(byteArrayToString(encryptedData));
                        index += blockSize;
                    }
                    return message.ToString();
                }

            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }

        }

        static public string RSADecrypt(string input, RSAParameters RSAKeyInfo)
        {
            try
            {
                bool DoOAEPPadding = false;
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    int index = 0;
                    int range = input.Length;
                    StringBuilder message = new StringBuilder(range);
                    int blockSize = 128;
                    while (index < range)
                    {
                        string tmp;
                        if (range - index > blockSize - 1)
                        {
                            tmp = input.Substring(index, blockSize);
                            decryptedData = RSA.Decrypt(stringToByteArray(tmp), DoOAEPPadding);
                        }
                        else
                        {
                            tmp = input.Substring(index, range - index);
                            decryptedData = RSA.Decrypt(stringToByteArray(tmp), DoOAEPPadding);
                        }
                        message.Append(byteArrayToString(decryptedData));
                        index += blockSize;
                    }
                    return message.ToString();
                }

            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }

        }

        static public string RSASignData(string input, RSAParameters RSAKeyInfo)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    using (SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider())
                    {
                        encryptedData = RSA.SignData(stringToByteArray(input), new SHA256CryptoServiceProvider());
                    }
                    return byteArrayToString(encryptedData);
                }

            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }

        }

        static public bool RSAVerifyData(string input, string signature, RSAParameters RSAKeyInfo)
        {
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    
                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);
                    using (SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider())
                    {
                        return RSA.VerifyData(stringToByteArray(input), new SHA256CryptoServiceProvider(), stringToByteArray(signature));
                    }
                }

        }

        private static byte[] stringToByteArray(String s)
        {
            byte[] byteArray = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                byteArray[i] = (byte)s[i];
            }
            return byteArray;
        }
        private static string byteArrayToString(byte[] b)
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
