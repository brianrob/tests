using System;
using System.Diagnostics;

namespace TraceLoggingEventHandleMap
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int numDeclaredEvents = 0; numDeclaredEvents <= 10; numDeclaredEvents++)
            {
                for (int numTraceLoggingEvents = 0; numTraceLoggingEvents <= 10000; numTraceLoggingEvents += 2000)
                {
                    Console.WriteLine($"Testing: DeclaredEvents = {numDeclaredEvents}, TraceLoggingEvents = {numTraceLoggingEvents}");
                    Test(numDeclaredEvents, numTraceLoggingEvents);
                }
            }
        }

        private static void Test(int numDeclaredEvents, int numTraceLoggingEvents)
        {
            TraceLoggingEventHandleTable handleTable = new TraceLoggingEventHandleTable(numDeclaredEvents);

            for (int i = numDeclaredEvents; i < numDeclaredEvents + numTraceLoggingEvents; i++)
            {
                handleTable.SetEventHandle(i, (IntPtr)i);
                Validate(handleTable, numDeclaredEvents + 1, i + 1, numDeclaredEvents);
            }
        }

        private static void Validate(TraceLoggingEventHandleTable handleTable, int startEventID, int endEventID, int numDeclaredEvents)
        {
            for (int i = startEventID; i < endEventID; i++)
            {
                Debug.Assert((IntPtr)i == handleTable[i]);
            }

            for (int i = endEventID - numDeclaredEvents; i < handleTable.Length; i++)
            {
                Debug.Assert(IntPtr.Zero == handleTable[i+numDeclaredEvents]);
            }
        }
    }
}
