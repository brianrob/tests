// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Tracing;
using System.Threading;

namespace Diagnostics.Tracing.Examples
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            using (SimpleEventListener eventListener = new SimpleEventListener())
            {
                // Trigger the allocator task.
                System.Threading.Tasks.Task.Run(new Action(Allocator));

                // Wait for events.
                Thread.Sleep(1000);

                // Generate some GC events.
                GC.Collect(2, GCCollectionMode.Forced);
            }
        }

        private static void Allocator()
        {
            while (true)
            {
                for(int i=0; i<1000; i++)
                {
                    GC.KeepAlive(new object());
                }

                Thread.Sleep(10);
            }
        }
    }

    internal sealed class SimpleEventListener : EventListener
    {
        // Called whenever an EventSource is created.
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            // Watch for the .NET runtime EventSource and enable all of its events.
            if (eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
            {
                    EnableEvents(eventSource, EventLevel.Verbose, (EventKeywords)(-1));
            }
        }

        // Called whenever an event is written.
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            // Write the contents of the event to the console.
            Console.WriteLine($"ThreadID = {eventData.OSThreadId} ID = {eventData.EventId} Name = {eventData.EventName}");
            for (int i = 0; i < eventData.Payload.Count; i++)
            {
                string payloadString = eventData.Payload[i] != null ? eventData.Payload[i].ToString() : string.Empty;
                Console.WriteLine($"\tName = \"{eventData.PayloadNames[i]}\" Value = \"{payloadString}\"");
            }
            Console.WriteLine("\n");
        }
    }
}
