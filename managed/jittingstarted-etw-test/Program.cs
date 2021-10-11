using System;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;
using Microsoft.Diagnostics.Tracing.Session;

namespace jittingstarted_etw_test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TraceEventSession session = new TraceEventSession("MySession"))
            {
                DynamicTraceEventParser dynamicParser = new DynamicTraceEventParser(session.Source);
                dynamicParser.AddCallbackForProviderEvent(ClrTraceEventParser.ProviderName, "MethodJittingStarted_V1", (data) =>
                {
                    Console.WriteLine("Dynamic");
                });
                dynamicParser.All += delegate (TraceEvent data)
                {
                    if (data.EventName.StartsWith("MethodJitting"))
                    {
                        Console.WriteLine("Found");
                    }
                };

                session.Source.Clr.MethodJittingStarted += delegate (MethodJittingStartedTraceData data)
                {
                    Console.WriteLine("Static");
                };
                session.EnableProvider(ClrTraceEventParser.ProviderGuid.ToString(), TraceEventLevel.Verbose, (ulong)ClrTraceEventParser.Keywords.Jit);
                session.Source.Process();
            }
        }
    }
}
