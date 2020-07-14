using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculatorServiceReference;
using client;

namespace GettingStartedClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string mode = "async";
            int numThreads = 1;
            if(args.Length > 0)
            {
                mode = args[0];
                if(args.Length > 1)
                {
                    numThreads = Convert.ToInt32(args[1]);
                }
            }

            Console.WriteLine($"Executing in {mode} mode with {numThreads} threads.");

            Action theAction;
            if(mode.Equals("async"))
            {
                theAction = new Action(AsyncTask);
            }
            else if(mode.Equals("sync"))
            {
                theAction = new Action(SyncTask);
            }
            else
            {
                throw new NotImplementedException();
            }

            Task[] tasks = new Task[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                tasks[i] = Task.Factory.StartNew(theAction, TaskCreationOptions.LongRunning);
            }

            Console.ReadKey();
        }

        private static async void AsyncTask()
        {
            using (CalculatorClient client = new CalculatorClient("NetTcpBinding_ICalculator"))
            {
                int sleepTime;
                do
                {
                    ClientEventSource.Log.RequestStart();
                    sleepTime = await client.SleepAsync(10);
                    ClientEventSource.Log.RequestStop();
                }
                while (sleepTime > 0);
            }
        }

        private static void SyncTask()
        {
            using (CalculatorClient client = new CalculatorClient("NetTcpBinding_ICalculator"))
            {
                int sleepTime;
                do
                {
                    ClientEventSource.Log.RequestStart();
                    sleepTime = client.Sleep(10);
                    ClientEventSource.Log.RequestStop();
                }
                while(sleepTime > 0);
            }
        }

        //private void Sample()
        //{
        //    //Step 1: Create an instance of the WCF proxy.
        //    CalculatorClient client = new CalculatorClient("NetTcpBinding_ICalculator");

        //    // Step 2: Call the service operations.
        //    // Call the Add service operation.
        //    double value1 = 100.00D;
        //    double value2 = 15.99D;
        //    //double result = await client.AddAsync(value1, value2);
        //    double result = client.Add(value1, value2);
        //    Console.WriteLine("Add({0},{1}) = {2}", value1, value2, result);

        //    // Call the Subtract service operation.
        //    value1 = 145.00D;
        //    value2 = 76.54D;
        //    result = await client.SubtractAsync(value1, value2);
        //    Console.WriteLine("Subtract({0},{1}) = {2}", value1, value2, result);

        //    // Call the Multiply service operation.
        //    value1 = 9.00D;
        //    value2 = 81.25D;
        //    result = await client.MultiplyAsync(value1, value2);
        //    Console.WriteLine("Multiply({0},{1}) = {2}", value1, value2, result);

        //    // Call the Divide service operation.
        //    value1 = 22.00D;
        //    value2 = 7.00D;
        //    result = await client.DivideAsync(value1, value2);
        //    Console.WriteLine("Divide({0},{1}) = {2}", value1, value2, result);

        //    await client.SleepAsync(1000);

        //    // Step 3: Close the client to gracefully close the connection and clean up resources.
        //    Console.WriteLine("\nPress <Enter> to terminate the client.");
        //    Console.ReadLine();
        //    client.Close();
        //}

    }
}