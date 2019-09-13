using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspnetLabResultsParser
{
    public sealed class TestResult
    {
        public string ScenarioName
        {
            get; set;
        }

        public string ConfigurationName
        {
            get; set;
        }

        public int CommitMB
        {
            get; set;
        }

        public int SwapMB
        {
            get; set;
        }

        public long RequestsPerSecond
        {
            get; set;
        }

        public long SocketErrors
        {
            get; set;
        }

        public long BadResponses
        {
            get; set;
        }
    }
}
