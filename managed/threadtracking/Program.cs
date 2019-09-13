using System;
using System.Threading;

namespace threadtracking
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(ThreadProc);
            t.Start();
        }

        private static void ThreadProc(object state)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
