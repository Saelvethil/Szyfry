using Enigma.Gui;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Szyfry;

namespace SzyfryGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EnigmaWindow enigma;
        List<StringBuilder> blocks;
        byte[] message;
        string messageString;
        CipherKeyGenerator cipherKeyGenerator;
        AESProvider AESProvider;
        string lastExt = ".txt";
        RSAParameters RSAPrivateParameters;
        RSAParameters RSAPublicParameters;
        CngKey DHKey;
        byte[] DHKeyBlob, ForeignDHKeyBlob;
        string fileKey = "ÉÏ9ÝÌ9{Û$àíp?ó¤K	ã;U";

        public MainWindow()
        {
            InitializeComponent();
            enigma = new EnigmaWindow();
            enigma.enigmaForm.Lacznica.createButtons();
            enigma.Show();
            enigma.Top = Top + Height;
            enigma.Left = Left;
            cipherKeyGenerator = new CipherKeyGenerator();
            cipherKeyGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 192));
            GenerateDES3Key();
            AESProvider = new AESProvider();
            using (RSACryptoServiceProvider RSAProviderPrivate = new RSACryptoServiceProvider())
            {
                RSAPrivateParameters = RSAProviderPrivate.ExportParameters(true);
                RSAPublicParameters = RSAProviderPrivate.ExportParameters(false);
            }
            RSAPrivateKey.Text = GetHexString(RSAPrivateParameters.Modulus);
            RSAPublicKey.Text = GetHexString(RSAPublicParameters.Modulus);
            GenerateAESKey();
            GenerateNewDHKey();
        }

        private void LoadMsgFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "files");
            dlg.RestoreDirectory = true;
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                lastExt = System.IO.Path.GetExtension(filename);
                message = File.ReadAllBytes(filename);
                StringBuilder temp = new StringBuilder(message.Length);
                for (int i = 0; i < message.Length; i++)
                {
                    temp.Append((char)message[i]);
                }
                if (lastExt == ".txt")
                {
                    MessageTextBox.Text = temp.ToString();
                }
                else
                {
                    messageString = temp.ToString();
                    MessageTextBox.Text = "Wczytano plik.";
                }

            }
            CreateBlocks();
        }

        private void SaveMsgFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "files");
            dlg.RestoreDirectory = true;
            dlg.FileName = "file";
            dlg.DefaultExt = lastExt;
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                if (lastExt == ".txt")
                {
                    byte[] bytes = new byte[MessageTextBox.Text.Length];
                    for (int i = 0; i < MessageTextBox.Text.Length; i++)
                    {
                        bytes[i] = (byte)MessageTextBox.Text[i];
                    }
                    File.WriteAllBytes(filename, bytes);
                }
                else
                {
                    byte[] bytes = new byte[messageString.Length];
                    for (int i = 0; i < messageString.Length; i++)
                    {
                        bytes[i] = (byte)messageString[i];
                    }
                    File.WriteAllBytes(filename, bytes);
                    MessageTextBox.Text = "Zapisano plik.";
                }
            }
        }

        private void LoadEncryptFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "keys");
            dlg.RestoreDirectory = true;
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    string filename = dlg.FileName;
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

                    using (StringReader output = new StringReader(byteArrayToString(outputAES).Substring(0, input.Length - ct)))
                    {
                        railStep.Text = output.ReadLine();
                        ColumnarKey.Text = output.ReadLine();
                        ColumnarKey2.Text = output.ReadLine();
                        CaesarKey0.Text = output.ReadLine();
                        CaesarKey1.Text = output.ReadLine();
                        VigenereKey.Text = output.ReadLine();
                        DES3Key.Text = output.ReadLine();
                        AESKey.Text = output.ReadLine();

                        using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                        {
                            RSA.FromXmlString(output.ReadLine());
                            RSAPublicParameters = RSA.ExportParameters(false);
                            RSA.FromXmlString(output.ReadLine());
                            RSAPrivateParameters = RSA.ExportParameters(true);
                            updateVisualRSAKeys();
                        }
                        enigma.enigmaForm.Reflektor.SelectedIndex = int.Parse(output.ReadLine());
                        enigma.enigmaForm.Wirnik3.cbRotor.SelectedIndex = int.Parse(output.ReadLine());
                        enigma.enigmaForm.Wirnik3.numRotor.Value = int.Parse(output.ReadLine());
                        enigma.enigmaForm.Wirnik3.cbRotorPos.SelectedIndex = int.Parse(output.ReadLine());
                        enigma.enigmaForm.Wirnik2.cbRotor.SelectedIndex = int.Parse(output.ReadLine());
                        enigma.enigmaForm.Wirnik2.numRotor.Value = int.Parse(output.ReadLine());
                        enigma.enigmaForm.Wirnik2.cbRotorPos.SelectedIndex = int.Parse(output.ReadLine());
                        enigma.enigmaForm.Wirnik1.cbRotor.SelectedIndex = int.Parse(output.ReadLine());
                        enigma.enigmaForm.Wirnik1.numRotor.Value = int.Parse(output.ReadLine());
                        enigma.enigmaForm.Wirnik1.cbRotorPos.SelectedIndex = int.Parse(output.ReadLine());
                        StringBuilder str = new StringBuilder(output.ReadLine());
                        UcPlugBoard lacznica = enigma.enigmaForm.Lacznica;
                        lacznica.myPlugs.Clear();
                        enigma.enigmaForm.myPlugBoard.myPlugs.Clear();
                        if (str.Length > 0 && str.Length % 2 == 0)
                        {
                            for (int i = 0; i < str.Length; i += 2)
                            {
                                lacznica.AddPlugByChar(str[i]);
                                lacznica.AddPlugByChar(str[i + 1]);
                            }
                        }
                        int blockCount = int.Parse(output.ReadLine());
                        BlockSlider.Value = blockCount;
                        int counter = 0;
                        IEnumerator enumerator = ComboBoxesPanel.Children.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            if (counter < blockCount)
                            {
                                ((ComboBox)(enumerator.Current)).SelectedIndex = int.Parse(output.ReadLine());
                                counter++;
                            }
                            else break;
                        }
                    }

                }
                catch { Console.WriteLine("Blad"); }
            }
            CreateBlocks();
        }

        private void SaveEncryptFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "keys");
            dlg.RestoreDirectory = true;
            dlg.FileName = "settings";
            dlg.DefaultExt = ".conf";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                int blockCount = (int)BlockSlider.Value;
                StringBuilder input = new StringBuilder();

                input.AppendLine(railStep.Text); //klucze
                input.AppendLine(ColumnarKey.Text);
                input.AppendLine(ColumnarKey2.Text);
                input.AppendLine(CaesarKey0.Text);
                input.AppendLine(CaesarKey1.Text);
                input.AppendLine(VigenereKey.Text);
                input.AppendLine(DES3Key.Text);
                input.AppendLine(AESKey.Text);

                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAPublicParameters);
                    input.AppendLine(RSA.ToXmlString(false));
                    RSA.ImportParameters(RSAPrivateParameters);
                    input.AppendLine(RSA.ToXmlString(true));
                }
                input.AppendLine(enigma.enigmaForm.Reflektor.SelectedIndex.ToString());
                input.AppendLine(enigma.enigmaForm.Wirnik3.cbRotor.SelectedIndex.ToString());
                input.AppendLine(enigma.enigmaForm.Wirnik3.numRotor.Value.ToString());
                input.AppendLine(enigma.enigmaForm.Wirnik3.cbRotorPos.SelectedIndex.ToString());
                input.AppendLine(enigma.enigmaForm.Wirnik2.cbRotor.SelectedIndex.ToString());
                input.AppendLine(enigma.enigmaForm.Wirnik2.numRotor.Value.ToString());
                input.AppendLine(enigma.enigmaForm.Wirnik2.cbRotorPos.SelectedIndex.ToString());
                input.AppendLine(enigma.enigmaForm.Wirnik1.cbRotor.SelectedIndex.ToString());
                input.AppendLine(enigma.enigmaForm.Wirnik1.numRotor.Value.ToString());
                input.AppendLine(enigma.enigmaForm.Wirnik1.cbRotorPos.SelectedIndex.ToString());
                Dictionary<char, char> plugs = enigma.enigmaForm.Lacznica.myPlugs;
                foreach (KeyValuePair<char, char> pair in plugs)
                {
                    input.Append(pair.Key + "" + pair.Value);
                }
                input.AppendLine();
                input.AppendLine(blockCount.ToString()); //ilość bloków
                int counter = 0;
                IEnumerator enumerator = ComboBoxesPanel.Children.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (counter < blockCount)
                    {
                        input.AppendLine(((ComboBox)(enumerator.Current)).SelectedIndex.ToString()); //numer algorytmu
                        counter++;
                    }
                    else break;
                }

                byte[] inputAES = stringToByteArray(input.ToString());
                byte[] outputAES = AESProvider.Encrypt(inputAES, fileKey);
                StringBuilder output = new StringBuilder(byteArrayToString(outputAES));
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filename, false))
                {
                    file.Write(output.ToString());
                }
            }
        }

        private void BlockEncrypt_Click(object sender, RoutedEventArgs e)
        {
            CreateBlocks();
            IEnumerator enumerator = ComboBoxesPanel.Children.GetEnumerator();
            for (int i = 0; i < blocks.Count; i++)
            {
                enumerator.MoveNext();
                int index = ((ComboBox)(enumerator.Current)).SelectedIndex;

                switch (index)
                {
                    case 0:
                        blocks[i] = new StringBuilder(RailFenceCipher.Encrypt(blocks[i].ToString(), Convert.ToInt32(railStep.Text)));
                        break;
                    case 1:
                        string key = ColumnarKey.Text;
                        int[] intKey = new int[key.Length];
                        for (int j = 0; j < key.Length; j++)
                        {
                            intKey[j] = key[j] - 48;
                        }
                        blocks[i] = new StringBuilder(ColumnarTranspositionCipher.Encrypt(blocks[i].ToString(), intKey));
                        break;
                    case 2:
                        blocks[i] = new StringBuilder(ColumnarTranspositionCipher2.Encrypt(blocks[i].ToString(), ColumnarKey2.Text));
                        break;
                    case 3:
                        blocks[i] = new StringBuilder(CaesarCipher.Encrypt(blocks[i].ToString(), Convert.ToInt32(CaesarKey0.Text), Convert.ToInt32(CaesarKey1.Text)));
                        break;
                    case 4:
                        blocks[i] = new StringBuilder(VigenereCipher.Encrypt(blocks[i].ToString(), VigenereKey.Text));
                        break;
                    case 5:
                        enigma.enigmaForm.tbInput.Text = "A";
                        enigma.enigmaForm.tbInput.Text = "";
                        blocks[i] = new StringBuilder(enigma.enigmaForm.myEnigmaMachine.Encode(blocks[i].ToString()));
                        break;
                    case 6:
                        byte[] input = stringToByteArray(blocks[i].ToString());
                        byte[] output = EncryptDES3_CBC(input);
                        blocks[i] = new StringBuilder(byteArrayToString(output));
                        break;
                    case 7:
                        byte[] inputAES = stringToByteArray(blocks[i].ToString());
                        byte[] outputAES = AESProvider.Encrypt(inputAES, AESKey.Text);
                        blocks[i] = new StringBuilder(byteArrayToString(outputAES));
                        break;
                    case 8:
                        blocks[i] = new StringBuilder(RSACipher.RSAEncrypt(blocks[i].ToString(), RSAPublicParameters));
                        break;
                }
            }
            messageString = "";
            MessageTextBox.Text = "";
            if (lastExt == ".txt")
            {
                blocks.ForEach(str =>
                {
                    MessageTextBox.Text += str;
                });
            }
            else
            {
                blocks.ForEach(str =>
                {
                    messageString += str;
                });
                MessageTextBox.Text = "Zaszyfrowano plik.";
            }

            CreateBlocks();
        }

        private void BlockDecrypt_Click(object sender, RoutedEventArgs e)
        {
            CreateBlocks();
            IEnumerator enumerator = ComboBoxesPanel.Children.GetEnumerator();
            for (int i = 0; i < blocks.Count; i++)
            {
                enumerator.MoveNext();
                int index = ((ComboBox)(enumerator.Current)).SelectedIndex;

                switch (index)
                {
                    case 0:
                        blocks[i] = new StringBuilder(RailFenceCipher.Decrypt(blocks[i].ToString(), Convert.ToInt32(railStep.Text)));
                        break;
                    case 1:
                        string key = ColumnarKey.Text;
                        int[] intKey = new int[key.Length];
                        for (int j = 0; j < key.Length; j++)
                        {
                            intKey[j] = key[j] - 48;
                        }
                        blocks[i] = new StringBuilder(ColumnarTranspositionCipher.Decrypt(blocks[i].ToString(), intKey));
                        break;
                    case 2:
                        blocks[i] = new StringBuilder(ColumnarTranspositionCipher2.Decrypt(blocks[i].ToString(), ColumnarKey2.Text));
                        break;
                    case 3:
                        blocks[i] = new StringBuilder(CaesarCipher.Decrypt(blocks[i].ToString(), Convert.ToInt32(CaesarKey0.Text), Convert.ToInt32(CaesarKey1.Text)));
                        break;
                    case 4:
                        blocks[i] = new StringBuilder(VigenereCipher.Decrypt(blocks[i].ToString(), VigenereKey.Text));
                        break;
                    case 5:
                        enigma.enigmaForm.tbInput.Text = "A";
                        enigma.enigmaForm.tbInput.Text = "";
                        blocks[i] = new StringBuilder(enigma.enigmaForm.myEnigmaMachine.Encode(blocks[i].ToString()));
                        break;
                    case 6:
                        byte[] input = stringToByteArray(blocks[i].ToString());
                        byte[] output = DecryptDES3_CBC(input);
                        blocks[i] = new StringBuilder(byteArrayToString(output));
                        break;
                    case 7:
                        byte[] inputAES = stringToByteArray(blocks[i].ToString());
                        byte[] outputAES = AESProvider.Decrypt(inputAES, AESKey.Text);
                        int ct = 0, k = blocks[i].Length - 1;
                        while (outputAES[k] == 0 && k >= 0)
                        {
                            ct++;
                            k--;
                        }
                        blocks[i] = new StringBuilder(byteArrayToString(outputAES));
                        blocks[i] = new StringBuilder(blocks[i].ToString().Substring(0, blocks[i].Length - ct));
                        break;
                    case 8:
                        blocks[i] = new StringBuilder(RSACipher.RSADecrypt(blocks[i].ToString(), RSAPrivateParameters));
                        break;
                }
            }
            messageString = "";
            MessageTextBox.Text = "";
            if (lastExt == ".txt")
            {
                blocks.ForEach(str =>
                {
                    MessageTextBox.Text += str;
                });
            }
            else
            {
                blocks.ForEach(str =>
                {
                    messageString += str;
                });
                MessageTextBox.Text = "Odszyfrowano plik.";
            }
            CreateBlocks();
        }

        private void BlockSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                int value = (int)e.NewValue;
                BlockShowSlider.Maximum = e.NewValue;
                IEnumerator enumerator = ComboBoxesPanel.Children.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    ((UIElement)(enumerator.Current)).IsEnabled = false;
                }
                enumerator.Reset();
                int counter = 0;
                while (enumerator.MoveNext())
                {
                    if (counter < value)
                    {
                        ((UIElement)(enumerator.Current)).IsEnabled = true;
                        counter++;
                    }
                    else break;
                }
                CreateBlocks();
            }
            catch { }
        }

        private void CreateBlocks()
        {
            int value = (int)BlockSlider.Value;
            blocks = new List<StringBuilder>();
            int lettersPerBlock;
            int remainder;
            string message;
            if (lastExt == ".txt")
            {
                for (int i = 0; i < value; i++)
                {
                    blocks.Add(new StringBuilder(MessageTextBox.Text.Length / value));
                }
                lettersPerBlock = MessageTextBox.Text.Length / blocks.Count;
                remainder = MessageTextBox.Text.Length % blocks.Count;
                message = MessageTextBox.Text;
            }
            else
            {
                for (int i = 0; i < value; i++)
                {
                    blocks.Add(new StringBuilder(messageString.Length / value));
                }
                lettersPerBlock = messageString.Length / blocks.Count;
                remainder = messageString.Length % blocks.Count;
                message = messageString;
            }

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
            if (lastExt == ".txt")
            {
                BlockTextBox.Text = blocks[(int)BlockShowSlider.Value - 1].ToString();
            }
            else
            {
                BlockTextBox.Text = "Podgląd niedostępny dla plików binarnych.";
            }
        }

        private void BlockShowSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                CreateBlocks();
            }
            catch { };

        }

        private void EnigmaButton_Click(object sender, RoutedEventArgs e)
        {
            if (enigma.Visibility == Visibility.Visible) enigma.Hide();
            else if (enigma.Visibility == Visibility.Hidden)
            {
                enigma.Show();
                enigma.Top = Top + Height;
                enigma.Left = Left;
            }

        }

        private void Des3Button_Click(object sender, RoutedEventArgs e)
        {
            GenerateDES3Key();
        }


        private void AESButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateAESKey();
        }

        private void GenerateDES3Key()
        {
            DES3Key.Text = GenerateKey();
        }

        private void GenerateAESKey()
        {
            AESKey.Text = GenerateKey();
        }

        private string GenerateKey()
        {
            byte[] key;
            string stringKey;
            do
            {
                key = cipherKeyGenerator.GenerateKey();
                stringKey = byteArrayToString(key);
            }
            while (stringKey.Any(x => Char.IsWhiteSpace(x)));
            return stringKey;

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

        public byte[] EncryptDES3_CBC(byte[] message)
        {
            DesEdeEngine desedeEngine = new DesEdeEngine();
            BufferedBlockCipher bufferedCipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(desedeEngine));
            byte[] key = stringToByteArray(DES3Key.Text);
            KeyParameter keyparam = ParameterUtilities.CreateKeyParameter("DESEDE", key);
            byte[] output = new byte[bufferedCipher.GetOutputSize(message.Length)];
            bufferedCipher.Init(true, keyparam);
            byte[] result = bufferedCipher.DoFinal(message);
            return result;
        }
        public byte[] DecryptDES3_CBC(byte[] message)
        {
            DesEdeEngine desedeEngine = new DesEdeEngine();
            BufferedBlockCipher bufferedCipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(desedeEngine));
            byte[] key = stringToByteArray(DES3Key.Text);
            KeyParameter keyparam = ParameterUtilities.CreateKeyParameter("DESEDE", key);
            byte[] output = new byte[bufferedCipher.GetOutputSize(message.Length)];
            bufferedCipher.Init(false, keyparam);
            byte[] result = bufferedCipher.DoFinal(message);
            return result;
        }


        private void RSACreateKeyButton_Click(object sender, RoutedEventArgs e)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSAPrivateParameters = RSA.ExportParameters(true);
                RSAPublicParameters = RSA.ExportParameters(false);
            }
            updateVisualRSAKeys();
        }

        private void RSALoadPublicKeyButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "keys");
            dlg.RestoreDirectory = true;
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    string filename = dlg.FileName;
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
                    string publicKey = byteArrayToString(outputAES);
                    using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                    {
                        RSA.FromXmlString(publicKey);
                        try
                        {
                            RSA.ExportParameters(true);
                        }
                        catch (Exception)
                        {
                            RSAPublicParameters = RSA.ExportParameters(false);
                            updateVisualRSAKeys();
                            return;
                        }

                        MessageBox.Show("Nie można wczytać klucza prywatnego jako klucz publiczny.");
                    }

                }
                catch { }
            }
        }

        private void RSASavePublicKeyButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "keys");
            dlg.RestoreDirectory = true;
            dlg.FileName = "publicKey";
            dlg.DefaultExt = ".pub";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                string publicKey;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAPublicParameters);
                    publicKey = RSA.ToXmlString(false);
                }
                byte[] inputAES = stringToByteArray(publicKey);
                byte[] outputAES = AESProvider.Encrypt(inputAES, fileKey);
                StringBuilder output = new StringBuilder(byteArrayToString(outputAES));
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filename, false))
                {
                    file.Write(output.ToString());
                }
            }
        }

        private void RSALoadPrivateKeyButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "keys");
            dlg.RestoreDirectory = true;
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    string filename = dlg.FileName;
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
                    string privateKey = byteArrayToString(outputAES);
                    using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                    {
                        RSA.FromXmlString(privateKey);
                        try
                        {
                            RSAPrivateParameters = RSA.ExportParameters(true);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Nie można wczytać klucza publicznego jako klucz prywatny.");
                            return;
                        }
                    }
                    updateVisualRSAKeys();
                }
                catch { }
            }
        }

        private void RSASavePrivateKeyButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "keys");
            dlg.RestoreDirectory = true;
            dlg.FileName = "privateKey";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                string privateKey;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAPrivateParameters);
                    privateKey = RSA.ToXmlString(true);
                }
                byte[] inputAES = stringToByteArray(privateKey);
                byte[] outputAES = AESProvider.Encrypt(inputAES, fileKey);
                StringBuilder output = new StringBuilder(byteArrayToString(outputAES));
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filename, false))
                {
                    file.Write(output.ToString());
                }
            }
        }

        private void updateVisualRSAKeys()
        {
            RSAPrivateKey.Text = GetHexString(RSAPrivateParameters.Modulus);
            RSAPublicKey.Text = GetHexString(RSAPublicParameters.Modulus);
        }

        private string GetHexString(byte[] byteArray)
        {
            if (byteArray.Length != 0)
            {
                StringBuilder hexString = new StringBuilder(byteArray.Length * 2);
                for (int i = 0; i < byteArray.Length; i++)
                    hexString.Append(string.Format("{0:X}", byteArray[i]));
                int x = hexString.Capacity;
                return hexString.ToString();
            }
            else return "";
        }

        private void GenerateHashButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateHashFromMessage();
        }

        private void GenerateHashFromMessage()
        {
            byte[] input;
            if (lastExt == ".txt")
            {
                input = new byte[MessageTextBox.Text.Length];
                for (int i = 0; i < MessageTextBox.Text.Length; i++)
                {
                    input[i] = (byte)MessageTextBox.Text[i];
                }
            }
            else
            {
                input = new byte[messageString.Length];
                for (int i = 0; i < messageString.Length; i++)
                {
                    input[i] = (byte)messageString[i];
                }
            }

            SHA256Managed SHA256 = new SHA256Managed();
            SHA256.ComputeHash(input);
            HashTextBox.Text = byteArrayToString(SHA256.Hash);
        }

        private void SaveHashFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "hashes");
            dlg.RestoreDirectory = true;
            dlg.FileName = "SHA256Hash";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                StringBuilder output = new StringBuilder(HashTextBox.Text);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filename, false))
                {
                    file.Write(output.ToString());
                }
            }
        }

        private void LoadHashFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "hashes");
            dlg.RestoreDirectory = true;
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    string filename = dlg.FileName;
                    CompareHashTextBox.Text = File.ReadAllText(filename);
                }
                catch { }
            }
        }

        private void CompareHashButton_Click(object sender, RoutedEventArgs e)
        {
            if (HashTextBox.Text == CompareHashTextBox.Text)
            {
                MessageBox.Show("Skróty są zgodne.");
            }
            else
            {
                MessageBox.Show("Skróty są niezgodne.");
            }
        }

        private void GenerateSignatureButton_Click(object sender, RoutedEventArgs e)
        {
            if (lastExt == ".txt")
            {
                SignatureTextBox.Text = RSACipher.RSASignData(MessageTextBox.Text, RSAPrivateParameters);
            }
            else
            {
                SignatureTextBox.Text = RSACipher.RSASignData(messageString, RSAPrivateParameters);
            }
        }

        private void SaveSignatureFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "hashes");
            dlg.RestoreDirectory = true;
            dlg.FileName = "Signature";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                byte[] signature = stringToByteArray(SignatureTextBox.Text);
                File.WriteAllBytes(filename, signature);
            }
        }

        private void LoadSignatureFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "hashes");
            dlg.RestoreDirectory = true;
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    string filename = dlg.FileName;
                    SignatureTextBox.Text = byteArrayToString(File.ReadAllBytes(filename));
                }
                catch { }
            }
        }

        private void VerifySignatureButton_Click(object sender, RoutedEventArgs e)
        {
            string msg;
            if (lastExt == ".txt")
            {
                msg = MessageTextBox.Text;
            }
            else
            {
                msg = messageString;
            }
            if (RSACipher.RSAVerifyData(msg, SignatureTextBox.Text, RSAPublicParameters))
            {
                MessageBox.Show("Podpis wiadomości jest zgodny.");
            }
            else
            {
                MessageBox.Show("Podpis wiadomości jest niezgodny.");
            }
        }

        private void GenerateDHKeyButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewDHKey();
        }

        private void GenerateNewDHKey()
        {
            DHKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP521);
            DHKeyBlob = DHKey.Export(CngKeyBlobFormat.EccPublicBlob);
            DHKeyTextBox.Text = byteArrayToString(DHKeyBlob);
        }

        private void SaveDHKeyFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "keys");
            dlg.RestoreDirectory = true;
            dlg.FileName = "DHPublicKey";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                File.WriteAllBytes(filename, DHKeyBlob);
            }
        }

        private void LoadDHKeyFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "keys");
            dlg.RestoreDirectory = true;
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                DHKeyBlob = File.ReadAllBytes(filename);
                DHKey = CngKey.Import(DHKeyBlob, CngKeyBlobFormat.EccPublicBlob, CngProvider.MicrosoftSoftwareKeyStorageProvider);
                DHKeyTextBox.Text = byteArrayToString(DHKeyBlob);
            }
        }

        private void LoadForeignDHKeyFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "keys");
            dlg.RestoreDirectory = true;
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                ForeignDHKeyBlob = File.ReadAllBytes(filename);
                ForeignDHKeyTextBox.Text = byteArrayToString(ForeignDHKeyBlob);
            }
        }

        private void GenerateSymmetricDHKeyButton_Click(object sender, RoutedEventArgs e)
        {
            if (DHKeyTextBox.Text.Length > 0 && ForeignDHKeyTextBox.Text.Length > 0)
            {
                using (ECDiffieHellmanCng Algorithm = new ECDiffieHellmanCng(DHKey))
                using (CngKey ForeignPublicKey = CngKey.Import(ForeignDHKeyBlob,
                    CngKeyBlobFormat.EccPublicBlob, CngProvider.MicrosoftSoftwareKeyStorageProvider))
                {
                    SymmetricDHKeyTextBox.Text = byteArrayToString(Algorithm.DeriveKeyMaterial(ForeignPublicKey));
                }
            }
            else
            {
                MessageBox.Show("Musisz najpierw wczytać oba klucze publiczne.");
            }
        }
    }

}
