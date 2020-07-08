using System;
using System.Threading;

namespace minmax_threads
{
    class Program
    {
        static void Main(string[] args)
        {
            int workerThreads;
            int ioThreads;
            ThreadPool.GetMinThreads(out workerThreads, out ioThreads);
            Console.WriteLine($"GetMinThreads() WorkerThreads({workerThreads}) IOThreads({ioThreads})");
            ThreadPool.GetMaxThreads(out workerThreads, out ioThreads);
            Console.WriteLine($"GetMaxThreads() WorkerThreads({workerThreads}) IOThreads({ioThreads})");
        }
    }
}
