using System;
using Microsoft.Diagnostics.Tracing.AutomatedAnalysis;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration configuration = new Configuration();
            configuration.AddConfigurationFile(@"c:\src\tests\managed\xml-config\test.config.xml");
            Console.WriteLine("Hello World!");
        }
    }
}
