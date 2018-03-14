using System;
using System.Diagnostics.Tracing;

namespace eventsource_binary
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();

            byte[] b = new byte[10];

            // Send zero-filled byte array.
            TestEventSource.Log.SendByteArray(b);
            TestEventSource.Log.Write("SendByteArrayWriteOfT", new { byteArray = b });

            // Send one-filled byte array.
            for(int i=0; i<b.Length; i++)
            {
                if(i % 2 == 0)
                    b[i] = 0xCA;
                else
                    b[i] = 0xFE;
            }
            TestEventSource.Log.SendByteArray(b);
            TestEventSource.Log.Write("SendByteArrayWriteOfT", new { byteArray = b });

            TestEventSource.Log.SendString("Hello World!");
            TestEventSource.Log.SendString(null);
            TestEventSource.Log.SendInt(2);
            TestEventSource.Log.SendByteArray(null);
        }
    }

    public sealed class TestEventSource : EventSource
    {
        public static TestEventSource Log = new TestEventSource();

        private TestEventSource()
            : base()
        {
        }

        [Event(1)]
        public void SendByteArray(byte[] b)
        {
            WriteEvent(1, b);
        }
        
        [Event(2)]
        public void SendString(string s)
        {
            WriteEvent(2, s);
        }

        [Event(3)]
        public void SendInt(int i)
        {
            WriteEvent(3, i);
        }
    }
}
