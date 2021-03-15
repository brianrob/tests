using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace memory_mapped_file_copy
{
    class Program
    {
        private const string AppHostPath = @"C:\Program Files\dotnet\sdk\5.0.200-preview.21077.7\AppHostTemplate\apphost.exe";

        static void Main(string[] args)
        {

            using (MemoryMappedFile srcFile = MemoryMappedFile.CreateFromFile(AppHostPath))
            {
                using(MemoryMappedViewStream inStream = srcFile.CreateViewStream())
                {
                    using(MemoryMappedViewAccessor accessor = srcFile.CreateViewAccessor())
                    using (MemoryMappedFile destFile = MemoryMappedFile.CreateNew(null, accessor.Capacity))
                    {
                        using (MemoryMappedViewStream outStream = destFile.CreateViewStream())
                        {
                            inStream.CopyTo(outStream);
                        }
                    }
                }
            }
        }
    }
}
