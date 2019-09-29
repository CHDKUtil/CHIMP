using Chimp.Containers;
using Chimp.Logging.Extensions;
using Chimp.Model;
using Chimp.Providers;
using Chimp.Services;
using Chimp.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Net.Chdk;
using Net.Chdk.Detectors.Camera;
using Net.Chdk.Detectors.CameraModel;
using Net.Chdk.Detectors.Card;
using Net.Chdk.Detectors.Software;
using Net.Chdk.Detectors.Software.Chdk;
using Net.Chdk.Detectors.Software.Fhp;
using Net.Chdk.Detectors.Software.Ml;
using Net.Chdk.Detectors.Software.Sdm;
using Net.Chdk.Encoders.Binary;
using Net.Chdk.Generators.Script;
using Net.Chdk.Generators.Platform;
using Net.Chdk.Generators.Platform.Eos;
using Net.Chdk.Generators.Platform.Ps;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Camera;
using Net.Chdk.Providers.Category;
using Net.Chdk.Providers.Crypto;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using Net.Chdk.Providers.Software.Chdk;
using Net.Chdk.Providers.Software.Ml;
using Net.Chdk.Providers.Software.Sdm;
using Net.Chdk.Providers.Substitute;
using Net.Chdk.Validators.Software;
using Net.Chdk.Watchers.Volume;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Chimp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private ILogger Logger { get; }
        private IServiceProvider ServiceProvider { get; }
        private ILoggerFactory LoggerFactory { get; }
        private MainViewModel ViewModel { get; }
        private IDialogService DialogService { get; }

        public App()
        {
            var serviceCollection = new ServiceCollection()
                .AddLogging()
                .AddChimpLogging()
                .AddSingleton<MainViewModel>()
                .AddSingleton<MainWindow>()
                .AddSingleton<SynchronizationContext>(new DispatcherSynchronizationContext(Dispatcher));

            ConfigureValidators(serviceCollection);
            ConfigureDetectors(serviceCollection);
            ConfigureGenerators(serviceCollection);
            ConfigureProviders(serviceCollection);
            ConfigureContainers(serviceCollection);
            ConfigureServices(serviceCollection);
            ConfigureOptions(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            LoggerFactory = ServiceProvider.GetService<ILoggerFactory>();
            var args = Environment.GetCommandLineArgs();

            var logLevel = LogLevel.Information;
            var args1 = args.Skip(1);
            if (args1.Contains("/verbose"))
                logLevel = LogLevel.Trace;
            else if (args1.Contains("/debug"))
                logLevel = LogLevel.Debug;

            var chdkTempPath = Path.Combine(Path.GetTempPath(), "CHIMP");
            var filePath = Path.Combine(chdkTempPath, "CHIMP.log");
            LoggerFactory.AddFile(filePath, logLevel, retainedFileCountLimit: 8);

            Logger = LoggerFactory.CreateLogger<App>();

            var assembly = Assembly.GetExecutingAssembly();
            Logger.LogInformation(assembly.FullName);
            var config = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
            Logger.LogInformation("Configuration: {0}", config.Configuration);

            Logger.LogObject(LogLevel.Information, args);
            Logger.LogInformation(OperatingSystem.DisplayName);
            Logger.LogInformation("64-bit: {0}", Environment.Is64BitProcess);
            Logger.LogInformation("Version: {0}", OperatingSystem.Version);
            Logger.LogInformation("Processors: {0}", Environment.ProcessorCount);

            ViewModel = ServiceProvider.GetService<MainViewModel>();
            DialogService = ServiceProvider.GetService<IDialogService>();

            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private static void ConfigureValidators(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSoftwareHashValidator()
                .AddSoftwareValidator()
                .AddModulesValidator();
        }

        private static void ConfigureDetectors(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddCardDetector()

                .AddSoftwareDetector()
                .AddMetadataSoftwareDetector()
                .AddBinarySoftwareDetector()
                .AddFileSystemSoftwareDetector()
                .AddEosHashSoftwareDetector()
                .AddPsHashSoftwareDetector()
                .AddScriptHashSoftwareDetector()
                .AddEosBinarySoftwareDetector()
                .AddKnownPsBinarySoftwareDetector()
                .AddUnkownPsBinarySoftwareDetector()

                .AddChdkSoftwareDetector()
                .AddChdkProductDetector()
                .AddSdmSoftwareDetector()
                .AddSdmProductDetector()
                .AddNightlyMlSoftwareDetector()
                .AddBetaMlSoftwareDetector()
                .AddMlProductDetector()
                .AddFhpSoftwareDetector()
                .AddFhpProductDetector()

                .AddModulesDetector()
                .AddMetadataModulesDetector()
                .AddFileSystemModulesDetector()
                .AddBinaryModuleDetector()
                .AddDerivedModuleDetector()
                .AddMlModuleDetector()

                .AddCameraDetector()
                .AddFileSystemCameraDetector()
                .AddAllFileSystemCameraDetector()
                .AddFileCameraDetector()

                .AddCameraModelDetector()
                .AddFileSystemCameraModelDetector()
                .AddFileCameraModelDetector()

                .AddVolumeWatcher()
                .AddBinaryEncoder()
                .AddBinaryDecoder();
        }

        private static void ConfigureGenerators(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddPlatformGenerator()
                .AddEosPlatformGenerator()
                .AddPsPlatformGenerator()
                .AddScriptGenerator();
        }

        private static void ConfigureProviders(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddCategoryProvider()
                .AddProductProvider()
                .AddBootProvider()
                .AddModuleProvider()
                .AddHashProvider()
                .AddSourceProvider()
                .AddChdkSourceProvider()
                .AddSdmSourceProvider()
                .AddMlSourceProvider()
                .AddSoftwareHashProvider()
                .AddFirmwareProvider()
                .AddCameraProvider()
                .AddSubstituteProvider()
                .AddSingleton<IResourceProvider, ChdkResourceProvider>()
                .AddSingleton<IStepProvider, StepProvider>()
                .AddSingleton<IActionProvider, ActionContainer>()
                .AddSingleton<ILicenseProvider, LicenseProvider>()
                .AddSingleton<IDownloaderProvider, AggregateDownloaderProvider>()
                .AddSingleton<IInstallerProvider, InstallerProvider>()
                .AddSingleton<ITipProvider, AggregateTipProvider>()
                ;
        }

        private static void ConfigureContainers(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IServiceActivator, ServiceActivator>()
                .AddSingleton<IPageContainer, PageContainer>()
                .AddSingleton<IControllerContainer, ControllerContainer>()
                .AddSingleton<IVolumeContainer, VolumeContainer>();
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IDialogService, DialogService>()
                .AddSingleton<IDownloadService, DownloadService>()
                .AddSingleton<IExtractService, ExtractService>()
                .AddSingleton<IMetadataService, MetadataService>()
                .AddSingleton<IInstallService, InstallService>()
                .AddSingleton<IPartitionService, PartitionService>()
                .AddSingleton<IFormatService, FormatService>()
                .AddSingleton<IBootService, BootService>()
                .AddSingleton<IScriptService, ScriptService>()
                .AddSingleton<IEjectService, EjectService>()
                .AddSingleton<IToastService, ToastService>();
        }

        private static void ConfigureOptions(IServiceCollection serviceCollection)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), Directories.Data);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            serviceCollection
                .AddOptions()
                .Configure<SoftwareDetectorSettings>(configuration.GetSection("softwareDetector"))
                .Configure<StepsData>(configuration)
                .Configure<ActionsData>(configuration)
                .Configure<LicensesData>(configuration)
                .Configure<InstallersData>(configuration);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ParseCommandLineArgs();

            Logger.LogInformation("Culture: {0}", Dispatcher.Thread.CurrentCulture);
            Logger.LogInformation("UI Culture: {0}", Dispatcher.Thread.CurrentUICulture);

            //Localize bindings
            FrameworkElement.LanguageProperty.OverrideMetadata(
                  typeof(FrameworkElement),
                  new FrameworkPropertyMetadata(
                      XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag)));

            MainWindow = ServiceProvider.GetService<MainWindow>();
            MainWindow.Show();
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.LogCritical(e.Exception.ToString());

            if (e.Exception is AggregateException aggregateException)
            {
                foreach (var exception in aggregateException.InnerExceptions)
                {
                    DialogService.ShowErrorMessage(exception.Message);
                }
            }
            else
            {
                DialogService.ShowErrorMessage(e.Exception.Message);
            }

            if (ViewModel != null)
                ViewModel.IsAborted = true;

            e.Handled = true;
            Shutdown(e.Exception.HResult);
        }

        private void ParseCommandLineArgs()
        {
            var args = Environment.GetCommandLineArgs();
            for (int i = 1; i < args.Length; i++)
            {
                var arg = args[i];
                switch (arg.ToLowerInvariant())
                {
                    case "/lang":
                        if (i == args.Length - 1)
                            throw new ApplicationException("Language argument is missing");
                        Dispatcher.Thread.CurrentUICulture = CultureInfo.GetCultureInfo(args[++i]);
                        break;
                    case "/verbose":
                        break;
                    case "/debug":
                        break;
                    default:
                        throw new ApplicationException($"Invalid argument: {args[i]}");
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Logger.LogInformation("Exit code: {0}", e.ApplicationExitCode);
            Logger.LogInformation("\r\n");
        }
    }
}
