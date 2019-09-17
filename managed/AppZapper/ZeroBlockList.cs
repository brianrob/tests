using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppZapper
{
    public sealed class ZeroBlockList : IEnumerable<ulong>
    {
        private SortedSet<ulong> _set = new SortedSet<ulong>();

        public ZeroBlockList()
        {
        }

        private ZeroBlockList(ZeroBlockList list)
        {
            _set.UnionWith(list);
        }

        public int Count
        {
            get { return _set.Count; }
        }

        public void Add(ulong startAddress)
        {
            _set.Add(startAddress);
        }

        public void AddRange(IEnumerable<ulong> startAddresses)
        {
            foreach(ulong addr in startAddresses)
            {
                _set.Add(addr);
            }
        }

        public bool Contains(ulong value)
        {
            return _set.Contains(value);
        }

        public void WriteToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Write the header.
                writer.WriteLine($"BlockSize: {Config.BlockSize / 1024}K");
                writer.WriteLine();

                // Write every block entry.
                foreach (ulong addr in _set)
                {
                    writer.WriteLine("{0:X16}", addr);
                }
            }
        }

        public ZeroBlockList Clone()
        {
            return new ZeroBlockList(this);
        }

        public IEnumerator<ulong> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _set.GetEnumerator();
        }
    }
}
