using Net.Chdk.Watchers.Volume;
using System.Collections.Concurrent;

namespace Chimp.Containers
{
    sealed class VolumeContainer : IVolumeContainer
    {
        private readonly ConcurrentDictionary<string, Volume> volumes = new ConcurrentDictionary<string, Volume>();

        private IVolumeWatcher VolumeWatcher { get; }

        public VolumeContainer(IVolumeWatcher volumeWatcher)
        {
            VolumeWatcher = volumeWatcher;
            VolumeWatcher.VolumeRemoved += VolumeWatcher_VolumeRemoved;
        }

        public Volume GetVolume(string driveLetter)
        {
            return volumes.GetOrAdd(driveLetter, CreateVolume);
        }

        private Volume CreateVolume(string driveLetter)
        {
            return new Volume(driveLetter);
        }

        private void VolumeWatcher_VolumeRemoved(object sender, string driveLetter)
        {
            volumes.TryRemove(driveLetter, out Volume _);
        }
    }
}
