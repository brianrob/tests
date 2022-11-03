using System;

namespace dictionary_spinner
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            long count = 0;
            while (true)
            {
                WorkItem workItem = new WorkItem();
                workItem.Execute();
                Console.WriteLine($"Completed {count++} iterations.");
            }
        }
    }
}