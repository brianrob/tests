using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Etlx;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;

namespace ProcessTreeWriter
{
    class Program
    {
        // Dictionary to store process information
        private static Dictionary<int, ProcessInfo> _processes = new Dictionary<int, ProcessInfo>();          static void Main(string[] args)
        {
            // Check if ETL file path is provided
            if (args.Length == 0)
            {
                Console.WriteLine("Error: ETL file path not provided.");
                Console.WriteLine("Usage: ProcessTreeWriter <etl-file-path> [process-id] [filtered-processes]");
                Console.WriteLine("       filtered-processes: Optional comma-separated list of process names to exclude");
                return;
            }            
            string inputFilePath = args[0];
            int? rootPid = null;
            HashSet<string>? filteredProcesses = null;
            
            // Process command line arguments
            for (int i = 1; i < args.Length; i++)
            {
                // If argument is a number, treat it as process ID
                if (int.TryParse(args[i], out int pid))
                {
                    rootPid = pid;
                }
                // Otherwise, treat it as a comma-delimited list of process names to filter
                else
                {
                    filteredProcesses = new HashSet<string>(
                        args[i].Split(',').Select(p => p.Trim().ToLower()),
                        StringComparer.OrdinalIgnoreCase);
                }
            }
            
            try
            {                // Get the actual ETL file path (handle zip files if needed)
                string etlFilePath = PrepareEtlFile(inputFilePath);
                
                ParseEtlFile(etlFilePath);
                PrintProcessTree(rootPid, filteredProcesses);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing ETL file: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }        static string PrepareEtlFile(string filePath)
        {
            // If it's a zip file, extract it and find the ETL file
            if (filePath.EndsWith(".etl.zip", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Detected compressed ETL file: {filePath}");
                
                // Create extraction directory next to the zip file
                string directoryName = Path.GetDirectoryName(filePath) ?? "";
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath); // Removes .zip extension
                string extractionPath = Path.Combine(directoryName, fileNameWithoutExt + "_extracted");
                
                // Create the directory if it doesn't exist
                if (!Directory.Exists(extractionPath))
                {
                    Directory.CreateDirectory(extractionPath);
                }
                else
                {
                    // If directory exists, clean it up first
                    Console.WriteLine($"Cleaning up existing extraction directory: {extractionPath}");
                    foreach (var file in Directory.GetFiles(extractionPath))
                    {
                        File.Delete(file);
                    }
                }
                
                // Extract the zip file
                Console.WriteLine($"Extracting to: {extractionPath}");
                ZipFile.ExtractToDirectory(filePath, extractionPath);
                
                // Look for .etl files in the extraction directory
                var etlFiles = Directory.GetFiles(extractionPath, "*.etl", SearchOption.AllDirectories);
                
                if (etlFiles.Length == 0)
                {
                    throw new FileNotFoundException($"No ETL files found in the extracted directory: {extractionPath}");
                }
                else if (etlFiles.Length > 1)
                {
                    Console.WriteLine($"Multiple ETL files found. Using the first one: {etlFiles[0]}");
                }
                
                return etlFiles[0]; // Return the path to the first ETL file found
            }
            
            // If it's not a zip file, just return the original path
            return filePath;
        }
        
        static void ParseEtlFile(string etlFilePath)
        {
            Console.WriteLine($"Processing ETL file: {etlFilePath}");
            
            // Create a TraceEventSession to process the ETL file
            using (var source = new ETWTraceEventSource(etlFilePath))
            {
                // Subscribe to process start events
                source.Kernel.ProcessStart += Kernel_ProcessStart;
                
                // Subscribe to process stop events
                source.Kernel.ProcessStop += Kernel_ProcessStop;
                
                // Process the events
                source.Process();
            }
        }

        static void Kernel_ProcessStart(ProcessTraceData data)
        {
            // Create and store process information
            var processInfo = new ProcessInfo
            {
                ProcessID = data.ProcessID,
                ParentID = data.ParentID,
                ProcessName = data.ProcessName,
                CommandLine = data.CommandLine,
                StartTime = data.TimeStamp
            };
            
            _processes[data.ProcessID] = processInfo;
        }

        static void Kernel_ProcessStop(ProcessTraceData data)
        {            // Update the process with stop time if it exists
            if (_processes.TryGetValue(data.ProcessID, out ProcessInfo? processInfo) && processInfo != null)
            {
                processInfo.EndTime = data.TimeStamp;
                processInfo.ExitCode = data.ExitStatus;
            }
        }        static void PrintProcessTree(int? rootPid = null, HashSet<string>? filteredProcesses = null)
        {
            Console.WriteLine("\nProcess Tree:");
            Console.WriteLine("=============");

            // Display filter information if applicable
            if (filteredProcesses != null && filteredProcesses.Count > 0)
            {
                Console.WriteLine($"Filtering out these processes and their children: {string.Join(", ", filteredProcesses)}");
            }

            List<ProcessInfo> rootProcesses;

            if (rootPid.HasValue)
            {
                // If a specific process ID is provided, only show that subtree
                if (_processes.TryGetValue(rootPid.Value, out ProcessInfo? rootProcess) && rootProcess != null)
                {
                    Console.WriteLine($"Showing subtree for process ID: {rootPid.Value}");
                    
                    // Check if the root process should be filtered out
                    if (filteredProcesses != null && ShouldFilterProcess(rootProcess, filteredProcesses))
                    {
                        Console.WriteLine($"Process {rootProcess.ProcessName} (ID: {rootProcess.ProcessID}) is filtered out.");
                        return;
                    }
                    
                    rootProcesses = new List<ProcessInfo> { rootProcess };
                }
                else
                {
                    Console.WriteLine($"Process with ID {rootPid.Value} not found in the trace.");
                    return;
                }
            }
            else
            {
                // Find all root processes (those without a parent in our collection)
                rootProcesses = _processes.Values
                    .Where(p => !_processes.ContainsKey(p.ParentID))
                    .OrderBy(p => p.StartTime)
                    .ToList();
            }            // Print each root process and its children recursively
            foreach (var rootProcess in rootProcesses)
            {
                // Use the root process's start time as reference for elapsed time calculations
                DateTime rootStartTime = rootProcess.StartTime;
                PrintProcessNode(rootProcess, 0, filteredProcesses, rootStartTime);
            }
        }

        // Helper method to check if a process should be filtered out
        static bool ShouldFilterProcess(ProcessInfo process, HashSet<string> filteredProcesses)
        {
            return filteredProcesses.Contains(process.ProcessName);
        }        static void PrintProcessNode(ProcessInfo process, int indentLevel, HashSet<string>? filteredProcesses = null, DateTime rootStartTime = default)
        {
            // Check if this process should be filtered out
            if (filteredProcesses != null && ShouldFilterProcess(process, filteredProcesses))
            {
                // Skip this process and its children
                return;
            }
            
            // Create the indent string based on the level
            string indent = new string(' ', indentLevel * 2);
            
            // Calculate the process lifetime
            string lifetimeInfo = "";
            if (process.EndTime != DateTime.MinValue)
            {
                TimeSpan lifetime = process.EndTime - process.StartTime;
                lifetimeInfo = $" (Ran for {FormatTimeSpan(lifetime)})";
            }
            else
            {
                lifetimeInfo = " (Still running at trace end)";
            }
            
            // Calculate time elapsed since root process started
            string elapsedTime = "";
            if (rootStartTime != default)
            {
                TimeSpan elapsed = process.StartTime - rootStartTime;
                elapsedTime = $"+{FormatTimeSpan(elapsed)} ";
            }
              
            // Process command line - limit to 200 characters
            string commandLine = string.IsNullOrEmpty(process.CommandLine) ? "" : " " + TruncateString(process.CommandLine, 200);
            
            // Print this process info with elapsed time
            Console.WriteLine($"{indent}{elapsedTime}[{process.ProcessID}] {process.ProcessName}{lifetimeInfo}{commandLine}");
              
            // Find and print all child processes
            var children = _processes.Values
                .Where(p => p.ParentID == process.ProcessID)
                .OrderBy(p => p.StartTime)
                .ToList();
            
            foreach (var child in children)
            {
                PrintProcessNode(child, indentLevel + 1, filteredProcesses, rootStartTime);
            }
        }
          static string FormatTimeSpan(TimeSpan span)
        {
            if (span.TotalSeconds < 1)
            {
                return $"{span.TotalMilliseconds:F0} ms";
            }
            else if (span.TotalMinutes < 1)
            {
                return $"{span.TotalSeconds:F2} sec";
            }
            else if (span.TotalHours < 1)
            {
                return $"{span.TotalMinutes:F2} min";
            }
            else
            {
                return $"{span.TotalHours:F2} hours";
            }
        }
        
        // Helper method to truncate a string to a specified length and add ellipsis if needed
        static string TruncateString(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            
            if (input.Length <= maxLength)
            {
                return input;
            }
            
            return input.Substring(0, maxLength - 3) + "...";
        }
    }    class ProcessInfo
    {
        public int ProcessID { get; set; }
        public int ParentID { get; set; }
        public string ProcessName { get; set; } = string.Empty;
        public string CommandLine { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ExitCode { get; set; }
    }
}
