using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelFor
{
    class Program
    {
        private static int ConcurrentWorkItems = 0;

        static void Main(string[] args)
        {
            InitializeDefaultParallelOptions();
            Execute();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeDefaultParallelOptions()
        {
            ParallelOptions options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 4
            };

            // Get the System.Threading.Tasks.Parallel type.
            Type parallelType = typeof(Parallel);
            FieldInfo field = parallelType.GetField("s_defaultParallelOptions", BindingFlags.NonPublic | BindingFlags.Static);
            if(field == null)
            {
                throw new Exception("Unable to get field.");
            }

            field.SetValue(null, options);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Execute()
        {
            Console.WriteLine($"Processors: {Environment.ProcessorCount}");
            int length = 16;
            int[] list = new int[length];

            Parallel.For(
                0,
                list.Length,
                //new ParallelOptions() { MaxDegreeOfParallelism = 4 },
                index =>
                {
                    int currentCount = Interlocked.Increment(ref ConcurrentWorkItems);
                    Console.WriteLine($"Index: {index} Thread: {Thread.CurrentThread.ManagedThreadId} CurrentCount: {currentCount}");
                    Thread.Sleep(1000);
                    Interlocked.Decrement(ref ConcurrentWorkItems);
                });
        }
    }
}
