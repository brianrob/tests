﻿using System;
using System.IO;

namespace FileStomper
{
    class Program
    {
        private const string ZeroListFilePath = "ZeroList.txt";
        private const string BinaryPath = "ApiTemplate";
        private const int BlockSize = 8192;
        private static readonly byte[] Zeros = new byte[BlockSize];

        public static void Main(string[] args)
        {
            int blocksWritten = 0;

            Console.WriteLine("Writing zeros to binary.");
            using (StreamReader zeroListReader = new StreamReader(ZeroListFilePath))
            {
                using (FileStream binaryStreamWriter = new FileStream(BinaryPath, FileMode.Open, FileAccess.Write))
                {
                    while (!zeroListReader.EndOfStream)
                    {
                        // Get the next offset to zero out.
                        string line = zeroListReader.ReadLine();
                        long offset = Convert.ToInt64(line, 16);

                        long seekAddr = binaryStreamWriter.Seek(offset, SeekOrigin.Begin);
                        if(seekAddr != offset)
                        {
                            throw new InvalidOperationException("Unable to seek to requested address.");
                        }
                        binaryStreamWriter.Write(Zeros, 0, BlockSize);
                        blocksWritten++;
                    }
                }
            }

            Console.WriteLine("Reading the binary back.");
            long zeroBytes = 0;
            using (StreamReader binaryStreamReader = new StreamReader(BinaryPath))
            {
                while(!binaryStreamReader.EndOfStream)
                {
                    int b = binaryStreamReader.BaseStream.ReadByte();
                    if(b == 0)
                    {
                        zeroBytes++;
                    }
                    else if(b == -1)
                    {
                        break;
                    }
                }
            }

            Console.WriteLine($"Zero Bytes Written: {blocksWritten * BlockSize} ({(blocksWritten * BlockSize) / 1024 }kb)");
            Console.WriteLine($"Zero Bytes Read: {zeroBytes} ({zeroBytes / 1024 }kb)");
        }
    }
}
