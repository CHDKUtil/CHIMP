using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Product;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Zip
{
    public abstract class ZipMetaProvider<T>
        where T : class
    {
        protected ILogger Logger { get; }
        private IProductProvider ProductProvider { get; }
        private IBootProvider BootProvider { get; }

        protected ZipMetaProvider(IProductProvider productProvider, IBootProvider bootProvider, ILogger logger)
        {
            ProductProvider = productProvider;
            BootProvider = bootProvider;
            Logger = logger;
        }

        public string Extension => ".zip";

        protected IEnumerable<T> GetItems(string path, string productName)
        {
            var categoryName = ProductProvider.GetCategoryName(productName);
            var bootFileName = BootProvider.GetFileName(categoryName);
            using (var stream = File.OpenRead(path))
            {
                var fileName = Path.GetFileName(path);
                return GetItems(stream, fileName, productName, bootFileName);
            }
        }

        private IEnumerable<T> GetItems(Stream stream, string fileName, string productName, string bootFileName)
        {
            using (var zip = new ZipFile(stream))
            {
                return GetItems(zip, fileName, productName, bootFileName).ToArray();
            }
        }

        private IEnumerable<T> GetItems(ZipFile zip, string fileName, string productName, string bootFileName)
        {
            Logger.LogInformation("Enter {0}", fileName);
            foreach (ZipEntry entry in zip)
            {
                var items = GetItems(zip, entry, productName, bootFileName);
                foreach (var item in items)
                    yield return item;
                yield return GetItem(zip, fileName, productName, entry, bootFileName);
            }
            Logger.LogInformation("Exit {0}", fileName);
        }

        private IEnumerable<T> GetItems(ZipFile zip, ZipEntry entry, string productName, string bootFileName)
        {
            if (!entry.IsFile)
                return Enumerable.Empty<T>();

            var ext = Path.GetExtension(entry.Name);
            if (!".zip".Equals(ext, StringComparison.OrdinalIgnoreCase))
                return Enumerable.Empty<T>();

            var fileName = Path.GetFileName(entry.Name);
            using (var stream = zip.GetInputStream(entry))
            {
                return GetItems(stream, fileName, productName, bootFileName);
            }
        }

        private T GetItem(ZipFile zip, string fileName, string productName, ZipEntry entry, string bootFileName)
        {
            if (!entry.IsFile)
                return null;

            if (!bootFileName.Equals(entry.Name, StringComparison.OrdinalIgnoreCase))
                return null;

            return DoGetItem(zip, fileName, productName, entry);
        }

        protected abstract T DoGetItem(ZipFile zip, string fileName, string productName, ZipEntry entry);
    }
}
