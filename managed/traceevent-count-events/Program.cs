using System;
using Microsoft.Diagnostics.Tracing;

namespace traceevent_count_events
{
    class Program
    {
        static void Main(string[] args)
        {
            ulong eventCount = 0;
            using (ETWTraceEventSource source = new ETWTraceEventSource(args[0]))
            {
                source.AllEvents += delegate (TraceEvent data)
                {
                    eventCount++;
                    if(eventCount > 20000000)
                    {
                        source.StopProcessing();
                    }
                };

                source.Process();
            }

            Console.WriteLine($"EventCount = {eventCount}");
        }
    }
}
