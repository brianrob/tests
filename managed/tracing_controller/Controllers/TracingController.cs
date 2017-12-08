using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventPipe;

namespace tracing_controller.Controllers
{
    [Route("/trace")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "start", "stop" };
        }

        [HttpGet("start")]
        public string StartTracing()
        {
            TraceControl.EnableDefault();
            return "Tracing Started.";
        }

        [HttpGet("stop")]
        public string StopTracing()
        {
            TraceControl.Disable();
            return "Tracing Stopped.";
        }
    }
}
