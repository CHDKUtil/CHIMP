using System;

namespace Chimp
{
    public sealed class Volume
    {
        #region Fields

        private string DriveLetter { get; }

        #endregion

        #region Constructor

        public Volume(string driveLetter)
        {
            DriveLetter = driveLetter;

            _disk = new Lazy<Disk>(GetDisk);
            _deviceName = new Lazy<string>(GetDeviceName);
        }

        #endregion

        #region Disk

        private readonly Lazy<Disk> _disk;

        public Disk Disk => _disk.Value;

        private Disk GetDisk()
        {
            return new Disk(this);
        }

        #endregion

        #region DeviceName

        private readonly Lazy<string> _deviceName;

        public string DeviceName => _deviceName.Value;

        private string GetDeviceName()
        {
            return $"\\\\.\\{DriveLetter}";
        }

        #endregion

        #region IsHotplugDevice

        public bool IsHotplugDevice()
        {
            using (var hDevice = Device.OpenRead(DeviceName))
            {
                Device.STORAGE_HOTPLUG_INFO hotplugInfo;
                return Device.TryGet(hDevice, Device.IOCTL_STORAGE_GET_HOTPLUG_INFO, out hotplugInfo)
                    ? hotplugInfo.DeviceHotplug
                    : false;
            }
        }

        #endregion
    }
}
