using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Diagnostics;
using System.Threading;

namespace rundown_example
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TraceEventSession session = new TraceEventSession("MyRundownSession", "rundown.etl"))
            {
                session.EnableProvider(
                    ClrRundownTraceEventParser.ProviderGuid,
                    Microsoft.Diagnostics.Tracing.TraceEventLevel.Verbose,
                    (ulong)(ClrRundownTraceEventParser.Keywords.Loader | ClrRundownTraceEventParser.Keywords.ForceEndRundown));
                
                // Very unsophisticated way to wait for rundown to complete so that we don't miss events.
                Thread.Sleep(1000);
            }
        }
    }
}
