using System;
using System.Threading;

namespace allocator_smallstack
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating allocator thread.");
            Thread t = new Thread(new ThreadStart(Allocate), 64 * 1024); // 64K stack
            t.Start();
            Console.ReadKey();

        }

        static void Allocate()
        {
            while(true)
            {
                GC.KeepAlive(new object());
            }
        }
    }
}
