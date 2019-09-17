using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace AppZapper
{
    public sealed class ExperimentGenerator
    {
        private ExperimentQueue _experimentQueue = new ExperimentQueue();
        private Experiment _bestExperiment;
        private long _appBinarySize;
        private Random _random = new Random();

        public ExperimentGenerator()
        {
            FileInfo fileInfo = new FileInfo(Config.AppPath);
            _appBinarySize = fileInfo.Length;

            // Prime the queue with the first experiment.
            _experimentQueue.Enqueue(new Experiment(
                new ZeroBlockList(),
                new ZeroBlockList()));
        }

        public void Execute()
        {
            // Get the next experiment.
            Experiment experiment;
            while (_experimentQueue.TryDequeue(out experiment))
            {
                try
                {
                    experiment.Execute();
                }
                catch
                {
                    // Consider the experiment failed and bail.
                    // (Succeeded won't be set until the very end.)
                }

                OnExperimentComplete(experiment);
            }

            // Check to see if we need to prime the queue again.
            if(_bestExperiment != null)
            {
                // Attempt to add more mutations of this experiment.
                Experiment[] experiments = GeneratePermutations(_bestExperiment, Config.NumPermutations);
                foreach (Experiment exp in experiments)
                {
                    _experimentQueue.Enqueue(exp);
                }
            }
        }

        private void OnExperimentComplete(Experiment experiment)
        {
            // Mark the experiment as completed.
            experiment.Complete();

            if (experiment.Succeeded)
            {
                Log(experiment, $"Experiment succeeded.");
                // Decide if this is the best experiment we've seen so far.
                if ((_bestExperiment == null) ||
                   (_bestExperiment.CommittedList.Count < experiment.CommittedList.Count))
                {
                    Log(experiment, "New best experiment found.");
                    _bestExperiment = experiment;

                    // If this is a new best experiment, then write the list of blocks out.
                    _bestExperiment.CommittedList.WriteToFile(Config.LatestSuccessfulList);
                }

                // Attempt to add more mutations of this experiment.
                Experiment[] experiments = GeneratePermutations(experiment, Config.NumPermutations);
                foreach(Experiment exp in experiments)
                {
                    _experimentQueue.Enqueue(exp);
                }
            }
        }

        private Experiment[] GeneratePermutations(Experiment baseExperiment, int numExperiments)
        {
            Log(baseExperiment, $"Generating {numExperiments} experiments based on this successful experiment.");
            Experiment[] experiments = new Experiment[numExperiments];
            for (int i=0; i<numExperiments; i++)
            {
                // Generate the next block list.
                ZeroBlockList attemptingList = new ZeroBlockList();
                while(true)
                {
                    ulong value = GenerateRandomBlock();
                    if(!baseExperiment.CommittedList.Contains(value))
                    {
                        attemptingList.Add(value);
                        break;
                    }
                }

                // Create a new experiment.
                experiments[i] = new Experiment(baseExperiment.CommittedList, attemptingList);
            }

            return experiments;
        }

        private ulong GenerateRandomBlock()
        {
            ulong randVal = (ulong)_random.Next(0, (int)_appBinarySize);
            randVal = (randVal + (Config.BlockSize - 1)) & ~(Config.BlockSize - 1);
            return randVal;
        }

        private static void Log(Experiment experiment, string message)
        {
            Console.WriteLine($"[{experiment.Number}] {message}");
        }
    }
}
