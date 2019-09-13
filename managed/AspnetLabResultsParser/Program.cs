using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Table;

namespace AspnetLabResultsParser
{
    class Program
    {
        private static string ScenarioName;
        private static string ResultsDirectory;

        static void Main(string[] args)
        {
            // Get the scenario name we want to process.
            if(args.Length <= 1)
            {
                Console.WriteLine("Usage: AspnetLabResultsParser.exe <scenarioName> <directory>");
                return;
            }

            ScenarioName = args[0];
            Console.WriteLine($"Processing scenario '{ScenarioName}'.");

            ResultsDirectory = args[1];
            Console.WriteLine($"Directory: '{ResultsDirectory}'");

            // Create the test results data structure.
            TestResultCollection resultsCollection = new TestResultCollection();

            // Iterate over all of the scenario files.
            string searchPattern = ScenarioName + "*.bench.json";
            string[] resultFiles = Directory.GetFiles(ResultsDirectory, searchPattern);
            foreach (string resultFile in resultFiles)
            {
                Console.WriteLine($"Processing '{resultFile}'.");

                // Read the file into a string.
                string serializedJson = File.ReadAllText(resultFile);

                // Deserialize the file.
                TestResult result = JsonConvert.DeserializeObject<TestResult>(serializedJson);

                // Set the test metadata.
                SetScenarioMetadata(resultFile, result);

                // Add the result to the collection.
                resultsCollection.AddResult(result);
            }

            ResultsTable throughputTable = new ResultsTable(resultsCollection, ResultsTableType.RequestsPerSecond);
            throughputTable.Save("c:\\work\\results\\throughput.csv");

            ResultsTable socketErrorsTable = new ResultsTable(resultsCollection, ResultsTableType.SocketErrors);
            socketErrorsTable.Save("c:\\work\\results\\socketerrors.csv");
        }

        private static void SetScenarioMetadata(string fileName, TestResult result)
        {
            // Parser the file name.
            string[] tokens = fileName.Split('.');
            if (tokens.Length != 7)
            {
                // Check to see if this is a core_3.0 file.
                if (fileName.Contains("core_3.0") && tokens.Length == 8)
                {
                    string[] newTokens = new string[7];
                    newTokens[0] = tokens[0];
                    newTokens[1] = tokens[1] + "." + tokens[2];
                    newTokens[2] = tokens[3];
                    newTokens[3] = tokens[4];
                    newTokens[4] = tokens[5];
                    newTokens[5] = tokens[6];
                    newTokens[6] = tokens[7];

                    tokens = newTokens;
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Invalid file name '{fileName}'.");
                }
            }

            if (!tokens[0].EndsWith(ScenarioName))
            {
                throw new ArgumentOutOfRangeException($"File name '{fileName}' does not match scenario '{ScenarioName}'.");
            }

            result.ScenarioName = ScenarioName;
            result.ConfigurationName = tokens[1];
            
            // Token is of the form mem<value>.  Skip "mem".
            result.CommitMB = Convert.ToInt32(tokens[2].Substring(3));

            // Token is of the form swap<value>.  Skip "swap".
            result.SwapMB = Convert.ToInt32(tokens[3].Substring(4));
        }
    }
}
