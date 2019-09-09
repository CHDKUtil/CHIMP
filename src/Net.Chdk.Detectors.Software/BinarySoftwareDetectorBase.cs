using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Chdk.Encoders.Binary;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Camera;
using Net.Chdk.Providers.Software;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    abstract class BinarySoftwareDetectorBase : IInnerBinarySoftwareDetector
    {
        protected SoftwareDetectorSettings Settings { get; }
        protected ILogger Logger { get; }
        protected IBinaryDecoder BinaryDecoder { get; }
        protected IBootProvider BootProvider { get; }
        protected ISoftwareHashProvider HashProvider { get; }

        protected string HashName => Settings.HashName;
        protected bool ShuffleOffsets => Settings.ShuffleOffsets;
        protected int MaxThreads => Settings.MaxThreads;

        private IEnumerable<IProductBinarySoftwareDetector> SoftwareDetectors { get; }
        private ICameraProvider CameraProvider { get; }

        protected BinarySoftwareDetectorBase(IEnumerable<IProductBinarySoftwareDetector> softwareDetectors, IBinaryDecoder binaryDecoder, IBootProvider bootProvider, ICameraProvider cameraProvider,
            ISoftwareHashProvider hashProvider, IOptions<SoftwareDetectorSettings> settings, ILogger logger)
        {
            Settings = settings.Value;
            Logger = logger;
            SoftwareDetectors = softwareDetectors;
            BinaryDecoder = binaryDecoder;
            BootProvider = bootProvider;
            CameraProvider = cameraProvider;
            HashProvider = hashProvider;
        }

        public SoftwareInfo GetSoftware(string basePath, string categoryName, IProgress<double> progress, CancellationToken token)
        {
            if (!CategoryName.Equals(categoryName, StringComparison.Ordinal))
                return null;

            var fileName = BootProvider.GetFileName(CategoryName);
            var diskbootPath = Path.Combine(basePath, fileName);

            Logger.LogTrace("Detecting software from {0}", diskbootPath);

            if (!File.Exists(diskbootPath))
            {
                Logger.LogTrace("{0} not found", diskbootPath);
                return null;
            }

            var inBuffer = File.ReadAllBytes(diskbootPath);
            var software = GetSoftware(inBuffer, progress, token);
            if (software != null)
            {
                if (software.Product.Created == null)
                    software.Product.Created = File.GetCreationTimeUtc(diskbootPath);
            }
            return software;
        }

        public virtual SoftwareInfo GetSoftware(byte[] inBuffer, IProgress<double> progress, CancellationToken token)
        {
            var detectors = GetDetectors();
            var prefix = BootProvider.GetPrefix(CategoryName);
            var software = GetSoftware(detectors, prefix, inBuffer, progress, token);
            if (software != null)
            {
                var fileName = BootProvider.GetFileName(CategoryName);
                software.Hash = HashProvider.GetHash(inBuffer, fileName, HashName);
            }
            return software;
        }

        public virtual bool UpdateSoftware(SoftwareInfo software, byte[] inBuffer)
        {
            if (!CategoryName.Equals(software.Category.Name, StringComparison.Ordinal))
                return false;

            var detectors = GetDetectors(software.Product);
            var encoding = GetEncoding(software.Product, software.Camera, software.Encoding);

            var fileName = BootProvider.GetFileName(CategoryName);
            software.Hash = HashProvider.GetHash(inBuffer, fileName, HashName);

            var software2 = GetSoftware(detectors, inBuffer, encoding);
            if (software2 != null)
            {
                if (software2.Product.Created != null)
                    software.Product.Created = software2.Product.Created;
                if (software2.Build.Changeset != null)
                    software.Build.Changeset = software2.Build.Changeset;
                if (software2.Build.Creator != null)
                    software.Build.Creator = software2.Build.Creator;
                if (software2.Compiler != null)
                    software.Compiler = software2.Compiler;
                if (software.Encoding == null)
                    software.Encoding = software2.Encoding;
                return true;
            }

            return false;
        }

        private SoftwareInfo GetSoftware(IEnumerable<IProductBinarySoftwareDetector> detectors, byte[] inBuffer, SoftwareEncodingInfo encoding,
            IProgress<double> progress = null, CancellationToken token = default(CancellationToken))
        {
            var prefix = BootProvider.GetPrefix(CategoryName);
            if (encoding == null)
                return GetSoftware(detectors, prefix, inBuffer, progress, token);
            return DoGetSoftware(detectors, prefix, inBuffer, encoding, token);
        }

        private SoftwareInfo GetSoftware(IEnumerable<IProductBinarySoftwareDetector> detectors, byte[] prefix, byte[] inBuffer, IProgress<double> progress, CancellationToken token)
        {
            if (!BinaryDecoder.ValidatePrefix(inBuffer, inBuffer.Length, prefix))
                return PlainGetSoftware(detectors, prefix, inBuffer, token);
            return DoGetSoftware(detectors, prefix, inBuffer, progress, token);
        }

        protected SoftwareInfo PlainGetSoftware(IEnumerable<IProductBinarySoftwareDetector> detectors, byte[] prefix, byte[] inBuffer, CancellationToken token)
        {
            var worker = new BinarySoftwareDetectorWorker(detectors, BinaryDecoder, prefix, inBuffer, new SoftwareEncodingInfo());
            return worker.GetSoftware(new ProgressState(), token);
        }

        private SoftwareInfo DoGetSoftware(IEnumerable<IProductBinarySoftwareDetector> detectors, byte[] prefix, byte[] inBuffer, SoftwareEncodingInfo encoding, CancellationToken token)
        {
            var worker = new BinarySoftwareDetectorWorker(detectors, BinaryDecoder, prefix, inBuffer, encoding);
            return worker.GetSoftware(new ProgressState(), token);
        }

        protected virtual SoftwareInfo DoGetSoftware(IEnumerable<IProductBinarySoftwareDetector> detectors, byte[] prefix, byte[] inBuffer, IProgress<double> progress, CancellationToken token)
        {
            var processorCount = Environment.ProcessorCount;
            var count = MaxThreads > 0 && MaxThreads < processorCount
                ? MaxThreads : processorCount;

            var offsets = GetOffsets();

            var watch = new Stopwatch();
            watch.Start();

            var workers = new BinarySoftwareDetectorWorker[count];
            for (var i = 0; i < count; i++)
            {
                workers[i] = new BinarySoftwareDetectorWorker(detectors, BinaryDecoder, prefix, inBuffer,
                    i * offsets.Length / count, (i + 1) * offsets.Length / count, offsets);
            }

            var progressState = new ProgressState(offsets.Length, progress);
            var software = GetSoftware(workers, offsets.Length, progressState, token);

            watch.Stop();
            Logger.LogDebug("Detecting software completed in {0}", watch.Elapsed);

            progressState.Reset();

            return software;
        }

        private SoftwareInfo GetSoftware(BinarySoftwareDetectorWorker[] workers, int offsetCount, ProgressState progress, CancellationToken token)
        {
            var workerCount = workers.Length;
            if (workerCount == 1)
            {
                Logger.LogDebug("Detecting software in a single thread from {0} offsets", offsetCount);
                return workers[0].GetSoftware(progress, token);
            }

            Logger.LogDebug("Detecting software in {0} threads from {1} offsets", workerCount, offsetCount);

            var threads = new Thread[workerCount];
            var results = new SoftwareInfo[workerCount];
            for (var j = 0; j < threads.Length; j++)
            {
                var i = j;
                threads[i] = new Thread(() => results[i] = workers[i].GetSoftware(progress, token));
            }

            foreach (var thread in threads)
                thread.Start();

            foreach (var thread in threads)
                thread.Join();

            return results
                .FirstOrDefault(s => s != null);
        }

        private SoftwareEncodingInfo GetEncoding(SoftwareProductInfo product, SoftwareCameraInfo camera, SoftwareEncodingInfo encoding)
        {
            return encoding
                ?? CameraProvider.GetEncoding(product, camera);
        }

        private IEnumerable<IProductBinarySoftwareDetector> GetDetectors()
        {
            return SoftwareDetectors
                .Where(d => d.CategoryName.Equals(CategoryName, StringComparison.Ordinal));
        }

        private IEnumerable<IProductBinarySoftwareDetector> GetDetectors(SoftwareProductInfo product)
        {
            var productName = product?.Name;
            return productName == null
                ? GetDetectors()
                : SoftwareDetectors
                    .Where(d => d.ProductName.Equals(productName, StringComparison.Ordinal));
        }

        protected abstract uint?[] GetOffsets();

        protected abstract string CategoryName { get; }
    }
}
