using System;
using System.Diagnostics.Tracing;

namespace client
{
    [EventSource(Name = "ClientEventSource")]
    public class ClientEventSource : EventSource
    {
        public static ClientEventSource Log = new ClientEventSource();

        [Event(1)]
        public void RequestStart()
        {
            WriteEvent(1);
        }

        [Event(2)]
        public void RequestStop()
        {
            WriteEvent(2);
        }
    }
}
