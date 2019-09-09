using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading;

namespace Net.Chdk.Validators
{
    public abstract class Validator<T> : IValidator<T>
    {
        public void Validate(T value, string basePath, IProgress<double> progress, CancellationToken token)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(basePath))
                throw new ArgumentException("Missing base path", nameof(basePath));

            DoValidate(value, basePath, progress, token);
        }

        protected abstract void DoValidate(T value, string basePath, IProgress<double> progress, CancellationToken token);

        protected static void Validate(Version version)
        {
            if (version == null)
                throw new ValidationException("Null version");

            if (version.Major < 1 || version.Minor < 0)
                throw new ValidationException("Invalid version");
        }

        protected static void ValidateCreated(DateTime? created, Func<string> formatter)
        {
            if (created == null)
                ThrowValidationException("Null {0} created", formatter);

            if (created.Value < new DateTime(2000, 1, 1) || created.Value > DateTime.Now)
                ThrowValidationException("Invalid {0} created", formatter);
        }

        protected static void ValidateChangeset(string changeset, Func<string> formatter)
        {
            if (changeset == null)
                return;

            try
            {
                ulong.Parse(changeset, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            catch
            {
                ThrowValidationException("Invalid {0} changeset", formatter);
            }
        }

        protected static void ThrowValidationException(string format, Func<string> formatter)
        {
            var arg = formatter();
            ThrowValidationException(format, arg);
        }

        protected static void ThrowValidationException(string format, params string[] args)
        {
            var message = string.Format(format, args);
            throw new ValidationException(message);
        }
    }
}
