using System;
using System.Runtime.InteropServices;

namespace Spinner
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            int spinSeconds = 5;
            if(args.Length > 0)
            {
                spinSeconds = Convert.ToInt32(args[0]);
            }

            Console.WriteLine($"Start spin for {spinSeconds} seconds.");
            object obj = null;
            DateTime stopTime = DateTime.Now.Add(TimeSpan.FromSeconds(spinSeconds));
            while(DateTime.Now < stopTime)
            {
                obj = new object();
                GC.KeepAlive(obj);
            }
            Console.WriteLine("Spin complete.");
        }
    }
}
