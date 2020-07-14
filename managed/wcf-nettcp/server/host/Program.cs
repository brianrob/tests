using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using GettingStartedLib;

namespace GettingStartedHost
{
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1: Create a URI to serve as the base address.
            Uri baseAddress = new Uri("net.tcp://localhost:8734/GettingStarted/");

            // Step 2: Create a ServiceHost instance.
            ServiceHost selfHost = new ServiceHost(typeof(CalculatorService));

            try
            {
                // Step 3: Add a service endpoint.
                NetTcpBinding binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.None;
                selfHost.AddServiceEndpoint(typeof(ICalculator), binding, "net.tcp://localhost:8734/GettingStarted/CalculatorService/");

                // Step 4: Enable metadata exchange.
                //ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                //smb.HttpGetEnabled = true;
                //selfHost.Description.Behaviors.Add(smb);

                // Step 5: Start the service.
                selfHost.Open();
                Console.WriteLine("The service is ready.");

                // Close the ServiceHost to stop the service.
                Console.WriteLine("Press <Enter> to terminate the service.");
                Console.WriteLine();
                Console.ReadLine();
                selfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
                selfHost.Abort();
            }
        }
    }
}