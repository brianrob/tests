using System;
using System.Runtime.InteropServices;

namespace TimeToMain
{
    public static class Program
    {
        [DllImport("liblttng-ust.so")]
        private static extern void _lttng_ust_tracef(string value);

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            _lttng_ust_tracef("Hello World!");
        }
    }
}
