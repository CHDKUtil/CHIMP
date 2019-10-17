using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Boot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace Net.Chdk.Validators.Software
{
    abstract class SoftwareValidator<T> : Validator<T>
    {
        protected IValidator<SoftwareHashInfo> HashValidator { get; }

        protected SoftwareValidator(IValidator<SoftwareHashInfo> hashValidator)
        {
            HashValidator = hashValidator;
        }
    }

    sealed class SoftwareValidator : SoftwareValidator<SoftwareInfo>
    {
        private IBootProvider BootProvider { get; }

        public SoftwareValidator(IBootProvider bootProvider, IValidator<SoftwareHashInfo> hashValidator)
            : base(hashValidator)
        {
            BootProvider = bootProvider;
        }

        protected override void DoValidate(SoftwareInfo software, string basePath, IProgress<double> progress, CancellationToken token)
        {
            var version = software.Version;
            Validate(version);
            Validate(software.Category, version);
            Validate(software.Product, version);
            Validate(software.Camera, version);
            Validate(software.Model, version);
            Validate(software.Build, version);
            Validate(software.Compiler, version);
            Validate(software.Source, version);
            Validate(software.Encoding, version);
            Validate(software.Hash, basePath, software.Category.Name, progress, token);
        }

        private void Validate(CategoryInfo category, Version version)
        {
            if (category == null)
                throw new ValidationException("Null category");

            if (string.IsNullOrEmpty(category.Name))
                throw new ValidationException("Missing category name");

            if (version.Major == 1 && category.Name != "EOS" && category.Name != "PS")
                throw new ValidationException("Invalid category name");
        }

        private static void Validate(SoftwareProductInfo product, Version version)
        {
            if (product == null)
                throw new ValidationException("Null product");

            if (string.IsNullOrEmpty(product.Name))
                throw new ValidationException("Missing product name");

            if (product.Version == null)
                throw new ValidationException("Null product version");

            if (product.Version.Major < 0 || product.Version.Minor < 0)
                throw new ValidationException("Invalid product version");

            if (product.VersionPrefix != null && product.VersionPrefix.Length == 0)
                throw new ValidationException("Invalid product versionPrefix");

            if (product.VersionSuffix != null && product.VersionSuffix.Length == 0)
                throw new ValidationException("Invalid product versionSuffix");

            ValidateCreated(product.Created, () => "product");

            // Optional in script
            if (version.Major == 1 && product.Language == null)
                throw new ValidationException("Invalid product language");
        }

        private static void Validate(SoftwareCameraInfo camera, Version _)
        {
            if (camera == null)
                throw new ValidationException("Null camera");

            if (string.IsNullOrEmpty(camera.Platform))
                throw new ValidationException("Missing camera platform");

            if (string.IsNullOrEmpty(camera.Revision))
                throw new ValidationException("Missing camera revision");
        }

        private void Validate(SoftwareModelInfo model, Version version)
        {
            if (model == null)
            {
                if (version.Major > 1)
                    throw new ValidationException("Null model");
                return;
            }

            if (model.Id == 0)
                throw new ValidationException("Zero model ID");

            if (model.Names == null || model.Names.Length == 0)
                throw new ValidationException("Missing model names");
        }

        private static void Validate(SoftwareBuildInfo build, Version version)
        {
            // Optional in script
            if (version.Major == 1 && build == null)
                throw new ValidationException("Null build");

            // Empty in update
            if (build.Name == null)
                throw new ValidationException("Null build name");

            // Empty in final
            if (build.Status == null)
                throw new ValidationException("Null build status");

            ValidateChangeset(build.Changeset, () => "build");
        }

        private static void Validate(SoftwareCompilerInfo compiler, Version _)
        {
            // Unknown in download, missing in script
            if (compiler == null)
                return;

            if (string.IsNullOrEmpty(compiler.Name))
                throw new ValidationException("Missing compiler name");

            if (compiler.Platform != null && compiler.Platform.Length == 0)
                throw new ValidationException("Missing compiler platform");

            if (compiler.Version == null)
                throw new ValidationException("Null compiler version");
        }

        private static void Validate(SoftwareSourceInfo source, Version _)
        {
            // Missing in manual build / script
            if (source == null)
                return;

            if (string.IsNullOrEmpty(source.Name))
                throw new ValidationException("Missing source name");

            if (source.Channel == null)
                throw new ValidationException("Missing source channel");

            if (source.Url == null)
                throw new ValidationException("Missing source url");
        }

        private void Validate(SoftwareEncodingInfo encoding, Version _)
        {
            // Missing if undetected / in script
            if (encoding == null)
                return;

            if (encoding.Name == null)
                throw new ValidationException("Missing encoding name");

            if (encoding.Name.Length > 0 && encoding.Data == null)
                throw new ValidationException("Missing encoding data");
        }

        private void Validate(SoftwareHashInfo hash, string basePath, string categoryName, IProgress<double> progress, CancellationToken token)
        {
            if (hash == null)
                ThrowValidationException("Null hash");

            var fileName = BootProvider.GetFileName(categoryName);
            if (!hash.Values.Keys.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                ThrowValidationException("Missing {0}", fileName);

            HashValidator.Validate(hash, basePath, progress, token);
        }
    }
}
