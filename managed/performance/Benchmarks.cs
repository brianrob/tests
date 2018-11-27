using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace performance
{
    public class TestBenchmark
    {
        [Benchmark]
        public void RunTest()
        {
            GC.Collect(2);
        }
    }
}
