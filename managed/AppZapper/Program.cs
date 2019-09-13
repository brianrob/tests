using System;

namespace AppZapper
{
    class Program
    {
        static void Main(string[] args)
        {
            string latestSuccessfulDirectory = Config.AppDirectory;
            while (true)
            {
                // Create a new experiment.
                Experiment experiment = new Experiment(latestSuccessfulDirectory, Config.TempDirectoryRoot);

                // Execute the experiment.
                experiment.Execute();

                // If the experiment succeeded, save it and use it as the next experiment's baseline.
                if(experiment.Succeeded)
                {
                    experiment.CopyTo(Config.LatestSuccessfulBits);
                    latestSuccessfulDirectory = Config.LatestSuccessfulBits;
                }
                else
                {
                    // TODO should we delete the experiment?
                    //experiment.Delete();
                }
            }
        }
    }
}
