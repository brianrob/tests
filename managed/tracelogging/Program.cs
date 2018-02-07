﻿using System;
using System.Diagnostics.Tracing;

namespace tracelogging
{
    public sealed class MySource : EventSource
    {
        MySource()
            : base(EventSourceSettings.EtwSelfDescribingEventFormat)
        {
        }

        public static MySource Logger = new MySource();
    }

    class Program
    {
        static void Main(string[] args)
        {
            MySource.Logger.Write("TestEvent", new { field1 = "Hello", field2 = 3, field3 = 6 });
            MySource.Logger.Write("TestEvent1", new { field1 = "Hello", field2 = 3, });
            MySource.Logger.Write("TestEvent2", new { field1 = "Hello" });
            MySource.Logger.Write("TestEvent3", new { field1 = new byte[10] });
        }
    }
}
