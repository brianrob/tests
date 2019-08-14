using System;

namespace app
{
    class Program
    {
        private static object Obj = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            while(true)
            {
                GC.KeepAlive(Obj);
            }
        }
    }
}
