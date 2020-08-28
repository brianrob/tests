using System;
using System.IO;

namespace machine_info
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach(DriveInfo driveInfo in DriveInfo.GetDrives())
            {
                Console.WriteLine($"Name: {driveInfo.Name}, Format: {driveInfo.DriveFormat}, Type: {driveInfo.DriveType}, Name: {driveInfo.Name}, VolumeLabel: {driveInfo.VolumeLabel}, TotalSize: {driveInfo.TotalSize}, TotalFreeSpace: {driveInfo.TotalFreeSpace}, AvailableFreeSpace {driveInfo.AvailableFreeSpace}");
            }
            Console.ReadKey();
        }
    }
}
