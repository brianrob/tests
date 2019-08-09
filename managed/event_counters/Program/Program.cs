using System;
using System.Threading;
using CounterLib;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                MinimalEventCounterSource.Log.Request("http://foo", (float)2.2);
                Thread.Sleep(100);
            }
        }
    }
}
