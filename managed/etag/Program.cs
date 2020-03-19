using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace etag
{
    class Program
    {
        private static readonly Uri _searchMetadataUri = new Uri("https://go.microsoft.com/fwlink/?linkid=2087906&clcid=0x409");
        private const string EtagFileName = "etag.txt";

        static void Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                if(File.Exists(EtagFileName))
                {
                    client.DefaultRequestHeaders.Add("If-None-Match", $"\"{File.ReadAllText(EtagFileName)}\"");
                }

                using (HttpResponseMessage response = client.GetAsync(_searchMetadataUri).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string resultText = response.Content.ReadAsStringAsync().Result;
                        GC.KeepAlive(resultText);
                        IEnumerable<string> etagValues;
                        if(response.Headers.TryGetValues("ETag", out etagValues))
                        {
                            if (etagValues.Count() == 1)
                            {
                                File.WriteAllText(EtagFileName, etagValues.First());
                            }
                        }
                    }
                }
            }
        }
    }
}
