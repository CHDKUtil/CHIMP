using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Card;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Chimp.Services
{
    sealed class FormatService : IFormatService
    {
        private const ulong MaxFat32CardSize = (ulong)32 * 1024 * 1024 * 1024;

        private ILogger Logger { get; }

        public FormatService(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<FormatService>();
        }

        public bool Format(CardInfo card, string fileSystem, string label)
        {
            var isCardFat32Formattable = card.Capacity <= MaxFat32CardSize;
            return isCardFat32Formattable || !"FAT32".Equals(fileSystem)
                ? Format(card.DriveLetter, fileSystem, label)
                : FormatFat32(card.DriveLetter, label);
        }

        private bool Format(string driveLetter, string fileSystem, string label)
        {
            Logger.LogTrace("Format");

            // The label is not passed via /v: since format.com does not strip quotes,
            // so a label containing spaces (e.g. "CANON 105") cannot be passed reliably.
            // An invalid label makes format.com prompt for a new one and wait for input.
            var sb = new StringBuilder();
            sb.Append(driveLetter);
            sb.Append($" /fs:{fileSystem}");
            sb.Append(" /y /q");
            var args = sb.ToString();

            if (!Run(Environment.SystemDirectory, "format.com", args))
                return false;

            SetLabel(driveLetter, label);
            return true;
        }

        private void SetLabel(string driveLetter, string label)
        {
            if (string.IsNullOrEmpty(label))
                return;

            Logger.LogInformation("Setting label of {0} to {1}", driveLetter, label);
            try
            {
                new DriveInfo(driveLetter).VolumeLabel = label;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException)
            {
                Logger.LogWarning(0, ex, "Cannot set label of {0}", driveLetter);
            }
        }

        private bool FormatFat32(string driveLetter, string label)
        {
            Logger.LogTrace("FormatFat32");

            var sb = new StringBuilder();
            sb.Append("/F");
            sb.Append($" /v{label}");
            sb.Append($" {driveLetter}");
            var args = sb.ToString();

            return Run(Environment.CurrentDirectory, "fat32format.exe", args);
        }

        private bool Run(string path, string fileName, string args)
        {
            var filePath = Path.Combine(path, fileName);
            Logger.LogInformation("Run {0} {1}", filePath, args);

            using (var process = new Process())
            {
                process.StartInfo.FileName = filePath;
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                return Run(process);
            }
        }

        private bool Run(Process process)
        {
            var fileName = Path.GetFileName(process.StartInfo.FileName);

            if (!process.Start())
            {
                Logger.LogError("Start failed");
                return false;
            }

            // Never wait for user input (the window is hidden).
            process.StandardInput.Close();

            string line;
            while ((line = process.StandardOutput.ReadLine()) != null)
                Logger.LogInformation($"{fileName}: {{1}}", line);

            while ((line = process.StandardError.ReadLine()) != null)
                Logger.LogError($"{fileName}: {{1}}", line);

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                Logger.LogInformation("Exit code: {0}", process.ExitCode);
                return false;
            }

            return true;
        }
    }
}
