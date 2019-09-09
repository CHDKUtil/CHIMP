using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;    
using System.IO;
using System.Linq;
using System.Threading;

namespace Net.Chdk.Validators.Software
{
    sealed class ModulesValidator : SoftwareValidator<ModulesInfo>
    {
        private IModuleProvider ModuleProvider { get; }

        public ModulesValidator(IModuleProvider moduleProvider, IValidator<SoftwareHashInfo> hashValidator)
            : base(hashValidator)
        {
            ModuleProvider = moduleProvider;
        }

        protected override void DoValidate(ModulesInfo modules, string basePath, IProgress<double> progress, CancellationToken token)
        {
            Validate(modules.Version);
            Validate(modules.Product);
            Validate(modules.Product.Name, modules.Modules, basePath, progress, token);
        }

        private static void Validate(ModulesProductInfo product)
        {
            if (product == null)
                throw new ValidationException("Null product");

            if (string.IsNullOrEmpty(product.Name))
                throw new ValidationException("Missing product name");
        }

        private void Validate(string productName, IDictionary<string, ModuleInfo> modules, string basePath, IProgress<double> progress, CancellationToken token)
        {
            if (modules == null)
                ThrowValidationException("Null modules");

            token.ThrowIfCancellationRequested();

            var count = modules
                .SelectMany(kvp => kvp.Value.Hash.Values)
                .Count();
            var index = 0;
            var values = new Dictionary<string, string>();
            foreach (var kvp in modules)
            {
                Validate(kvp.Key, kvp.Value, basePath, progress, token);
                foreach (var kvp2 in kvp.Value.Hash.Values)
                {
                    values.Add(kvp2.Key, kvp2.Value);
                    if (progress != null)
                        progress.Report((double)(++index) / count);
                }
            }

            Validate(productName, values, basePath);
        }

        private void Validate(string name, ModuleInfo module, string basePath, IProgress<double> progress, CancellationToken token)
        {
            if (string.IsNullOrEmpty(name))
                ThrowValidationException("Missing module name");

            Func<string> formatter = () => string.Format("module {0}", name);

            if (module == null)
                ThrowValidationException("Null {0}", formatter);

            ValidateCreated(module.Created, formatter);
            ValidateChangeset(module.Changeset, formatter);

            HashValidator.Validate(module.Hash, basePath, progress, token);
        }

        private void Validate(string productName, Dictionary<string, string> values, string basePath)
        {
            var modulesPath = ModuleProvider.GetPath(productName);
            var path = Path.Combine(basePath, modulesPath);
            if (!Directory.Exists(path))
                ThrowValidationException("Missing {0}", path);

            var extension = ModuleProvider.GetExtension(productName);
            var pattern = string.Format("*{0}", extension);
            var files = Directory.EnumerateFiles(path, pattern);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var filePath = Path.Combine(modulesPath, fileName).ToLowerInvariant();
                if (!values.ContainsKey(filePath))
                    ThrowValidationException("Missing {0}", fileName);
            }
        }
    }
}
