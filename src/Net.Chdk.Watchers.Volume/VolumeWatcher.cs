using System;
using System.Management;

namespace Net.Chdk.Watchers.Volume
{
    public sealed class VolumeWatcher : IVolumeWatcher
    {
        private ManagementEventWatcher Watcher { get; set; }

        public void Initialize()
        {
            if (Watcher == null)
            {
                Watcher = new ManagementEventWatcher("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 OR EventType = 3");
                Watcher.EventArrived += Watcher_EventArrived;
            }
        }

        public void Dispose()
        {
            if (Watcher != null)
            {
                Watcher.EventArrived -= Watcher_EventArrived;
                Watcher.Dispose();
                Watcher = null;
            }
        }

        public void Start()
        {
            Watcher.Start();
        }

        public void Stop()
        {
            Watcher.Stop();
        }

        /// <summary>
        /// Raised when a volume is added.
        /// </summary>
        /// <value>Drive letter.</value>
        public event EventHandler<string> VolumeAdded;

        /// <summary>
        /// Raised when a volume is removed.
        /// </summary>
        /// <value>Drive letter.</value>
        public event EventHandler<string> VolumeRemoved;

        private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            var eventType = (ushort)e.NewEvent["EventType"];
            var driveLetter = (string)e.NewEvent["DriveName"];
            switch (eventType)
            {
                case 2:
                    VolumeAdded?.Invoke(this, driveLetter);
                    break;
                case 3:
                    VolumeRemoved?.Invoke(this, driveLetter);
                    break;
                default:
                    break;
            }
        }
    }
}
