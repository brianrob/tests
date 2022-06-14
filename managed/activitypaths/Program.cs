using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics.Tracing;

namespace activitypaths
{
class MySource : EventSource
    {
        [Event(1, Message = "{0}")]
        public void Message(string arg) { WriteEvent(1, arg); }

        [Event(2, Message = "{0}", ActivityOptions = EventActivityOptions.Recursive)]
        public void MyActivityStart(string arg) { WriteEvent(2, arg); }

        [Event(3, Message = "{0}", ActivityOptions = EventActivityOptions.Recursive)]
        public void MyActivityStop(string arg) { WriteEvent(3, arg); }

        public static MySource Logger = new MySource();
    }

    class MyListener : EventListener
    {
        public const EventKeywords TasksFlowActivityIds = (EventKeywords)0x80;
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (eventSource.Name == "MySource")
            {
                EnableEvents(eventSource, EventLevel.Verbose);
                Console.WriteLine("*** In MyListener: turning on MySource");
            }
            else if (eventSource.Name == "System.Threading.Tasks.TplEventSource")
            {
                EnableEvents(eventSource, EventLevel.Verbose, TasksFlowActivityIds);
                Console.WriteLine("*** In MyListener: turning on Task library Activity Flow");
            }
        }
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            // We expect the activity and relatedActities to be set properly, currently they are zero.  
            Console.WriteLine("MyListener: level {0}, message: {1} activity {2}  related {3}",
                eventData.Level, 
                eventData.Message != null ? string.Format(eventData.Message, eventData.Payload.ToArray()) : "", 
                eventData.ActivityId, eventData.RelatedActivityId);
        }
    }

    public class Program
    {
        private const int NumActivities = 20;

        public static void Main(string[] args)
        {
            Console.WriteLine("Paused");
            Console.ReadKey();
            MyListener listener = new MyListener();

            // Currently the activityIDs generated in the OnEventWritten EventListener callback are always 0.
            // They should be non-zero during the activity.   
            MySource.Logger.Message("Before Start");

            for (int i = 0; i < NumActivities; i++)
            {
                MySource.Logger.MyActivityStart($"Activity{i + 1}");
                MySource.Logger.MyActivityStop($"Activity{i + 1}");
            }
            MySource.Logger.Message("After task");
            Console.ReadKey();
        }
    }
}
