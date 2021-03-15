using System;
using System.Diagnostics;

namespace process_start_time
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the current process.
            Process currentProcess = Process.GetCurrentProcess();
            TimeSpan processTime = DateTime.Now - currentProcess.StartTime;
            Console.WriteLine($"Execution Time: {processTime}");
            Console.ReadKey();
        }
    }
}
