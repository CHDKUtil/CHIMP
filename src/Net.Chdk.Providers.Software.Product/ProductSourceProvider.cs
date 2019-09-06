using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Net.Chdk.Providers.Software.Product
{
    public abstract class ProductSourceProvider : DataProvider<Dictionary<string, SoftwareSourceInfo>>, IProductSourceProvider
    {
        #region Constants

        private const string DataFileName = "sources.json";

        #endregion

        #region Fields

        private IProductProvider ProductProvider { get; }

        #endregion

        #region Constructor

        protected ProductSourceProvider(IProductProvider productProvider, ILogger logger)
            : base(logger)
        {
            ProductProvider = productProvider;
        }

        #endregion

        #region Data

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, Directories.Product, ProductName, DataFileName);
        }

        #endregion
        
        #region IProductSourceProvider Members

        public IEnumerable<ProductSource> GetSources(CategoryInfo category)
        {
            return Data
                .Where(kvp => IsMatch(kvp.Value, category))
                .Select(CreateProductSource)
                .OrderBy(GetProductSourceOrder);
        }

        public IEnumerable<ProductSource> GetSources(SoftwareProductInfo product)
        {
            return Data
                .Where(kvp => IsMatch(kvp.Value, product))
                .Select(CreateProductSource)
                .OrderBy(GetProductSourceOrder);
        }

        public IEnumerable<SoftwareSourceInfo> GetSources(SoftwareProductInfo product, string sourceName)
        {
            return Data
                .Select(kvp => kvp.Value)
                .Where(s => IsMatch(s, product, sourceName))
                .OrderBy(GetSourceOrder);
        }

        #endregion

        #region Abstract/Virtual Members

        protected abstract string ProductName { get; }

        protected virtual string GetChannelName(SoftwareProductInfo product) => null;

        protected virtual CultureInfo GetLanguage(SoftwareSourceInfo source) => null;

        #endregion

        #region Helper Members

        private string CategoryName => ProductProvider.GetCategoryName(ProductName);

        private bool IsMatch(SoftwareSourceInfo source, CategoryInfo category)
        {
            if (category?.Name == null)
                return true;

            return CategoryName.Equals(category.Name, StringComparison.Ordinal);
        }

        private bool IsMatch(SoftwareProductInfo product)
        {
            if (product?.Name == null)
                return true;

            return ProductName.Equals(product.Name, StringComparison.Ordinal);
        }

        private bool IsMatch(SoftwareSourceInfo source, SoftwareProductInfo product)
        {
            if (!IsMatch(product))
                return false;

            var channelName = GetChannelName(product);
            if (channelName == null)
                return true;

            if (!channelName.Equals(source.Channel, StringComparison.Ordinal))
                return false;

            if (product.Language == null)
                return true;

            var language = GetLanguage(source);
            if (language == null)
                return true;

            return language.Equals(product.Language);
        }

        private bool IsMatch(SoftwareSourceInfo source, SoftwareProductInfo product, string sourceName)
        {
            return IsMatch(source, product)
                && sourceName.Equals(source.Name, StringComparison.Ordinal);
        }

        private ProductSource CreateProductSource(KeyValuePair<string, SoftwareSourceInfo> kvp)
        {
            return new ProductSource(ProductName, kvp.Key, kvp.Value);
        }

        private int GetProductSourceOrder(ProductSource productSource)
        {
            return GetSourceOrder(productSource.Source);
        }

        private int GetSourceOrder(SoftwareSourceInfo source)
        {
            var language = GetLanguage(source);
            return language.IsCurrentUICulture()
                ? -1
                : 0;
        }

        #endregion
    }
}
