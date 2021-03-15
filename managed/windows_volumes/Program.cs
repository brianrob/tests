using System;
using System.IO;
using System.Reflection;

namespace windows_volumes
{
    class Program
    {
        static void Main(string[] args)
        {
            int pid = System.Diagnostics.Process.GetCurrentProcess().Id;
            Console.WriteLine($"PID: {pid}");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();

            DisableLegacyPathHandling();

            string input = @"c:\device\harddiskvolume7\sdk\dotnet-sdk-3.1.401-win-x64\shared\microsoft.netcore.app\3.1.7\coreclr.dll";
            string output = WindowsDeviceToVolumeMap.Instance.ConvertDevicePathToVolumePath(input);

            Console.WriteLine($"Input: {input}");
            Console.WriteLine($"Output: {output}");
            if(File.Exists(output))
            {
                File.Copy(output, @"C:\users\brianrob\desktop\coreclr.dll");
                Console.WriteLine("Copied.");
            }
        }

        private static void DisableLegacyPathHandling()
        {
            Assembly systemAssembly = Assembly.Load("mscorlib");
            if (systemAssembly != null)
            {
                // Disable the AppContext switch.
                Type appContextType = systemAssembly.GetType("System.AppContext");
                if (appContextType != null)
                {
                    MethodInfo setSwitchMethod = appContextType.GetMethod("SetSwitch");
                    if (setSwitchMethod != null)
                    {
                        setSwitchMethod.Invoke(null, new object[] { "Switch.System.IO.UseLegacyPathHandling", false });
                    }
                }

                // Invalidate the cached copy of the switch.
                Type appContextSwitchesType = systemAssembly.GetType("System.AppContextSwitches");
                if(appContextSwitchesType != null)
                {
                    FieldInfo useLegacyPathHandlingField = appContextSwitchesType.GetField("_useLegacyPathHandling", BindingFlags.NonPublic | BindingFlags.Static);
                    if(useLegacyPathHandlingField != null)
                    {
                        useLegacyPathHandlingField.SetValue(0, 0);
                    }
                }
            }
        }
    }
}
