using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace system_diagnostics_process
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Factory.StartNew(Spin, TaskCreationOptions.LongRunning);
            while (true)
            {
                Process currentProcess = Process.GetCurrentProcess();
                Console.WriteLine($"Processor Time (ticks): {currentProcess.TotalProcessorTime.Ticks}");
                Console.WriteLine($"Private Memory: {currentProcess.PrivateMemorySize64:N}");
                Thread.Sleep(2000);
            }
        }

        private static void Spin()
        {
            while (true)
            {
                Thread.SpinWait(1000);
                Thread.Sleep(10);
            }
        }
    }
}
