using Chimp.Model;
using Chimp.Properties;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.InteropServices;

namespace Chimp.Services
{
    sealed class PartitionService : IPartitionService
    {
        private const uint BytesInMegabyte = 1048576;

        private const ushort PartStart = 0x1be;
        private const ushort PartLength = 0x10;
        private const ushort PartCount = 4;

        private const ushort TypeOffset = 4;
        private const ushort StartOffset = 8;
        private const ushort LengthOffset = 12;
        private const ushort SignatureOffset = 0x1fe;

        private const ushort Signature = 0xaa55;

        private const uint SinglePartStart = 249; //TODO Why not 1?

        private ILogger Logger { get; }
        private IVolumeContainer VolumeContainer { get; }

        public PartitionService(IVolumeContainer volumeContainer, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<PartitionService>();
            VolumeContainer = volumeContainer;
        }

        public void CreateSinglePartition(string driveLetter)
        {
            Logger.LogInformation("Creating single partition on {0}", driveLetter);

            var volume = VolumeContainer.GetVolume(driveLetter);
            var sectorSize = volume.Disk.SectorSize;

            using var hDevice = Device.OpenReadWrite(volume.Disk.DeviceName);
            
            var buffer = new byte[sectorSize];

            var start = SinglePartStart;
            var length = (uint)volume.Disk.DriveSectors - start;
            var type = PartitionType.PrimaryFAT_0;
            SetPartition(buffer, part: 0, type: type, start: start, length: length);

            SetSignature(buffer);

            Device.WriteBlock(hDevice, buffer, sectorSize);
        }

        public void CreatePartitions(string driveLetter)
        {
            Logger.LogInformation("Creating two partitions on {0}", driveLetter);

            var volume = VolumeContainer.GetVolume(driveLetter);
            var sectorSize = volume.Disk.SectorSize;

            using var hDevice = Device.OpenReadWrite(volume.Disk.DeviceName);

            var buffer = new byte[sectorSize];

            var start = 1u;
            var length = (uint)Settings.Default.VolumeSmallSizeMB * BytesInMegabyte / sectorSize;
            var type = PartitionType.PrimaryFAT;
            SetPartition(buffer, part: 0, type: type, start: start, length: length);

            start += length;
            length = (uint)volume.Disk.DriveSectors - start - 1;
            type = PartitionType.PrimaryFAT32;
            SetPartition(buffer, part: 1, type: type, start: start, length: length);

            SetSignature(buffer);

            Device.MoveToStart(hDevice);

            Device.WriteBlock(hDevice, buffer, sectorSize);
        }

        public bool SwitchPartitions(string driveLetter, int part)
        {
            Logger.LogInformation("Switching to partition {0} of {2}", part, driveLetter);

            var volume = VolumeContainer.GetVolume(driveLetter);
            var sectorSize = volume.Disk.SectorSize;

            using var hDevice = Device.OpenReadWrite(volume.Disk.DeviceName);

            var buffer = Device.ReadBlock(hDevice, sectorSize);

            SwitchPartitions(buffer, part);

            Device.MoveToStart(hDevice);

            Device.WriteBlock(hDevice, buffer, sectorSize);

            return true;
        }

        public void UpdateProperties(string driveLetter)
        {
            Logger.LogInformation("Updating properties of {0}", driveLetter);

            var volume = VolumeContainer.GetVolume(driveLetter);

            using var hDevice = Device.OpenReadWrite(volume.DeviceName);

            if (!Device.Invoke(hDevice, Device.IOCTL_VOLUME_UPDATE_PROPERTIES))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        public PartitionType[] GetPartitionTypes(string driveLetter)
        {
            var volume = VolumeContainer.GetVolume(driveLetter);
            var sectorSize = volume.Disk.SectorSize;

            using var hDevice = Device.OpenRead(volume.Disk.DeviceName);

            var buffer = Device.ReadBlock(hDevice, sectorSize);
            return GetPartitionTypes(buffer);
        }

        public bool? TestSwitchedPartitions(PartitionType[] partTypes)
        {
            if (partTypes[0] == PartitionType.PrimaryFAT
                && (partTypes[1] == PartitionType.PrimaryFAT32 || partTypes[1] == PartitionType.PrimaryFAT32_2)
                && partTypes[2] == PartitionType.None)
            {
                return false;
            }
            if ((partTypes[0] == PartitionType.PrimaryFAT32 || partTypes[0] == PartitionType.PrimaryFAT32_2)
                && partTypes[1] == PartitionType.PrimaryFAT
                && partTypes[2] == PartitionType.None)
            {
                return true;
            }
            return null;
        }

        private static PartitionType[] GetPartitionTypes(byte[] buffer)
        {
            var types = new PartitionType[PartCount];
            for (var i = 0; i < PartCount; i++)
            {
                types[i] = (PartitionType)buffer[PartStart + i * PartLength + TypeOffset];
            }
            return types;
        }

        private static void SwitchPartitions(byte[] buffer, int part)
        {
            var bTemp = new byte[PartLength];
            Array.Copy(buffer, PartStart, bTemp, 0, PartLength);
            Array.Copy(buffer, PartStart + part * PartLength, buffer, PartStart, PartLength);
            Array.Copy(bTemp, 0, buffer, PartStart + part * PartLength, PartLength);
        }

        private static void SetPartition(byte[] buffer, ushort part, PartitionType type, uint start, uint length)
        {
            var offset = PartStart + part * PartLength;
            SetValue(buffer, (byte)type, offset + TypeOffset);
            SetValue(buffer, start, offset + StartOffset);
            SetValue(buffer, length, offset + LengthOffset);
        }

        private static void SetSignature(byte[] buffer)
        {
            SetValue(buffer, Signature, SignatureOffset);
        }

        private static void SetValue(byte[] buffer, byte type, int offset)
        {
            buffer[offset] = type;
        }

        private static void SetValue(byte[] buffer, ushort value, int offset)
        {
            buffer[offset] = (byte)value;
            buffer[offset + 1] = (byte)(value >> 8);
        }

        private static void SetValue(byte[] buffer, uint value, int offset)
        {
            buffer[offset] = (byte)value;
            buffer[offset + 1] = (byte)(value >> 8);
            buffer[offset + 2] = (byte)(value >> 16);
            buffer[offset + 3] = (byte)(value >> 24);
        }
    }
}
