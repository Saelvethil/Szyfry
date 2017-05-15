using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Core;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography;

namespace Szyfry
{
    public enum Cipher
    {
        RailFence = 0,
        ColumnarTransposition = 1,
        ColumnarTransposition2 = 2,
        Caesar = 3,
        Vigenere = 4,
        Enigma = 5,
        DES3 = 6,
        AES = 7,
        RSA = 8
    }

    public class BlockEngine
    {
        int railStep;
        int[] ColumnarKey;
        string ColumnarKey2;
        int CaesarKey0;
        int CaesarKey1;
        string VigenereKey;
        byte[] DES3Key;
        string AESKey;
        int blockCount;
        RSAParameters RSAPrivateParameters;
        RSAParameters RSAPublicParameters;
        Cipher[] algorithms;
        StringBuilder message;
        private List<StringBuilder> blocks;
        RotorMachine myRotorMachine;
        PlugBoard myPlugBoard;
        EnigmaMachine myEnigmaMachine;
        AESProvider AESProvider;

        public BlockEngine()
        {
            message = new StringBuilder();
            myRotorMachine = new RotorMachine();
            myPlugBoard = new PlugBoard();
            myEnigmaMachine = new EnigmaMachine(myPlugBoard, myRotorMachine);
            AESProvider = new AESProvider();
        }
        public string Message
        {
            get { return message.ToString(); }
            set
            {
                message.Clear();
                message.Append(value);
            }
        }

        public void setKeys(int rail, int[] columnar, string columnar2, int caesar0, int caesar1, string vigenere, byte[] des3, string aes)
        {
            railStep = rail;
            ColumnarKey = columnar;
            ColumnarKey2 = columnar2;
            CaesarKey0 = caesar0;
            CaesarKey1 = caesar1;
            VigenereKey = vigenere;
            DES3Key = des3;
            AESKey = aes;
        }
        public void setEnigmaParts(int reflektor, int Rotor3, int Rotor3Sett, int Rotor3Pos, int Rotor2, int Rotor2Sett, int Rotor2Pos,
                    int Rotor1, int Rotor1Sett, int Rotor1Pos, string plugs)
        {
            Rotor r1 = myRotorMachine.Rotors.ElementAt(Rotor1).Value;
            Rotor r2 = myRotorMachine.Rotors.ElementAt(Rotor2).Value;
            Rotor r3 = myRotorMachine.Rotors.ElementAt(Rotor3).Value;
            myRotorMachine.SetRotor(1, r1);
            myRotorMachine.SetRotor(2, r2);
            myRotorMachine.SetRotor(3, r3);
            myRotorMachine.SetRotor(4, myRotorMachine.Ukws.ElementAt(reflektor).Value);

            r1.SetRingPosition(Rotor1Sett);
            r2.SetRingPosition(Rotor2Sett);
            r3.SetRingPosition(Rotor3Sett);

            r1.SetRotorPosition(Rotor1Pos);
            r2.SetRotorPosition(Rotor2Pos);
            r3.SetRotorPosition(Rotor3Pos);

            if (plugs.Length > 0 && plugs.Length % 2 == 0)
            {
                for (int i = 0; i < plugs.Length; i += 2)
                {
                    myPlugBoard.AddPlug(plugs[i], plugs[i + 1]);
                }
            }
        }

        public void setRSAParameters(RSAParameters RSAPublicParameters, RSAParameters RSAPrivateParameters)
        {
            this.RSAPublicParameters = RSAPublicParameters;
            this.RSAPrivateParameters = RSAPrivateParameters;
        }
        public void setAlgorithms(int count, Cipher[] algs)
        {
            blockCount = count;
            algorithms = algs;
        }
        private void CreateBlocks()
        {
            if (message.Length > 0)
            {
                blocks = new List<StringBuilder>();
                for (int i = 0; i < blockCount; i++)
                {
                    blocks.Add(new StringBuilder(message.Length / blockCount));
                }
                int lettersPerBlock = message.Length / blocks.Count;
                int remainder = message.Length % blocks.Count;

                int remaining = message.Length;
                int counter = 0;
                for (int i = 0; i < blocks.Count; i++)
                {
                    int count = lettersPerBlock;
                    if (remainder > 0)
                    {
                        count++;
                        remainder--;
                    }
                    for (int j = 0; j < count; j++)
                    {
                        blocks[i].Append(message[counter]);
                        counter++;
                    }
                }
            }
        }

