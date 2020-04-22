using System;
using System.Collections.Generic;

namespace server_gc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            List<object[]> list = new List<object[]>();
            int segmentSize = 10000;
            int iter = 0;
            while(true)
            {
                if(++iter % 10000 == 0)
                {
                    Console.WriteLine("Cleared");
                    list.Clear();
                }
                object[] arr = new object[segmentSize];
                for(int i=0; i<segmentSize; i++)
                {
                    arr[i] = new object();                    
                }
                list.Add(arr);
                GC.Collect(2, GCCollectionMode.Forced);
            }

        }
    }


}
