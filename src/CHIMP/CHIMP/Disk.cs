using System;
using System.Runtime.InteropServices;

namespace Chimp
{
    public sealed class Disk
    {
        private Volume Volume { get; }

        public Disk(Volume volume)
        {
            Volume = volume;

            _deviceName = new Lazy<string>(GetDeviceName);

            GetDiskParams(out uint sectorSize, out ulong driveSectors, out ulong diskSize);
            SectorSize = sectorSize;
            DriveSectors = driveSectors;
            DiskSize = diskSize;
        }

        public uint SectorSize { get; }
        public ulong DriveSectors { get; }
        public ulong DiskSize { get; }

        private void GetDiskParams(out uint sectorSize, out ulong driveSectors, out ulong diskSize)
        {
            using (var hDevice = Device.OpenRead(DeviceName))
            {
                if (Device.TryGet(hDevice, Device.IOCTL_DISK_GET_DRIVE_GEOMETRY_EX, out Device.DISK_GEOMETRY_EX geometryEx))
                {
                    sectorSize = geometryEx.Geometry.BytesPerSector;
                    driveSectors = geometryEx.DiskSize / sectorSize;
                    diskSize = geometryEx.DiskSize;
                    return;
                }
                if (Device.TryGet(hDevice, Device.IOCTL_DISK_GET_DRIVE_GEOMETRY, out Device.DISK_GEOMETRY geometry))
                {
                    sectorSize = geometry.BytesPerSector;
                    driveSectors = geometry.SectorsPerTrack * geometry.TracksPerCylinder * geometry.Cylinders;
                    diskSize = sectorSize * driveSectors;
                    return;
                }
                throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }

        #region DeviceName

        private readonly Lazy<string> _deviceName;

        public string DeviceName => _deviceName.Value;

        private string GetDeviceName()
        {
            Device.STORAGE_DEVICE_NUMBER storageDeviceNumber;
            using (var hDevice = Device.OpenRead(Volume.DeviceName))
            {
                if (!Device.TryGet(hDevice, Device.IOCTL_STORAGE_GET_DEVICE_NUMBER, out storageDeviceNumber))
                    throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            return $"\\\\.\\PHYSICALDRIVE{storageDeviceNumber.DeviceNumber}";
        }

        #endregion
    }
}
