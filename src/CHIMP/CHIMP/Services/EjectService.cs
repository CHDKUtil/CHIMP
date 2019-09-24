using Chimp.Properties;
using Chimp.Logging.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;
using System;
using System.Linq;
using System.Threading;

namespace Chimp.Services
{
    sealed class EjectService : IEjectService
	{
		private ILogger Logger { get; }
		private IVolumeContainer VolumeContainer { get; }
		private ILoggerFactory LoggerFactory { get; }

		public EjectService(IVolumeContainer volumeContainer, ILoggerFactory loggerFactory)
		{
			Logger = loggerFactory.CreateLogger<EjectService>();
			VolumeContainer = volumeContainer;
			LoggerFactory = loggerFactory;
		}

		public bool Eject(string driveLetter)
		{
			Logger.LogInformation("Ejecting {0}", driveLetter);

			var volume = VolumeContainer.GetVolume(driveLetter);

			Thread.Sleep(Settings.Default.VolumeEjectDelay);

			using (var hDevice = Device.OpenRead(volume.Disk.DeviceName))
			{
				if (!Lock(hDevice) || !Dismount(hDevice))
					return false;
			}

			Thread.Sleep(Settings.Default.VolumeEjectDelay);

            try
            {
                return DoEject(driveLetter);
            }
            catch (Exception ex)
            {
                Logger.LogError(0, ex, "Eject failed");
                return false;
            }
		}

		private bool DoEject(string driveLetter)
		{
			var loggerFactory = new LoggerFactoryAdapter(LoggerFactory);
			using (var volumeDeviceClass = new UsbEject.VolumeDeviceClass(loggerFactory))
			{
				var volume = volumeDeviceClass.SingleOrDefault(v => driveLetter.Equals(v.LogicalDrive));
				if (volume == null)
				{
					Logger.LogError("Volume not found");
					return false;
				}

				var allowUI = true;
				var result = volume.Eject(allowUI);
				if (result != null)
				{
					Logger.LogError("Eject result: {0}", result);
					return false;
				}
			}

			return true;
		}

		private bool Lock(SafeFileHandle hDevice)
		{
			var retryCount = Settings.Default.VolumeLockRetryCount;
			var delay = Settings.Default.VolumeLockDelay;
			if (retryCount <= 0 || delay <= 0)
				return true;
			for (int i = 0; i < retryCount; i++)
			{
				if (Device.Invoke(hDevice, Device.FSCTL_LOCK_VOLUME))
				{
					Logger.LogTrace("Lock succeeded after {0} retries", i);
					return true;
				}
				Thread.Sleep(delay);
			}
			Logger.LogError("Lock failed");
			return false;
		}

		private bool Dismount(SafeFileHandle hDevice)
		{
			var retryCount = Settings.Default.VolumeLockRetryCount;
			var delay = Settings.Default.VolumeLockDelay;
			if (retryCount <= 0 || delay <= 0)
				return true;
			if (Device.Invoke(hDevice, Device.FSCTL_DISMOUNT_VOLUME))
				return true;
			Logger.LogError("Dismount failed");
			return false;
		}

        //private bool Online(SafeFileHandle hDevice)
        //{
        //	if (Device.Invoke(hDevice, Device.IOCTL_VOLUME_ONLINE))
        //		return true;
        //	Logger.LogError("Online failed");
        //	return false;
        //}

        //private bool Offline(SafeFileHandle hDevice)
        //{
        //    if (Device.Invoke(hDevice, Device.IOCTL_VOLUME_OFFLINE))
        //        return true;
        //    Logger.LogError("Offline failed");
        //    return false;
        //}

        //private bool PreventRemoval(SafeFileHandle hDevice, bool prevent)
        //{
        //	var value = prevent ? (byte)1 : (byte)0;
        //	if (Device.Set(hDevice, Device.IOCTL_STORAGE_MEDIA_REMOVAL, value))
        //		return true;
        //	Logger.LogError("PreventRemoval failed");
        //	return false;
        //}
    }
}
