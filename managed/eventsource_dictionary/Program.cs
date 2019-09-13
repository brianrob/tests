using System;
using System.Diagnostics.Tracing;
using System.Collections.Generic;

namespace eventsource_dictionary
{
    public sealed class TestEventSource : EventSource
    {
        public static TestEventSource Log = new TestEventSource();

        public TestEventSource() : base(true)
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string,string> foo = new Dictionary<string,string>();
            foo["foo"] = "bar";
            TestEventSource.Log.Write<Dictionary<string,string>>("Event",foo);
//            TestEventSource.Log.Write("Event", new { Payload = foo });
            Console.WriteLine("Hello World!");
        }
    }
}
