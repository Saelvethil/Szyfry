//Copyright (c), October 2007, Some Rights Reserved 
//By Murat Firat

using System;
using System.Text;
using System.IO;


namespace Szyfry
{
    public class AESProvider : AES
    {
        public delegate void ProgressEventHandler(object o, ProgressEventArgs args);

        public class ProgressEventArgs : EventArgs
        {
            public ProgressEventArgs()
            { }

            public ProgressEventArgs(int i)
            { progress = i; }

            public ProgressEventArgs(int i, bool b)
            { progress = i; stop = b; }

            public int progress;
            public bool stop;
        }

        public event ProgressEventHandler ProgressChanged;

        public ProgressEventArgs Args = new ProgressEventArgs();

        public AESProvider()
        {
            base.InitializeTables();
        }

        public byte[] Encrypt(byte[] BytesInput, string Key)
        {
            if (this.Key == null) ExpandKey(Key);
            else if (Encoding.ASCII.GetString(this.Key, 0, this.Key.Length) != Key) ExpandKey(Key);
            Args.stop = false;

            byte[] BlockIn = new byte[16];
            byte[] BlockOut = new byte[16];

            //Extend input to multiples of 16
            byte[] BytesOut = new byte[BytesInput.Length +
                ((BytesInput.Length % 16) == 0 ? 0 : (16 - BytesInput.Length % 16))];

            //Encrypt by 16 bytes
            int tmp = BytesInput.Length / 16;
            for (int i = 0; i < tmp; i++)
            {
                Array.Copy(BytesInput, i * 16, BlockIn, 0, 16);
                BlockOut = base.Encrypt128Bit(BlockIn);
                Array.Copy(BlockOut, 0, BytesOut, i * 16, 16);
                if ((Args.progress != (i * 100) / tmp) && ProgressChanged != null)
                {
                    Args.progress = (i * 100) / tmp;
                    ProgressChanged(this, Args);//signal to parent object
                    if (Args.stop)
                        break;
                }
            }

            //Encrypt last 16 bytes(if present)
            int leftBytesNum = BytesInput.Length % 16;
            if (leftBytesNum > 0)
            {
                BlockIn = new byte[16];
                Array.Copy(BytesInput, BytesInput.Length - leftBytesNum, BlockIn, 0, leftBytesNum);
                BlockOut = base.Encrypt128Bit(BlockIn);
                Array.Copy(BlockOut, 0, BytesOut, BytesOut.Length - 16, 16);
            }
            Args.progress = 100;
            if (ProgressChanged != null) ProgressChanged(this, Args);
            return BytesOut;
        }

        public byte[] Decrypt(byte[] BytesInput, string Key)
        {
            if (this.Key == null) ExpandKey(Key);
            else if (Encoding.ASCII.GetString(this.Key, 0, this.Key.Length) != Key) ExpandKey(Key);
            Args.stop = false;

            byte[] BlockIn = new byte[16];
            byte[] BlockOut = new byte[16];

            //Extend input to multiples of 16
            byte[] BytesOut = new byte[BytesInput.Length +
                ((BytesInput.Length % 16) == 0 ? 0 : (16 - BytesInput.Length % 16))];

            //Decrypt by 16 bytes
            int tmp = BytesInput.Length / 16;
            for (int i = 0; i < tmp; i++)
            {
                Array.Copy(BytesInput, i * 16, BlockIn, 0, 16);
                BlockOut = base.Decrypt128Bit(BlockIn);
                Array.Copy(BlockOut, 0, BytesOut, i * 16, 16);
                if ((Args.progress != (i * 100) / tmp) && ProgressChanged != null)
                {
                    Args.progress = (i * 100) / tmp;
                    ProgressChanged(this, Args);//signal to parent object
                    if (Args.stop)
                        break;
                }
            }

            //Decrypt last 16 byte(if present)
            int leftBytesNum = BytesInput.Length % 16;
            if (leftBytesNum > 0)
            {
                BlockIn = new byte[16];
                Array.Copy(BytesInput, BytesInput.Length - leftBytesNum, BlockIn, 0, leftBytesNum);
                BlockOut = base.Decrypt128Bit(BlockIn);
                Array.Copy(BlockOut, 0, BytesOut, BytesOut.Length - 16, 16);
            }
            Args.progress = 100;
            if (ProgressChanged != null) ProgressChanged(this, Args);
            return BytesOut;
        }
    }
}
