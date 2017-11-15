using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace sleeptastic
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                SleepSome();
                Thread.Sleep(10);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void SleepSome()
        {
            Thread.Sleep(100);
        }
    }
}
