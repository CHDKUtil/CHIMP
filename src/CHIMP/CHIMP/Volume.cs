using System;

namespace Chimp
{
    public sealed partial class Volume
    {
        private string DriveLetter { get; }

        public Volume(string driveLetter)
        {
            DriveLetter = driveLetter;

            _disk = new Lazy<Disk>(GetDisk);
            _deviceName = new Lazy<string>(GetDeviceName);
        }

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
    }
}
