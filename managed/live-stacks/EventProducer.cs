using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace live_stacks
{
    [EventSource(Name = "EventProducer")]
    internal class EventProducer : EventSource
    {
        internal static EventProducer Log = new EventProducer();

        [Event(1)]
        public void TestEvent()
        {
            WriteEvent(1);
        }

        internal static void LogEvents()
        {
            Task.Factory.StartNew(() =>
            {
                while(true)
                {
                    Log.TestEvent();
                    Thread.Sleep(1000);
                }
            },
            TaskCreationOptions.LongRunning);
        }
    }
}
