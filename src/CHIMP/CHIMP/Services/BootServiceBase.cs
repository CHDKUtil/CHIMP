using Net.Chdk.Model.Card;
using Net.Chdk.Providers.Boot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chimp.Services
{
    abstract class BootServiceBase
    {
        private IVolumeContainer VolumeContainer { get; }
        private IBootProvider BootProvider { get; }

        protected BootServiceBase(IVolumeContainer volumeContainer, IBootProvider bootProvider)
        {
            VolumeContainer = volumeContainer;
            BootProvider = bootProvider;
        }

        protected bool Test(CardInfo cardInfo, string categoryName, string fileSystem)
        {
            var blockSize = GetBlockSize(categoryName, fileSystem);
            var bytes = GetBytes(categoryName, fileSystem);
            if (blockSize == 0 || bytes == null)
                return false;

            var volume = VolumeContainer.GetVolume(cardInfo.DriveLetter);
            using (var hDevice = Device.OpenRead(volume.DeviceName))
            {
                var buffer = Device.ReadBlock(hDevice, blockSize);
                return Test(buffer, bytes);
            }
        }

        protected bool Set(CardInfo cardInfo, string categoryName, string fileSystem, bool value)
        {
            var blockSize = GetBlockSize(categoryName, fileSystem);
            var bytes = GetBytes(categoryName, fileSystem);
            if (blockSize == 0 || bytes == null)
                return false;

            var volume = VolumeContainer.GetVolume(cardInfo.DriveLetter);
            using (var hDevice = Device.OpenReadWrite(volume.DeviceName))
            {
                var buffer = Device.ReadBlock(hDevice, blockSize);
                var oldValue = Test(buffer, bytes);
                if (oldValue != value)
                {
                    Set(buffer, bytes, value);
                    Device.MoveToStart(hDevice);
                    Device.WriteBlock(hDevice, buffer, blockSize);
                }
            }

            return true;
        }

        protected string GetFileName(string categoryName)
        {
            return BootProvider.GetFileName(categoryName);
        }

        protected IDictionary<string, byte[]> GetFiles(string categoryName)
        {
            return BootProvider.GetFiles(categoryName);
        }

        private uint GetBlockSize(string categoryName, string fileSystem)
        {
            return BootProvider.GetBlockSize(categoryName, fileSystem);
        }

        private IDictionary<int, byte[]> GetBytes(string categoryName, string fileSystem)
        {
            return BootProvider.GetBytes(categoryName, fileSystem);
        }

        private static bool Test(byte[] buffer, IDictionary<int, byte[]> bytes)
        {
            return bytes.All(kvp => Test(buffer, kvp.Key, kvp.Value));
        }

        private static bool Test(byte[] buffer, int startIndex, byte[] bytes)
        {
            for (var i = 0; i < bytes.Length; i++)
                if (buffer[i + startIndex] != bytes[i])
                    return false;
            return true;
        }

        private static void Set(byte[] buffer, IDictionary<int, byte[]> bytes, bool value)
        {
            if (value)
                Set(buffer, bytes);
            else
                Clear(buffer, bytes);
            SetChecksum(buffer);
        }

        private static void Set(byte[] buffer, IDictionary<int, byte[]> bytes)
        {
            foreach (var kvp in bytes)
                kvp.Value.CopyTo(buffer, kvp.Key);
        }

        private static void Clear(byte[] buffer, IDictionary<int, byte[]> bytes)
        {
            foreach (var kvp in bytes)
                Array.Clear(buffer, kvp.Key, kvp.Value.Length);
        }

        protected static string GetPath(CardInfo cardInfo, string fileName)
        {
            var rootPath = cardInfo.GetRootPath();
            return Path.Combine(rootPath, fileName);
        }

        private const int SectorSize = 512;
        private const int ExFatVbrSectorCount = 12;
        private const int ExFatVbrSize = SectorSize * ExFatVbrSectorCount;

        private const int ExFatVolumeFlags1 = 106;
        private const int ExFatVolumeFlags2 = 107;
        private const int ExFatPercentInUse = 112;

        private static void SetChecksum(byte[] buffer)
        {
            if (buffer.Length <= SectorSize)
                return;
            SetChecksum(buffer, 0);
            SetChecksum(buffer, ExFatVbrSize);
        }

        private static uint GetChecksum(byte[] buffer, int start)
        {
            var sum = 0u;
            for (var index = 0; index < ExFatVbrSize - SectorSize; index++)
            {
                if (index != ExFatVolumeFlags1
                    && index != ExFatVolumeFlags2
                    && index != ExFatPercentInUse)
                {
                    sum = ((sum << 31) | (sum >> 1)) + buffer[start + index];
                }
            }
            return sum;
        }

        private static void SetChecksum(byte[] buffer, int start)
        {
            var sum = GetChecksum(buffer, start);
            var bSum = BitConverter.GetBytes(sum);
            for (var index = 0; index < SectorSize; index += 4)
            {
                Array.Copy(bSum, 0, buffer, start + index + ExFatVbrSize - SectorSize, 4);
            }
        }
    }
}
