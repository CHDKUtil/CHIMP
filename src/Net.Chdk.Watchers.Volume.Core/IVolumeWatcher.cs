using System;

namespace Net.Chdk.Watchers.Volume
{
    public interface IVolumeWatcher : IDisposable
    {
        void Initialize();
        void Start();
        void Stop();
        event EventHandler<string> VolumeAdded;
        event EventHandler<string> VolumeRemoved;
    }
}
