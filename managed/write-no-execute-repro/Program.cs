using System;
using System.Runtime.CompilerServices;

namespace Test
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            while(true)
            {
                Method1();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Method1()
        {
            Method2();
        }
        
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Method2()
        {
            Method3();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Method3()
        {
            Method4();
        }
        
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Method4()
        {
            GC.KeepAlive(new object());
        }
    }
}