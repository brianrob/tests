using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace performance
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TestBenchmark>();
        }
    }
}
