﻿using System;
using System.Threading;

namespace Net.Chdk.Validators
{
    public interface IValidator<T>
    {
        void Validate(T value, string basePath, IProgress<double> progress, CancellationToken token);
    }
}
