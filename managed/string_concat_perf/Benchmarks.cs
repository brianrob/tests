using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace performance
{
    public class TestBenchmark
    {
        private const string S0 = "Hello World!";
        private const string S1 = "Hello World Again!";

        [Benchmark]
        public void StringConcat()
        {
            string result = string.Concat(S0, S1);
            GC.KeepAlive(result);
        }

        [Benchmark]
        public void StringBuilder()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(S0);
            builder.Append(S1);
            GC.KeepAlive(builder.ToString());
        }

        [Benchmark]
        public void ByHand()
        {
            char[] result = new char[S0.Length + S1.Length];
            int nextChar = 0;
            for(int i=0; i<S0.Length; i++)
            {
                result[nextChar++] = S0[i];
            }
            for(int i=0; i<S1.Length; i++)
            {
                result[nextChar++] = S1[i];
            }
            GC.KeepAlive(new String(result));
        }

        [Benchmark]
        public void ByHandUnsafe()
        {
            char[] result = new char[S0.Length + S1.Length];
            unsafe
            {
                fixed(char *unsafe_S0 = S0, unsafe_S1 = S1)
                {
                    char *cS0 = unsafe_S0;
                    char *cS1 = unsafe_S1;
                    int nextChar = 0;
                    for(int i=0; i<S0.Length; i++)
                    {
                        result[nextChar++] = *cS0++;
                    }
                    for(int i=0; i<S1.Length; i++)
                    {
                        result[nextChar++] = *cS1++;
                    }
                }
            }
            GC.KeepAlive(new String(result));
        }
    }
}
