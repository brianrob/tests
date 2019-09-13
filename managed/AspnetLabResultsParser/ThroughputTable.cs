using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Table;

namespace AspnetLabResultsParser
{
    public enum ResultsTableType
    {
        RequestsPerSecond,
        SocketErrors
    }

    public sealed class ResultsTable
    {
        private const string ConfigName_NETCore = "core_3.0";
        private const string ConfigName_MonoJIT = "mono_jit";
        private const string ConfigName_MonoNET5 = "mono_net5";
        private const string ConfigName_GoFastHTTP = "go";

        private const string ColumnName_Memory = "Memory Limit (Commit/Swap)";
        private const string ColumnName_NETCore = ".NET Core 3.0";
        private const string ColumnName_MonoJIT = "Mono JIT";
        private const string ColumnName_MonoNET5 = "Mono .NET 5";
        private const string ColumnName_GoFastHTTP = "Go FastHttp";

        private TestResultCollection m_ResultCollection;
        private ResultsTableType m_TableType;
        private DataTable m_Table;
        private ColumnName m_MemoryColumn;
        private ColumnName m_NETCoreColumn;
        private ColumnName m_MonoJITColumn;
        private ColumnName m_MonoNET5Column;
        private ColumnName m_GoFastHTTPColumn;

        public ResultsTable(TestResultCollection resultCollection, ResultsTableType tableType)
        {
            m_ResultCollection = resultCollection;
            m_TableType = tableType;
            BuildTable();
        }

        public void Save(string outputPath)
        {
            m_Table.WriteToCSV(outputPath);
        }

        private void BuildTable()
        {
            // Build the throughput table.
            m_Table = new DataTable();
            m_MemoryColumn = m_Table.AddColumn(ColumnName_Memory);
            m_NETCoreColumn = m_Table.AddColumn(ColumnName_NETCore);
            m_MonoJITColumn = m_Table.AddColumn(ColumnName_MonoJIT);
            m_MonoNET5Column = m_Table.AddColumn(ColumnName_MonoNET5);
            m_GoFastHTTPColumn = m_Table.AddColumn(ColumnName_GoFastHTTP);

            foreach (TestScenarioBucket scenarioBucket in m_ResultCollection.Buckets)
            {
                // Make a new row.
                Row row = m_Table.AppendRow();
                row[m_MemoryColumn] = $"{scenarioBucket.CommitMB}MB/{scenarioBucket.SwapMB}MB";

                foreach (TestResultBucket resultBucket in scenarioBucket.ResultBuckets)
                {
                    // Get the column for this result bucket.
                    ColumnName columnName = GetColumnName(resultBucket);

                    // Compute the average of the results.
                    double averageValue;
                    switch (m_TableType)
                    {
                        case ResultsTableType.RequestsPerSecond:
                            averageValue = resultBucket.Results.Select(r => r.RequestsPerSecond).Average();
                            break;
                        case ResultsTableType.SocketErrors:
                            averageValue = resultBucket.Results.Select(r => r.SocketErrors).Average();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(m_TableType));
                    }

                    // Set the column value.
                    row[columnName] = averageValue.ToString();
                }
            }
        }

        private ColumnName GetColumnName(TestResultBucket resultBucket)
        {
            ColumnName retVal = null;
            if (ConfigName_NETCore.Equals(resultBucket.ConfigurationName))
            {
                retVal = m_NETCoreColumn;
            }
            else if (ConfigName_MonoJIT.Equals(resultBucket.ConfigurationName))
            {
                retVal = m_MonoJITColumn;
            }
            else if (ConfigName_MonoNET5.Equals(resultBucket.ConfigurationName))
            {
                retVal = m_MonoNET5Column;
            }
            else if (ConfigName_GoFastHTTP.Equals(resultBucket.ConfigurationName))
            {
                retVal = m_GoFastHTTPColumn;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(resultBucket.ConfigurationName));
            }

            return retVal;
        }
    }
}
