using System;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;

namespace eventcounter_listener
{
    class Program
    {
        private static readonly string[] Providers = new string[]
        {
            "System.Runtime"
        };

        public static void Main(string[] args)
        {
            // Create a new session.
            using TraceEventSession session = new TraceEventSession("EventCounter-Listener-Session");

            // Register for CTRL+C.
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs cancelArgs)
            {
                session.Dispose();
                Environment.Exit(0);
            };


            TraceEventProviderOptions options = new TraceEventProviderOptions();
            options.AddArgument("EventCounterIntervalSec", "1");

            foreach (string provider in Providers)
            {
                Guid providerGuid = TraceEventProviders.GetEventSourceGuidFromName(provider);

                session.EnableProvider(
                    providerGuid,
                    providerLevel: (TraceEventLevel) 1,
                    matchAnyKeywords: 0,
                    options: options);
            }


            ETWTraceEventSource eventSource = session.Source;

            // Register for events from each provider.
            foreach (string provider in Providers)
            {
                eventSource.Dynamic.AddCallbackForProviderEvent(
                    provider,
                    "EventCounters",
                    (data) =>
                    {
                        Console.WriteLine(data.ToString());
                    });
            }

            // Start processing events.
            eventSource.Process();
        }
    }
}
