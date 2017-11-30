using System;
using System.Runtime.InteropServices;
using System.Diagnostics.Tracing;

namespace eventsource_32bitlargeaddress
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            // Allocation size greater than 2GB (0x80000000)
            UInt64 allocSize = 0x80000000;
            UInt64 chunkSize = allocSize / 2;
            byte[] a = new byte[chunkSize];
            byte[] buf = new byte[chunkSize];
            fixed(byte *allocStart = &buf[0])
            {
                int *pValueStoredInHighMemLocation = ((int*)(allocStart + chunkSize - 8));
                *pValueStoredInHighMemLocation = 1234;

                Console.WriteLine("Writing 10 events.");
                for(int i=0; i<10; i++)
                {
                    TestEventSource.Log.FireEvent((IntPtr)pValueStoredInHighMemLocation);
                }
                Console.WriteLine("Done.");
            }
            GC.KeepAlive(a);
            GC.KeepAlive(buf);

        }
    }

    [EventSource]
    public sealed class TestEventSource : EventSource
    {
        public static TestEventSource Log = new TestEventSource();

        [Event(1)]
        public unsafe void FireEvent(IntPtr ptr)
        {
            EventSource.EventData* descrs = stackalloc EventSource.EventData[1];
            descrs[0].DataPointer = ptr;
            descrs[0].Size = 4;
            WriteEventCore(1, 1, descrs);
        }
    }
}
