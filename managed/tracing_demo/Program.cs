using System;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace tracing_demo
{
    class Program
    {
        private const int NumTasks = 2;

        static void Main(string[] args)
        {
            // Log events to the console.
            ConsoleEventListener listener = new ConsoleEventListener();

            // Start the spinner tasks.
            Task[] spinTasks = new Task[NumTasks];
            for(int i=0; i<NumTasks; i++)
            {
                spinTasks[i] = Task.Factory.StartNew(SpinTask);
            }

            // Wait for all of them to complete.
            Task.WaitAll(spinTasks);
        }

        private static void SpinTask()
        {
            SpinEventSource.Log.SpinTaskStart();

            SpinWorker(10);

            SpinEventSource.Log.SpinTaskStop();
        }

        private static void SpinWorker(int count)
        {
            SpinEventSource.Log.SpinWorker(count);

            if (count > 0)
            {
                SpinForASecond();
                SpinWorker(count - 1);
            }

            if (count % 2 == 0)
            {
                GC.Collect();
            }
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SpinForASecond()
        {
            SpinEventSource.Log.SpinForASecond();

            int i = 0;
            DateTime start = DateTime.Now;
            do
            {
                for ( ; i < 100; i++) ;
            }
            while (DateTime.Now.Subtract(start).TotalSeconds < 1.0);

            // Don't let the JIT optimize away the for loop.
            GC.KeepAlive(i);
        }
    }

    [EventSource]
    internal sealed class SpinEventSource : EventSource
    {
        public static SpinEventSource Log = new SpinEventSource();

        [Event(1)]
        public void SpinTaskStart()
        {
            WriteEvent(1);
        }

        [Event(2)]
        public void SpinTaskStop()
        {
            WriteEvent(2);
        }

        [Event(3)]
        public void SpinWorker(int count)
        {
            WriteEvent(3, count);
        }

        [Event(4)]
        public void SpinForASecond()
        {
            WriteEvent(4);
        }
    }

    internal sealed class ConsoleEventListener : EventListener
    {
        public ConsoleEventListener()
        {
            EnableEvents(SpinEventSource.Log, EventLevel.Verbose);
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            lock (this)
            {
                Console.Write($"[{eventData.EventName}][TID={Thread.CurrentThread.ManagedThreadId}]");
                for (int i = 0; i < eventData.PayloadNames.Count; i++)
                {
                    Console.Write($" {eventData.PayloadNames[i]}={eventData.Payload[i]}");
                }
                Console.WriteLine();
            }
        }
    }
}
