using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;

namespace etw_cpu_watcher
{
    class Program
    {
        private const float CpuSampleIntervalMsec = 10;
        private static CPUWatcher Watcher = new CPUWatcher(CpuSampleIntervalMsec);
        private static Stopwatch Stopwatch = new Stopwatch();
        private static TraceEventSession Session;

        public static void Main(string[] args)
        {
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                Task processingTask = Task.Factory.StartNew(StartProcessing, cts.Token, cts.Token);
                Task printingTask = Task.Factory.StartNew(PrintUtilization, cts.Token, cts.Token);

                // Press any key to stop collection.
                Console.ReadKey();
                cts.Cancel();
                Session.Source.StopProcessing();
                Task.WaitAll(processingTask, printingTask);
            }
        }

        private static void StartProcessing(object o)
        {
            using (Session = new TraceEventSession("CPUWatcher"))
            {
                // Use a stopwatch to keep track of time between printing so that we can calculate percentage utilization.
                Stopwatch.Start();

                // Enable CPU sampling and set the sampling interval.
                Session.CpuSampleIntervalMSec = CpuSampleIntervalMsec;
                Session.EnableKernelProvider(
                    KernelTraceEventParser.Keywords.Process |
                    KernelTraceEventParser.Keywords.Profile);

                // Hook the CPU sample event and start processing incoming events.
                ETWTraceEventSource source = Session.Source;
                source.Kernel.PerfInfoSample += Watcher.OnCPUSample;
                Session.Source.Process();
            }
        }

        private static void PrintUtilization(object o)
        {
            CancellationToken token = (CancellationToken) o;
            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                double elapsedMilliseconds = Stopwatch.ElapsedMilliseconds;

                // Print the CPU utilization for each processor.
                for (int i = 0; i < Watcher.CPUs.Length; i++)
                {
                    double cpuTime = Watcher.CPUs[i].GetTotalTimeAndReset();
                    double percentage = cpuTime / elapsedMilliseconds * 100;
                    percentage = percentage > 100 ? 100 : percentage;
                    Console.WriteLine($"[{i}] {percentage:N2}");
                }

                Console.WriteLine();
                Stopwatch.Restart();
            }
        }
    }
}
