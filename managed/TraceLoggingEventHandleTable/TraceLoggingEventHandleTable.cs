using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace TraceLoggingEventHandleMap
{
    internal sealed class TraceLoggingEventHandleTable
    {
        private const int DefaultLength = 10;
        private int m_numDeclaredEvents;
        private IntPtr[] m_innerTable;
        private object m_innerTableUpdateLock = new object();
        internal TraceLoggingEventHandleTable(int numDeclaredEvents)
        {
            if (numDeclaredEvents < 0)
            {
                throw new InvalidOperationException();
            }
            m_numDeclaredEvents = numDeclaredEvents;
            m_innerTable = new IntPtr[DefaultLength];
        }

        internal int Length
        {
            get { return Volatile.Read(ref m_innerTable).Length; }
        }

        internal IntPtr this[int eventID]
        {
            get
            {
                IntPtr ret = IntPtr.Zero;
                IntPtr[] innerTable = Volatile.Read(ref m_innerTable);

                int index = EventIDToIndex(eventID);
                if (index >= 0 && index < innerTable.Length)
                {
                    ret = innerTable[index];
                }

                return ret;
            }
        }

        internal void SetEventHandle(int eventID, IntPtr eventHandle)
        {
            lock (m_innerTableUpdateLock)
            {
                int index = EventIDToIndex(eventID);
                
                if(index >= m_innerTable.Length)
                {
                    int newSize = m_innerTable.Length * 2;
                    if (newSize <= index)
                    {
                        newSize = index + 1;
                    }

                    IntPtr[] newTable = new IntPtr[newSize];
                    Array.Copy(m_innerTable, newTable, m_innerTable.Length);
                    Volatile.Write(ref m_innerTable, newTable);
                }

                m_innerTable[index] = eventHandle;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int EventIDToIndex(int eventId)
        {
            // Declared event IDs start at 0 and increase monotonically.
            // This data structure only contains information on TraceLogging events whose IDs are
            // assigned after the declared events.
            return eventId - m_numDeclaredEvents;
        }
    }
}
