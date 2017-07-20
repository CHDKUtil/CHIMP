using Chimp.Properties;
using Chimp.Logging.Extensions;
using DesktopToast;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Chimp.Services
{
    sealed class ToastService : IToastService
	{
		private ILoggerFactory LoggerFactory { get; }
		private ILogger Logger { get; }

		public ToastService(ILoggerFactory loggerFactory)
		{
			LoggerFactory = loggerFactory;
			Logger = loggerFactory.CreateLogger<ToastService>();
		}

		public bool IsAvailable
		{
			get
			{
				if (OperatingSystem.OSType != 18 || OperatingSystem.Version.Major < 10)
					return false;

				if (Settings.Default.ToastMaxDuration < 0)
					return false;

				return true;
			}
		}

		public async Task<bool> ShowEjectToastAsync(string displayName)
		{
			if (displayName == null || !IsAvailable)
				return false;

			Logger.LogTrace("Displaying toast");

			try
			{
				var request = CreateRequest(displayName);
				var loggerFactory = new LoggerFactoryAdapter(LoggerFactory);
				await new ToastManager(loggerFactory).ShowAsync(request);
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogError(0, ex, "Toast error");
				return false;
			}
		}

		private ToastRequest CreateRequest(string displayName)
		{
			var filePath = Assembly.GetEntryAssembly().Location;
			var workPath = Path.GetDirectoryName(filePath);
			var dataPath = Path.Combine(workPath, "Assets");
			var logoPath = Path.Combine(dataPath, "app.png");
			var iconPath = Path.Combine(dataPath, "app.ico");
			var toastTitle = Resources.Eject_ToastTitle;
			var toastBody = string.Format(Resources.Eject_ToastBody_Format, displayName);
			var shortcutFileName = string.Format("{0}.lnk", Resources._Title);
            var maximumDuration = TimeSpan.FromMilliseconds(Settings.Default.ToastMaxDuration);
            var appId = "Net.Chdk.Utility.Wpf";

			return new ToastRequest
			{
				ToastTitle = toastTitle,
				ToastBody = toastBody,
				ToastLogoFilePath = logoPath,
				ShortcutFileName = shortcutFileName,
				ShortcutTargetFilePath = filePath,
				ShortcutIconFilePath = iconPath,
				ShortcutWorkingFolder = workPath,
                MaximumDuration = maximumDuration,
                AppId = appId,
            };
		}
	}
}
