using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Providers.Product
{
    sealed class ProductProvider : DataProvider<Dictionary<string, string>>, IProductProvider
    {
        #region Constants

        private const string DataFileName = "products.json";

        #endregion

        #region Constructor

        public ProductProvider(ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger<ProductProvider>())
        {
            _categoryNames = new Lazy<string[]>(DoGetCategoryNames);
        }

        #endregion

        #region IProductProvider Members

        public string[] GetCategoryNames()
        {
            return CategoryNames;
        }

        public string[] GetProductNames()
        {
            return Data.Keys.ToArray();
        }

        public string GetCategoryName(string productName)
        {
            return Data[productName];
        }

        #endregion

        #region Data

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, DataFileName);
        }

        protected override LogLevel LogLevel => LogLevel.Information;

        protected override string? Format => "Products: {0}";

        #endregion

        #region CategoryNames

        private readonly Lazy<string[]> _categoryNames;

        private string[] CategoryNames => _categoryNames.Value;

        private string[] DoGetCategoryNames()
        {
            return Data.Values
                .Distinct()
                .OrderBy(c => c)
                .ToArray();
        }

        #endregion
    }
}
