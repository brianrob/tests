using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace activityid
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid activityId = new Guid("10000000-0000-0000-0000-000000000001");
            
            Console.WriteLine($"Setting the activity ID to {activityId.ToString()}.");
            EventSource.SetCurrentThreadActivityId(activityId);
            Debug.Assert(activityId == EventSource.CurrentThreadActivityId);

            TestEventSource.Log.TestEvent(1, "hello world!");
        }
    }

    [EventSource]
    public class TestEventSource : EventSource
    {
        public static TestEventSource Log = new TestEventSource();

        private TestEventSource() : base(true)
        {
        }

        [Event(1)]
        public void TestEvent(int intArg, string strArg)
        {
            WriteEvent(1, intArg, strArg);
        }
    }
}