        public string EncryptBlocks()
        {
            CreateBlocks();
            for (int i = 0; i < blocks.Count; i++)
            {
                Cipher cipher = algorithms[i];

                switch (cipher)
                {
                    case Cipher.RailFence:
                        blocks[i] = new StringBuilder(RailFenceCipher.Encrypt(blocks[i].ToString(), railStep));
                        break;
                    case Cipher.ColumnarTransposition:
                        blocks[i] = new StringBuilder(ColumnarTranspositionCipher.Encrypt(blocks[i].ToString(), ColumnarKey));
                        break;
                    case Cipher.ColumnarTransposition2:
                        blocks[i] = new StringBuilder(ColumnarTranspositionCipher2.Encrypt(blocks[i].ToString(), ColumnarKey2));
                        break;
                    case Cipher.Caesar:
                        blocks[i] = new StringBuilder(CaesarCipher.Encrypt(blocks[i].ToString(), CaesarKey0, CaesarKey1));
                        break;
                    case Cipher.Vigenere:
                        blocks[i] = new StringBuilder(VigenereCipher.Encrypt(blocks[i].ToString(), VigenereKey));
                        break;
                    case Cipher.Enigma:
                        blocks[i] = new StringBuilder(myEnigmaMachine.Encode(blocks[i].ToString()));
                        break;
                    case Cipher.DES3:
                        byte[] input = stringToByteArray(blocks[i].ToString());
                        byte[] output = EncryptDES3_CBC(input);
                        blocks[i] = new StringBuilder(byteArrayToString(output));
                        break;
                    case Cipher.AES:
                        byte[] inputAES = stringToByteArray(blocks[i].ToString());
                        byte[] outputAES = AESProvider.Encrypt(inputAES, AESKey);
                        blocks[i] = new StringBuilder(byteArrayToString(outputAES));
                        break;
                    case Cipher.RSA:
                        blocks[i] = new StringBuilder(RSACipher.RSAEncrypt(blocks[i].ToString(), RSAPublicParameters));
                        break;
                }
            }
            string result = "";
            blocks.ForEach(str =>
            {
                result += str;
            });
            return result;
        }
        public string DecryptBlocks()
        {
            CreateBlocks();
            for (int i = 0; i < blocks.Count; i++)
            {
                Cipher cipher = algorithms[i];

                switch (cipher)
                {
                    case Cipher.RailFence:
                        blocks[i] = new StringBuilder(RailFenceCipher.Decrypt(blocks[i].ToString(), railStep));
                        break;
                    case Cipher.ColumnarTransposition:
                        blocks[i] = new StringBuilder(ColumnarTranspositionCipher.Decrypt(blocks[i].ToString(), ColumnarKey));
                        break;
                    case Cipher.ColumnarTransposition2:
                        blocks[i] = new StringBuilder(ColumnarTranspositionCipher2.Decrypt(blocks[i].ToString(), ColumnarKey2));
                        break;
                    case Cipher.Caesar:
                        blocks[i] = new StringBuilder(CaesarCipher.Decrypt(blocks[i].ToString(), CaesarKey0, CaesarKey1));
                        break;
                    case Cipher.Vigenere:
                        blocks[i] = new StringBuilder(VigenereCipher.Decrypt(blocks[i].ToString(), VigenereKey));
                        break;
                    case Cipher.Enigma:
                        blocks[i] = new StringBuilder(myEnigmaMachine.Encode(blocks[i].ToString()));
                        break;
                    case Cipher.DES3:
                        byte[] input = stringToByteArray(blocks[i].ToString());
                        byte[] output = DecryptDES3_CBC(input);
                        blocks[i] = new StringBuilder(byteArrayToString(output));
                        break;
                    case Cipher.AES:
                        byte[] inputAES = stringToByteArray(blocks[i].ToString());
                        byte[] outputAES = AESProvider.Decrypt(inputAES, AESKey);
                        blocks[i] = new StringBuilder(byteArrayToString(outputAES));
                        break;
                    case Cipher.RSA:
                        blocks[i] = new StringBuilder(RSACipher.RSADecrypt(blocks[i].ToString(), RSAPrivateParameters));
                        break;

                }
            }
            string result = "";
            blocks.ForEach(str =>
            {
                result += str;
            });
            return result;
        }

        public byte[] EncryptDES3_CBC(byte[] message)
        {
            DesEdeEngine desedeEngine = new DesEdeEngine();
            BufferedBlockCipher bufferedCipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(desedeEngine));
            KeyParameter keyparam = ParameterUtilities.CreateKeyParameter("DESEDE", DES3Key);
            byte[] output = new byte[bufferedCipher.GetOutputSize(message.Length)];
            bufferedCipher.Init(true, keyparam);
            byte[] result = bufferedCipher.DoFinal(message);
            return result;
        }
        public byte[] DecryptDES3_CBC(byte[] message)
        {
            DesEdeEngine desedeEngine = new DesEdeEngine();
            BufferedBlockCipher bufferedCipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(desedeEngine));
            KeyParameter keyparam = ParameterUtilities.CreateKeyParameter("DESEDE", DES3Key);
            byte[] output = new byte[bufferedCipher.GetOutputSize(message.Length)];
            bufferedCipher.Init(false, keyparam);
            byte[] result = bufferedCipher.DoFinal(message);
            return result;
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

        static public byte[] stringToByteArray(String s)
        {
            byte[] byteArray = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                byteArray[i] = (byte)s[i];
            }
            return byteArray;
        }

    }
}
