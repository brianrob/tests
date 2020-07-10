using System;
using System.Diagnostics.Tracing;

namespace eventsource_requeststartstop
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                RequestHandler("TestRequest");
            }
        }

        /// <summary>
        /// This is an example of a request handler that calls the RequestEventSource.
        /// The RequestEventSource can be used to measure time spent handling the request, as well as
        /// to provide markers inside of a trace so that it's possible to see what code executed in the context
        /// of the critical path of the request.
        /// </summary>
        /// <param name="requestAction"></param>
        private static void RequestHandler(string requestAction)
        {
            RequestEventSource.Log.RequestStart(requestAction);

            // Do some stuff here to handle the request.

            RequestEventSource.Log.RequestStop();
        }
    }

    [EventSource(Name = "Example-Request-EventSource")]
    public sealed class RequestEventSource : EventSource
    {
        public static RequestEventSource Log = new RequestEventSource();

        /// <summary>
        /// Fires an event at the start of a request.
        /// </summary>
        /// <remarks>
        /// No arguments are required, but if it is helpful to be able to identify the request,
        /// one or more arguments can be specified.
        /// It's important that the name of the method end with "Start".  This fires some additional machinery
        /// inside of EventSource that is used for causality tracing.
        /// </remarks>
        [Event(1)]
        public void RequestStart(string exampleArgument)
        {
            WriteEvent(1, exampleArgument);
        }

        /// <summary>
        /// Fires an event at the end of a request.
        /// </summary>
        /// <remarks>
        /// No arguments are required, but if it is helpful to be able to identify the request,
        /// one or more arguments can be specified.
        /// It's important that the name of the method end with "Stop".  This fires some additional machinery
        /// inside of EventSource that is used for causality tracing.
        /// </remarks>
        [Event(2)]
        public void RequestStop()
        {
            WriteEvent(2);
        }
    }
}
