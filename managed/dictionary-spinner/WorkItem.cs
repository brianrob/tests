using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dictionary_spinner
{
    internal class WorkItem
    {
        private const int NumIterations = 100;

        private const int T1_SleepTimeInMs = 0;
        private const int T2_SleepTimeInMs = 0;
        private const int T3_SleepTimeInMs = 0;

        private Dictionary<int, int> _dict = new Dictionary<int, int>();

        private Task _t1;
        private Task _t2;
        private Task _t3;

        private ManualResetEvent _t1Ready = new ManualResetEvent(false);
        private ManualResetEvent _t2Ready = new ManualResetEvent(false);
        private ManualResetEvent _t3Ready = new ManualResetEvent(false);

        public void Execute()
        {
            _t1 = Task.Factory.StartNew(T1);
            _t2 = Task.Factory.StartNew(T2);
            _t3 = Task.Factory.StartNew(T3);
            Task.WaitAll(_t1, _t2, _t3);
        }

        private void T1()
        {
            _t1Ready.Set();
            _t2Ready.WaitOne();
            _t3Ready.WaitOne();

            for (int i = 0; i < NumIterations; i++)
            {
                try
                {
                    _dict.Add(i, i);
                    _dict.Remove(i - 10);
                }
                catch
                {
                    // Swallow all exceptions.
                }

                if (T1_SleepTimeInMs > 0)
                {
                    Thread.Sleep(T1_SleepTimeInMs);
                }
            }
        }

        private void T2()
        {
            _t2Ready.Set();
            _t1Ready.WaitOne();
            _t3Ready.WaitOne();

            for (int i = -1; i > -NumIterations; i--)
            {
                try
                {
                    _dict.TryGetValue(i, out int value);
                }
                catch
                {
                    // Swallow all exceptions.
                }

                try
                {
                    _dict.ContainsKey(i);
                }
                catch
                {
                    // Swallow all exceptions.
                }

                if (T2_SleepTimeInMs > 0)
                {
                    Thread.Sleep(T2_SleepTimeInMs);
                }
            }
        }

        private void T3()
        {
            int iterationCount = NumIterations * 3;

            _t3Ready.Set();
            _t1Ready.WaitOne();
            _t2Ready.WaitOne();

            for (int i = NumIterations * 2; i < iterationCount; i++)
            {
                try
                {
                    _dict.Add(i, i);
                    _dict.Remove(i - 10);
                }
                catch
                {
                    // Swallow all exceptions.
                }

                if (T3_SleepTimeInMs > 0)
                {
                    Thread.Sleep(T3_SleepTimeInMs);
                }
            }
        }
    }
}
