using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Card;
using System;
using System.Linq;
using System.Management;

namespace Net.Chdk.Detectors.Card
{
    public sealed class CardDetector : ICardDetector
    {
        private const string CardsQueryString = "SELECT * FROM Win32_Volume WHERE DriveType = 2";

        private static readonly string[] SafeFileSystems = new[] { null, "FAT", "FAT32", "exFAT" };

        private ILoggerFactory LoggerFactory { get; }
        private ILogger<CardDetector> Logger { get; }

        public CardDetector(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger<CardDetector>();
        }

        public CardInfo[] GetCards()
        {
            Logger.LogTrace("Detecting cards");

            using (var searcher = new ManagementObjectSearcher(CardsQueryString))
            using (var volumes = searcher.Get())
            {
                return volumes
                    .Cast<ManagementObject>()
                    .Where(IsSafe)
                    .Select(GetCard)
                    .ToArray();
            }
        }

        public CardInfo GetCard(string driveLetter)
        {
            Logger.LogTrace("Detecting card {0}", driveLetter);

            using (var searcher = new ManagementObjectSearcher($"{CardsQueryString} AND DriveLetter = '{driveLetter}'"))
            using (var volumes = searcher.Get())
            {
                return volumes
                    .Cast<ManagementObject>()
                    .Select(GetCard)
                    .SingleOrDefault();
            }
        }

        private CardInfo GetCard(ManagementObject volume)
        {
            return new CardInfo
            {
                DeviceId = (string)volume["DeviceID"],
                DriveLetter = (string)volume["DriveLetter"],
                Label = (string)volume["Label"],
                FileSystem = (string)volume["FileSystem"],
                Capacity = (ulong?)volume["Capacity"],
                FreeSpace = (ulong?)volume["FreeSpace"],
            };
        }

        private static bool IsSafe(ManagementObject volume)
        {
            var fileSystem = (string)volume["FileSystem"];
            return SafeFileSystems.Contains(fileSystem, StringComparer.InvariantCulture);
        }
    }
}
