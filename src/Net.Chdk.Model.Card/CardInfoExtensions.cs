using System;

namespace Net.Chdk.Model.Card
{
    public static class CardInfoExtensions
    {
        public static string GetRootPath(this CardInfo cardInfo)
        {
            if (cardInfo == null)
                throw new ArgumentNullException(nameof(cardInfo));
            if (string.IsNullOrEmpty(cardInfo.DriveLetter))
                throw new ArgumentException("Invalid drive letter", nameof(cardInfo));
            return $"{cardInfo.DriveLetter}\\";
        }
    }
}
