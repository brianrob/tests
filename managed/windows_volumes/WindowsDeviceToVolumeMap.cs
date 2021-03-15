using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace windows_volumes
{
    public sealed class WindowsDeviceToVolumeMap
    {
        public static WindowsDeviceToVolumeMap Instance = new WindowsDeviceToVolumeMap();

        private const string VolumeNamePrefix = @"\\?\";
        private const string VolumeNameSuffix = "\\";

        private const string HardDiskVolumePathToken = "Device\\HarddiskVolume";
        private const int HardDiskVolumePathTokenIndex = 3;
        private const string VHDHardDiskPathToken = "Device\\VhdHardDisk{";
        private const int VHDHardDiskPathTokenIndex = 3;

        private Dictionary<string, string> _deviceNameToVolumeNameMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private WindowsDeviceToVolumeMap()
        {
            Initialize();
        }

        public string ConvertDevicePathToVolumePath(string inputPath)
        {
            string updatedPath = inputPath;

            // Check for "Device\HarddiskVolume" pattern.
            if(inputPath.IndexOf(HardDiskVolumePathToken, 0, StringComparison.OrdinalIgnoreCase) == HardDiskVolumePathTokenIndex)
            {
                // Get the device path and see if we have a match.
                int indexOfSlashAfterDeviceName = inputPath.IndexOf("\\", HardDiskVolumePathTokenIndex + HardDiskVolumePathToken.Length, StringComparison.OrdinalIgnoreCase);
                string deviceName = inputPath.Substring(2, indexOfSlashAfterDeviceName - 2);

                string volumePath;
                if(_deviceNameToVolumeNameMap.TryGetValue(deviceName, out volumePath))
                {
                    // Get the rest of the path.
                    string restOfPath = inputPath.Substring(indexOfSlashAfterDeviceName + 1);

                    // Replace the device path with the volume path.
                    updatedPath = Path.Combine(volumePath, restOfPath);
                }
            }
            // Check for "Device\VhdHardDisk{GUID}" pattern.
            else if (inputPath.IndexOf(VHDHardDiskPathToken, 0, StringComparison.OrdinalIgnoreCase) == VHDHardDiskPathTokenIndex)
            {
                // Get the device path and see if we have a match.
                int indexOfSlashAfterDeviceName = inputPath.IndexOf("\\", VHDHardDiskPathTokenIndex + VHDHardDiskPathToken.Length, StringComparison.OrdinalIgnoreCase);
                string deviceName = inputPath.Substring(2, indexOfSlashAfterDeviceName - 2);

                string volumePath;
                if (_deviceNameToVolumeNameMap.TryGetValue(deviceName, out volumePath))
                {
                    // Get the rest of the path.
                    string restOfPath = inputPath.Substring(indexOfSlashAfterDeviceName + 1);

                    // Replace the device path with the volume path.
                    updatedPath = Path.Combine(volumePath, restOfPath);
                }
            }

            return updatedPath;
        }

        private void Initialize()
        {
            // Create a string builder which will act as the buffer used to receive the volume and device names.
            StringBuilder builder = new StringBuilder((int)Interop.MAX_PATH, (int)Interop.MAX_PATH);

            // Get the first volume.
            IntPtr findHandle = Interop.FindFirstVolume(builder, Interop.MAX_PATH);
            try
            {
                do
                {
                    string volumeName = builder.ToString();
                    string deviceName = string.Empty;
                    string lookupKey = volumeName;

                    // Strip off the volume name prefix and suffix.
                    if (lookupKey.StartsWith(VolumeNamePrefix))
                    {
                        lookupKey = lookupKey.Substring(VolumeNamePrefix.Length);
                    }
                    if (lookupKey.EndsWith(VolumeNameSuffix))
                    {
                        lookupKey = lookupKey.Substring(0, lookupKey.Length - VolumeNameSuffix.Length);
                    }

                    // Get the device name.
                    uint charsWritten = Interop.QueryDosDevice(lookupKey, builder, (int)Interop.MAX_PATH);
                    if (charsWritten > 0)
                    {
                        deviceName = builder.ToString();
                    }

                    // Save the mapping.
                    if (volumeName.Length > 0 && deviceName.Length > 0)
                    {
                        _deviceNameToVolumeNameMap.Add(deviceName, volumeName);
                    }
                }
                while (Interop.FindNextVolume(findHandle, builder, Interop.MAX_PATH));
            }
            finally
            {
                // Close the live volume handle.
                if (findHandle != IntPtr.Zero)
                {
                    Interop.FindVolumeClose(findHandle);
                }
            }
        }
    }

    internal static class Interop
    {
        public const UInt32 MAX_PATH = 1024;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr FindFirstVolume(
            [Out] StringBuilder lpszVolumeName,
            uint cchBufferLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool FindNextVolume(
            IntPtr hFindVolume,
            [Out] StringBuilder lpszVolumeName,
            uint cchBufferLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool FindVolumeClose(
            IntPtr hFindVolume);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern uint QueryDosDevice(
            string lpDeviceName,
            StringBuilder lpTargetPath,
            int ucchMax);
    }
}
