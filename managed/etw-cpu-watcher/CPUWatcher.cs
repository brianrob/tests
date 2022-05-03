using System;
using System.Diagnostics;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;

namespace etw_cpu_watcher
{
    public sealed class CPUWatcher
    {
        // It's unlikely that you're modifying the CPU configuration online, so only check once.
        private static readonly int NumProcs = Environment.ProcessorCount;

        public CPUWatcher(double expectedTimeBetweenSamples)
        {
            for (int i = 0; i < CPUs.Length; i++)
            {
                CPUs[i] = new CPUState(expectedTimeBetweenSamples);
            }
        }

        public void OnCPUSample(SampledProfileTraceData data)
        {
            // This operates on a per-CPU basis, but you can also use data.ProcessID to operate on a per-process basis.
            // If you choose to do this, you should hook Source.Kernel.ProcessStart, Source.Kernel.ProcessDCStart, and Source.Kernel.ProcessStop to keep track of which processes are live.
            Debug.Assert(data.ProcessorNumber < NumProcs);
            CPUs[data.ProcessorNumber].OnCPUSample(data);
        }

        public CPUState[] CPUs { get; } = new CPUState[NumProcs];
    }

    public sealed class CPUState
    {
        private double _totalTime;
        private double _expectedTimeBetweenSamples;
        private double _lastTimeStampRelMsec;
        public CPUState(double expectedTimeBetweenSamples)
        {
            _expectedTimeBetweenSamples = expectedTimeBetweenSamples;
        }

        public void OnCPUSample(SampledProfileTraceData data)
        {
            // This can be done in a lock-free manner, but for the purposes of this sample, I'm using a lock for simplicity.
            lock (this)
            {
                // It's possible that the rate of events is faster than we expect, so make sure that we don't add more than the amount of time between samples.
                _totalTime += Math.Min(_expectedTimeBetweenSamples, data.TimeStampRelativeMSec - _lastTimeStampRelMsec);
                _lastTimeStampRelMsec = data.TimeStampRelativeMSec;
            }
        }

        public double GetTotalTimeAndReset()
        {
            // This can be done in a lock-free manner, but for the purposes of this sample, I'm using a lock for simplicity.
            double ret;
            lock (this)
            {
                ret = _totalTime;
                _totalTime = 0;
            }

            return ret;
        }
    }
}
