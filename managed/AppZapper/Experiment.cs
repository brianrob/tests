using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AppZapper
{
    public sealed class Experiment
    {
        private static long ExperimentCounter = 1;
        private long _experimentNumber;
        private string _experimentPath;
        private Process _experimentProcess;
        private ZeroBlockList _committedList;
        private ZeroBlockList _attemptingList;

        public Experiment(ZeroBlockList committedList, ZeroBlockList attemptingList)
        {
            // Assign an experiment number and increment the global counter.
            _experimentNumber = ExperimentCounter++;

            // Save the zero block lists.
            _committedList = committedList;
            _attemptingList = attemptingList;
        }

        public bool Succeeded
        {
            get; private set;
        }

        public ZeroBlockList CommittedList
        {
            get { return _committedList; }
        }

        public ZeroBlockList AttemptingList
        {
            get { return _attemptingList; }
        }

        public void Execute()
        {
            Log("========");
            Log($"New Experiment [{_experimentNumber}]");
            Log("========");

            // Make a new directory for the experiment.
            _experimentPath = Path.Combine(Config.TempDirectoryRoot, _experimentNumber.ToString());
            Directory.CreateDirectory(_experimentPath);

            // Copy the app into the experiment directory.
            Utilities.CopyDirectory(Config.AppDirectory, _experimentPath, true);

            // TODO: Modify the binary.

            // Start the app.
            Log("Starting test process.");
            ProcessStartInfo startInfo = GetStartInfo();
            _experimentProcess = Process.Start(startInfo);

            // Verify that the app started properly.
            bool processStartedSuccessfully = false;

            Task watchConsoleTask = Task.Run(() =>
            {
                StreamReader standardOutput = _experimentProcess.StandardOutput;
                while (!standardOutput.EndOfStream)
                {
                    // Read the line.
                    string line = standardOutput.ReadLine();

                    // Write it to the console.
                    Log(line);

                    // See if the line matches what we're looking for.
                    if (line.StartsWith(Config.AppStartupSuccessStartString))
                    {
                        Log("Process started successfully.");
                        processStartedSuccessfully = true;
                        break;
                    }
                }
            });

            DateTime timeOutTime = DateTime.UtcNow.Add(Config.OperationTimeout);

            while ((!processStartedSuccessfully) && (!watchConsoleTask.IsCompleted) && (timeOutTime > DateTime.UtcNow))
            {
                watchConsoleTask.Wait(Config.PollingInterval);
            }

            // If the app didn't start successfully, kill it and abort the experiment.
            if(!processStartedSuccessfully)
            {
                _experimentProcess.Kill(true);
                return;
            }

            // Exercise the app.
            Task requestsTask = SendAndValidateRequests();
            requestsTask.Wait(Config.OperationTimeout);

            // Shutdown the app.
            // TODO: Can we do this gracefully and make sure it doesn't crash?
            _experimentProcess.Kill(true);

            // Mark the experiment as succeeded if everything completed successfully.
            if(requestsTask.IsCompleted && (requestsTask.Exception == null))
            {
                Succeeded = true;
            }
        }

        private async Task SendAndValidateRequests()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                for (int i = 0; i < Config.NumRequests; i++)
                {
                    HttpResponseMessage responseMessage = await httpClient.GetAsync(Config.ServerUrl);
                    responseMessage.EnsureSuccessStatusCode();
                    string body = await responseMessage.Content.ReadAsStringAsync();
                    WeatherForecast[] forecastResponse = JsonSerializer.Deserialize<WeatherForecast[]>(body);
                    foreach (WeatherForecast forecast in forecastResponse)
                    {
                        if (!((forecast.Date != default) &&
                            !string.IsNullOrEmpty(forecast.Summary) &&
                            (forecast.TemperatureC >= -20 && forecast.TemperatureC <= 55) &&
                            (forecast.TemperatureF == (32 + (int)(forecast.TemperatureC / 0.5556)))))
                        {
                            throw new InvalidOperationException("The response provided does not match the expected result.");
                        }
                    }
                }
            }
        }

        private ProcessStartInfo GetStartInfo()
        {
            return new ProcessStartInfo(Path.Combine(_experimentPath, Config.AppExeName))
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
        }

        private void Log(string message)
        {
            Console.WriteLine($"[{_experimentNumber}] {message}");
        }

        public void CopyTo(string destinationPath)
        {
            if (Directory.Exists(destinationPath))
            {
                Directory.Delete(destinationPath, recursive: true);
            }
            Directory.CreateDirectory(destinationPath);

            Utilities.CopyDirectory(_experimentPath, destinationPath, true);
        }

        public void Complete()
        {
            if(Succeeded)
            {
                ZeroBlockList committedList = new ZeroBlockList();
                committedList.AddRange(_committedList);
                committedList.AddRange(_attemptingList);
                _committedList = committedList;

                _attemptingList = new ZeroBlockList();
            }

            Delete();
        }

        public void Delete()
        {
            if (Directory.Exists(_experimentPath))
            {
                Directory.Delete(_experimentPath, true);
            }
        }
    }

    /// <summary>
    /// Structure to deserialize the message body into.
    /// </summary>
    public class WeatherForecast
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("temperatureC")]
        public int TemperatureC { get; set; }
        [JsonPropertyName("temperatureF")]
        public int TemperatureF { get; set; }
        [JsonPropertyName("summary")]
        public string Summary { get; set; }
    }
}
