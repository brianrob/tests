using Microsoft.Diagnostics.Tracing.Etlx;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Stacks;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace traceevent_cpustacks
{
    class Program
    {
        static void Main(string[] args)
        {
            double start = 120809.610;
            double stop = 120863.655;
            string etlxFilePath = @"C:\Users\brianrob\appdata\Local\Temp\PerfView\PerfViewData_Core.etl_ee65d28d_85e09a35.etlx";

            TraceLog log = new TraceLog(etlxFilePath);

            TraceEvents events = log.Events.Filter((x)=>(x is SampledProfileTraceData && x.TimeStampRelativeMSec >= start && x.TimeStampRelativeMSec < stop  && x.ProcessID == 3332));
            Console.WriteLine(events.Count());
            TraceEventStackSource stackSource = new TraceEventStackSource(events);
            int sourceCount = 0;
            stackSource.ForEach(x => sourceCount++);
            Console.WriteLine(sourceCount);
        }
    }
}
