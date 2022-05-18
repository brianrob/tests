using System;
using System.Diagnostics;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Etlx;
using Microsoft.Diagnostics.Tracing.EventPipe;

namespace eventpipe_stacks
{
    class Program
    {
        private const string TracePath = @"sample.nettrace";

        public static void Main(string[] args)
        {
            // This sample will not emit stacks.
            // Stacks are only available through TraceLog because stacks are stored after the events they belong to.
            using (EventPipeEventSource source = new EventPipeEventSource(TracePath))
            {
                SampleProfilerTraceEventParser parser = new SampleProfilerTraceEventParser(source);
                parser.ThreadSample += delegate (ClrThreadSampleTraceData data)
                {
                    Debug.Assert(data.CallStackIndex() == CallStackIndex.Invalid);
                };
                parser.ThreadStackWalk += delegate (ClrThreadStackWalkTraceData data)
                {
                    Debug.Assert(false);
                };
                source.Process();
            }

            // This will emit stacks as the first pass occurs during creation of the etlx file.
            string traceLogPath = TraceLog.CreateFromEventPipeDataFile(TracePath);
            using (TraceLog traceLog = new TraceLog(traceLogPath))
            {
                TraceLogEventSource source = traceLog.Events.GetSource();
                source.AllEvents += delegate (TraceEvent data)
                {
                    TraceCallStack stack = data.CallStack();
                    if (stack != null)
                    {
                        Console.WriteLine($"{data.ProviderName}/{data.EventName}");
                        Debug.Assert(data.CallStackIndex() != CallStackIndex.Invalid);
                        PrintCallStack(stack);
                        Console.WriteLine();
                    }
                };
                source.Process();
            }
        }

        private static void PrintCallStack(TraceCallStack callStack)
        {
            TraceCallStack current = callStack;
            while (current != null)
            {
                Console.WriteLine($"[0x{current.CodeAddress.Address:X}] {current.CodeAddress.ModuleName}!{current.CodeAddress.FullMethodName}");
                current = current.Caller;
            }
        }
    }
}
