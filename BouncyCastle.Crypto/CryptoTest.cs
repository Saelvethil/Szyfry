using System;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;

namespace crypto_test
{
    public class Des3
    {
        static public byte[] keyDES3;

        static public byte[] enc;

        static public byte[] dec;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            for (int j = 0; j < 10; j++)
            {
                InitDES3Key();
                byte[] b = stringToByteArray("This is a secret message that should be encoded by DES");
                byte[] e = EncryptDES3_CBC(b);
                string enciphered = byteArrayToString(e);
                if (enciphered.Length > b.Length)
                {
                    enciphered = enciphered.Substring(0, b.Length);
                }
                else if (enciphered.Length < b.Length)
                {
                    for (int i = 0; i <= b.Length - enciphered.Length; i++)
                    {
                        enciphered += "\0";
                    }
                    if (enciphered.Length < b.Length)
                    {
                        for (int i = 0; i <= b.Length - enciphered.Length; i++)
                        {
                            enciphered += "\0";
                        }
                    }
                }
                var m = DecryptDES3_CBC(e);
                string s = byteArrayToString(m);
            }

            
            Console.ReadKey();

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

        static public void InitDES3Key()
        {
            CipherKeyGenerator cipherKeyGenerator = new CipherKeyGenerator();
            cipherKeyGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 192));
            //192 specifies the size of key in bits i.e 24 bytes

            keyDES3 = cipherKeyGenerator.GenerateKey();
        }



        /// <summary>
        /// Encryption using DES3 algorithm in CBC mode 
        /// </summary>
        /// <param name="message">Input message bytes</param>
        /// <returns>Encrypted message bytes</returns>
        static public byte[] EncryptDES3_CBC(byte[] message)
        {
            DesEdeEngine desedeEngine = new DesEdeEngine();
            BufferedBlockCipher bufferedCipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(desedeEngine));
            KeyParameter keyparam = ParameterUtilities.CreateKeyParameter("DESEDE", keyDES3);
            byte[] output = new byte[bufferedCipher.GetOutputSize(message.Length)];
            bufferedCipher.Init(true, keyparam);
            enc = bufferedCipher.DoFinal(message);
            return enc;
        }


      

        static public byte[] DecryptDES3_CBC(byte[] message)
        {
            DesEdeEngine desedeEngine = new DesEdeEngine();
            BufferedBlockCipher bufferedCipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(desedeEngine));
            KeyParameter keyparam = ParameterUtilities.CreateKeyParameter("DESEDE", keyDES3);
            byte[] output = new byte[bufferedCipher.GetOutputSize(message.Length)];
            bufferedCipher.Init(false, keyparam);
            dec = bufferedCipher.DoFinal(message);
            return dec;
        }

    }

    }
