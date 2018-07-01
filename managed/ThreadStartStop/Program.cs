using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadStartStop
{
    class Program
    {
        private static bool s_Continue = true;
        private static Thread s_WorkerThread = null;

        static void Main(string[] args)
        {
            while(true)
            {
                StartThread();
                Thread.Sleep(32);
                StopThread();
                Thread.Sleep(32);
                GC.Collect(0);
            }
        }

        private static void StartThread()
        {
            Console.WriteLine("Starting Thread.");
            s_Continue = true;
            s_WorkerThread = new Thread(new ThreadStart(ThreadProc));
            s_WorkerThread.Start();
        }

        private static void StopThread()
        {
            Console.WriteLine("Stopping Thread.");
            s_Continue = false;
            s_WorkerThread.Join();
            s_WorkerThread = null;
            Console.WriteLine("Thread Stopped.");
        }
        private static void ThreadProc()
        {
            int i = 0;
            while (s_Continue)
            {
                Console.WriteLine($"\t[index] = {i++}");
                Thread.Sleep(16);
            }
        }
    }
}
