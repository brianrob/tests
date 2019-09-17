using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace AppZapper
{
    public static class BinaryModifier
    {
        private static readonly byte[] Zeros = new byte[Config.BlockSize];

        public static void ApplyZeroBlocks(string pathToFile, ZeroBlockList committedList, ZeroBlockList attemptingList)
        {
            using (FileStream stream = new FileStream(pathToFile, FileMode.Open, FileAccess.Write))
            {
                foreach (ulong addr in committedList)
                {
                    long seekAddr = stream.Seek((long)addr, SeekOrigin.Begin);
                    if (seekAddr != (long)addr)
                    {
                        throw new InvalidOperationException("Unable to seek to the requested address.");
                    }

                    stream.Write(Zeros, 0, (int)Config.BlockSize);
                }

                foreach (ulong addr in attemptingList)
                {
                    long seekAddr = stream.Seek((long)addr, SeekOrigin.Begin);
                    if (seekAddr != (long)addr)
                    {
                        throw new InvalidOperationException("Unable to seek to the requested address.");
                    }

                    stream.Write(Zeros, 0, (int)Config.BlockSize);
                }
            }
        }
    }
}
