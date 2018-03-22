using System;
using System.Diagnostics.Tracing;
using System.Text;

namespace eventsource_hugeevent
{
    [EventSource]
    class MySource : EventSource
    {
        public static MySource Log = new MySource();

        private MySource()
            : base()
        {
        }

        [Event(1)]
        public void SendEvent(string s1, string s2)
        {
            WriteEvent(1, s1, s2);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Building a very large string.");
            int stringSize = 90947;
            StringBuilder builder = new StringBuilder(stringSize);
            for(int i=0; i<stringSize; i++)
            {
                builder.Append('A');
            }
            Console.WriteLine("Writing an event with a very large string.");
            MySource.Log.SendEvent("Foo", builder.ToString());
        }
    }
}
