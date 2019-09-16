using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AppZapper
{
    public sealed class ExperimentQueue
    {
        private sealed class KeyComparer : IComparer<int>
        {
            public int Compare([AllowNull] int x, [AllowNull] int y)
            {
                return y - x;
            }
        }

        private SortedList<int, List<Experiment>> _queue = new SortedList<int, List<Experiment>>(new KeyComparer());

        public void Enqueue(Experiment experiment)
        {
            lock (this)
            {
                List<Experiment> list;
                if (!_queue.TryGetValue(experiment.CommittedList.Count, out list))
                {
                    list = new List<Experiment>();
                    _queue.Add(experiment.CommittedList.Count, list);
                }

                list.Add(experiment);
            }
        }

        public bool TryDequeue(out Experiment experiment)
        {
            lock (this)
            {
                if (_queue.Count <= 0)
                {
                    experiment = null;
                    return false;
                }

                List<Experiment> list = _queue.Values[0];
                experiment = list[0];
                list.RemoveAt(0);
                if (list.Count == 0)
                {
                    _queue.RemoveAt(0);
                }
            }

            return true;
        }
    }
}
