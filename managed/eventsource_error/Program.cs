using System;
using System.Diagnostics.Tracing;

namespace eventsource_error
{
    class Program
    {
        static void Main(string[] args)
        {
            BrokenEventSource.Log.TestEvent("Hello World!");
            FunctionalEventSource.Log.Write("TestEvent", new { arg = "Hello World!" });
        }
    }

    [EventSource(Name = "BrokenEventSource")]
    public sealed class BrokenEventSource : EventSource
    {
        public static BrokenEventSource Log = new BrokenEventSource();

        private BrokenEventSource()
            : base(EventSourceSettings.EtwSelfDescribingEventFormat)
        {
        }

        [Event(0)]
        public void TestEvent(string arg)
        {
            WriteEvent(0, arg);
        }
    }

    [EventSource(Name = "FunctionalEventSource")]
    public sealed class FunctionalEventSource : EventSource
    {
        public static FunctionalEventSource Log = new FunctionalEventSource();

        private FunctionalEventSource()
            : base(EventSourceSettings.EtwSelfDescribingEventFormat)
        {
        }
    }
}
