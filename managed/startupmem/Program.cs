using System;
using System.Diagnostics;

namespace startupmem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(Process.GetCurrentProcess().PrivateMemorySize64);
            Console.ReadLine();
        }
    }
}
