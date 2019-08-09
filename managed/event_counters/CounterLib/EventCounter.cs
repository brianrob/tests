using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Tracing;

namespace CounterLib
{
    // Give your event sources a descriptive name using the EventSourceAttribute, otherwise the name of the class is used. 
    [EventSource(Name = "Samples-EventCounterDemos-Minimal")]
    public sealed class MinimalEventCounterSource : EventSource
    {
        // define the singleton instance of the event source
        public static MinimalEventCounterSource Log = new MinimalEventCounterSource();
        private EventCounter requestCounter;

        private MinimalEventCounterSource() : base(EventSourceSettings.EtwSelfDescribingEventFormat)
        {
            this.requestCounter = new EventCounter("request", this);
        }

        /// <summary>
        /// Call this method to indicate that a request for a URL was made which took a particular amount of time
        public void Request(string url, float elapsedMSec)
        {
            // Notes:
            //   1. the event ID passed to WriteEvent (1) corresponds to the (implied) event ID
            //      assigned to this method. The event ID could have been explicitly declared
            //      using an EventAttribute for this method
            //   2. Each counter supports a single float value, so conceptually it maps to a single
            //      measurement in the code.
            //   3. You don't have to have log with WriteEvent if you don't think you will ever care about details
            //       of individual requests (that counter data is sufficient).  
            WriteEvent(1, url, elapsedMSec);    // This logs it to the event stream if events are on.    
            this.requestCounter.WriteMetric(elapsedMSec);        // This adds it to the PerfCounter called 'Request' if PerfCounters are on
        }
    }
}
