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

        public ulong GetHighestAddress()
        {
            return _set.Max;
        }

        public void Write(StreamWriter writer)
        {
            foreach(ulong addr in _set)
            {
                writer.WriteLine($"{0:16x}", addr);
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
