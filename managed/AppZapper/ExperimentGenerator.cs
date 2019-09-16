using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppZapper
{
    public sealed class ExperimentGenerator
    {
        private ExperimentQueue _experimentQueue = new ExperimentQueue();
        private Experiment _bestExperiment;

        public ExperimentGenerator()
        {
            // Prime the queue with the first experiment.
            _experimentQueue.Enqueue(new Experiment(
                new ZeroBlockList(),
                new ZeroBlockList()));
        }

        public void Execute()
        {
            // Get the next experiment.
            Experiment experiment;
            while(_experimentQueue.TryDequeue(out experiment))
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
        }

        private void OnExperimentComplete(Experiment experiment)
        {
            // Mark the experiment as completed.
            experiment.Complete();

            if (experiment.Succeeded)
            {
                // Decide if this is the best experiment we've seen so far.
                if ((_bestExperiment == null) ||
                   (_bestExperiment.CommittedList.Count < experiment.CommittedList.Count))
                {
                    _bestExperiment = experiment;
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
            Experiment[] experiments = new Experiment[numExperiments];
            ZeroBlockList baseBlockList = baseExperiment.CommittedList;
            ulong nextZeroBlock = baseBlockList.GetHighestAddress() + Config.BlockSize;
            for (int i=0; i<numExperiments; i++)
            {
                // Generate the next block list.
                ZeroBlockList attemptingList = new ZeroBlockList();
                attemptingList.Add(nextZeroBlock);
                nextZeroBlock += Config.BlockSize;

                // Create a new experiment.
                experiments[i] = new Experiment(baseExperiment.CommittedList, attemptingList);
            }

            return experiments;
        }
    }
}
