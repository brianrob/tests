using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        using (var listener = new MyListener())
        {

        Guid id = Guid.NewGuid();
//        EventSource.SetCurrentThreadActivityId(id);
        Console.WriteLine("Current ID: " + EventSource.CurrentThreadActivityId);

        MySource.Log.Start();
        MySource.Log.Foo(42);
        Task.Run(() => Console.WriteLine("Running: " + EventSource.CurrentThreadActivityId)).Wait();
        MySource.Log.Stop();
        }
    }
}

[EventSource(Name = "TestSource")]
class MySource : EventSource
{
    public static readonly MySource Log = new MySource();

    [Event(1)]
    public void Foo(int value) => WriteEvent(1, value);

    [Event(2)]
    public void Start() => WriteEvent(2);

    [Event(3)]
    public void Stop() => WriteEvent(3);
}

class MyListener : EventListener
{
    protected override void OnEventSourceCreated(EventSource eventSource) =>
        EnableEvents(eventSource, EventLevel.LogAlways, EventKeywords.All);

    protected override void OnEventWritten(EventWrittenEventArgs eventData) =>
        Console.WriteLine("[" + System.Threading.Thread.CurrentThread.ManagedThreadId + "]" + eventData.EventSource.Name + ": " + eventData.EventName + ": " + eventData.ActivityId);
}
