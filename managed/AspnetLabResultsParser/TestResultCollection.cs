using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspnetLabResultsParser
{
    public sealed class TestResultCollection
    {
        private SortedDictionary<int, TestScenarioBucket> m_BackingCollection = new SortedDictionary<int, TestScenarioBucket>();

        public IEnumerable<TestScenarioBucket> Buckets
        {
            get
            {
                foreach(TestScenarioBucket bucket in m_BackingCollection.Values)
                {
                    yield return bucket;
                }
            }
        }

        public void AddResult(TestResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }
            if (result.CommitMB <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(result.CommitMB));
            }

            // Get or create the commit bucket.
            TestScenarioBucket bucket = GetOrCreateBucket(result);

            // Add the result to the bucket.
            bucket.AddResult(result);
        }

        private TestScenarioBucket GetOrCreateBucket(TestResult result)
        {
            TestScenarioBucket bucket;
            if (!m_BackingCollection.TryGetValue(result.CommitMB, out bucket))
            {
                bucket = new TestScenarioBucket(result.CommitMB, result.SwapMB);
                m_BackingCollection.Add(result.CommitMB, bucket);
            }

            return bucket;
        }
    }

    public sealed class TestScenarioBucket
    {
        private Dictionary<string, TestResultBucket> m_BackingCollection = new Dictionary<string, TestResultBucket>(4);

        public TestScenarioBucket(int commitMB, int swapMB)
        {
            CommitMB = commitMB;
            SwapMB = swapMB;
        }

        public int CommitMB
        {
            get;
        }

        public int SwapMB
        {
            get;
        }

        public void AddResult(TestResult result)
        {
            TestResultBucket resultBucket;
            if(!m_BackingCollection.TryGetValue(result.ConfigurationName, out resultBucket))
            {
                resultBucket = new TestResultBucket(result.ConfigurationName);
                m_BackingCollection.Add(resultBucket.ConfigurationName, resultBucket);
            }

            resultBucket.AddResult(result);
        }

        public IEnumerable<TestResultBucket> ResultBuckets
        {
            get
            {
                foreach (TestResultBucket resultBucket in m_BackingCollection.Values)
                {
                    yield return resultBucket;
                }
            }
        }
    }

    public sealed class TestResultBucket
    {
        private List<TestResult> m_BackingCollection = new List<TestResult>(3);

        public TestResultBucket(string configurationName)
        {
            ConfigurationName = configurationName;
        }

        public string ConfigurationName
        {
            get;
        }

        public void AddResult(TestResult result)
        {
            m_BackingCollection.Add(result);
        }

        public IEnumerable<TestResult> Results
        {
            get
            {
                foreach (TestResult result in m_BackingCollection)
                {
                    yield return result;
                }
            }
        }
    }
}
