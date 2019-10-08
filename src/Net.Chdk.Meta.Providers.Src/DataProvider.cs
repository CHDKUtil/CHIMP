﻿using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.Src
{
    public abstract class DataProvider<T> : MakefileParsingProvider<T>
        where T : class
    {
        protected DataProvider(ILogger logger)
            : base(logger)
        {
        }

        public T? GetData(string basePath, string platform, string? revision = null)
        {
            return GetValue(basePath, platform, revision);
        }

        protected override string Prefix => string.Empty;
    }
}