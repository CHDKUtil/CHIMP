using Microsoft.Extensions.Logging;
using Net.Chdk.Json;
using Net.Chdk.Model.Card;
using Net.Chdk.Model.Category;
using Net.Chdk.Validators;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;

namespace Net.Chdk.Detectors
{
    public abstract class MetadataDetector<TDetector, TValue>
        where TDetector : MetadataDetector<TDetector, TValue>
        where TValue : class
    {
        protected ILogger Logger { get; }

        private IValidator<TValue> Validator { get; }

        protected MetadataDetector(IValidator<TValue> validator, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<TDetector>();
            Validator = validator;
        }

        protected abstract string FileName { get; }

        [Obsolete]
        protected TValue GetValue(CardInfo cardInfo, IProgress<double> progress, CancellationToken token)
        {
            var basePath = cardInfo.GetRootPath();
            var filePath = Path.Combine(basePath, Directories.Metadata, FileName);
            return GetValue(basePath, filePath, progress, token);
        }

        protected TValue GetValue(string basePath, CategoryInfo category, IProgress<double> progress, CancellationToken token)
        {
            var filePath = Path.Combine(basePath, Directories.Metadata, category.Name, FileName);
            return GetValue(basePath, filePath, progress, token);
        }

        protected TValue GetValue(string basePath, string filePath, IProgress<double> progress, CancellationToken token)
        {
            if (!File.Exists(filePath))
            {
                Logger.LogTrace("{0} not found", filePath);
                return null;
            }

            var value = ReadValue(filePath);
            if (value == null)
                return null;

            if (basePath == null)
            {
                Logger.LogTrace("Skipping {0} validation", filePath);
                return value;
            }

            if (!TryValidate(value, basePath, progress, token))
                return null;

            return value;
        }

        private TValue ReadValue(string filePath)
        {
            Logger.LogInformation("Reading {0}", filePath);

            using (var stream = File.OpenRead(filePath))
            {
                try
                {
                    return JsonObject.Deserialize<TValue>(stream);
                }
                catch (JsonException ex)
                {
                    Logger.LogError(0, ex, "Deserialization error");
                    return null;
                }
            }
        }

        private bool TryValidate(TValue value, string basePath, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Validating {0}", typeof(TValue).Name);

            try
            {
                Validator.Validate(value, basePath, progress, token);
                return true;
            }
            catch (ValidationException ex)
            {
                Logger.LogError(0, ex, "Validation error");
                return false;
            }
        }
    }
}
