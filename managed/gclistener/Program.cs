using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;
using Microsoft.Diagnostics.Tracing.Session;

namespace gclistener
{
    class Program
    {
        private static TraceEventSession Session;
        private static Stack<GCStartTraceData> StartedGCs = new Stack<GCStartTraceData>();

        static void Main(string[] args)
        {
            Task t = Task.Factory.StartNew(() => Run(), TaskCreationOptions.LongRunning);
            Console.ReadKey();

            if(Session != null)
            {
                Session.Dispose();
            }
        }

        private static void Run()
        {
            Session = new TraceEventSession("GCListener");

            TraceEventProviderOptions options = new TraceEventProviderOptions();
            options.ProcessNameFilter = new List<string>();
            options.ProcessNameFilter.Add("devenv.exe");
            Session.EnableProvider(ClrTraceEventParser.ProviderName, TraceEventLevel.Verbose, (ulong)ClrTraceEventParser.Keywords.GC, options);

            ETWTraceEventSource source = Session.Source;
            source.Clr.GCStart += delegate (GCStartTraceData data)
            {
                StartedGCs.Push((GCStartTraceData)data.Clone());
            };

            source.Clr.GCStop += delegate (GCEndTraceData data)
            {
                GCStartTraceData startData = StartedGCs.Peek();
                if(startData == null || startData.ProcessID != data.ProcessID || startData.Count != data.Count)
                {
                    return;
                }

                StartedGCs.Pop();
                LogGC(startData, data);
            };

            source.Process();
        }

        private static void LogGC(GCStartTraceData startData, GCEndTraceData endData)
        {
            string logLine = $"[ProcessID:{startData.ProcessID}] [Index:{startData.Count}] [Reason:{startData.Reason}] [Depth:{startData.Depth}] [TimeStamp:{startData.TimeStampRelativeMSec.ToString("F2")}] [Latency:{(endData.TimeStampRelativeMSec - startData.TimeStampRelativeMSec).ToString("F2")}]";
            Console.WriteLine(logLine);
        }
    }
}
