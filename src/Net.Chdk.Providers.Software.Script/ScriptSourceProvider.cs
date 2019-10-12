using System;
using System.Collections.Generic;
using System.Linq;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;

namespace Net.Chdk.Providers.Software.Script
{
    sealed class ScriptSourceProvider : IProductSourceProvider
    {
        private const string CategoryName = "SCRIPT";

        private IProductProvider ProductProvider { get; }

        public ScriptSourceProvider(IProductProvider productProvider)
        {
            ProductProvider = productProvider;
        }

        public IEnumerable<ProductSource> GetSources(CategoryInfo category)
        {
            return IsValidCategory(category.Name)
                ? ProductProvider
                    .GetProductNames()
                    .Where(IsValidProduct)
                    .SelectMany(GetSources)
                : Enumerable.Empty<ProductSource>();
        }

        public IEnumerable<ProductSource> GetSources(SoftwareProductInfo product)
        {
            if (IsValidProduct(product.Name))
                yield return CreateProductSource(product.Name);
        }

        public IEnumerable<SoftwareSourceInfo> GetSources(SoftwareProductInfo product, string sourceName)
        {
            if (IsValidProduct(product.Name))
                yield return CreateSourceInfo(product.Name);
        }

        private static bool IsValidCategory(string? categoryName)
        {
            return categoryName == CategoryName;
        }

        private bool IsValidProduct(string? productName)
        {
            var categoryName = ProductProvider.GetCategoryName(productName);
            return IsValidCategory(categoryName);
        }

        private IEnumerable<ProductSource> GetSources(string? productName)
        {
            yield return CreateProductSource(productName);
        }

        private static ProductSource CreateProductSource(string? productName)
        {
            if (productName == null)
                throw new ArgumentNullException(nameof(productName));
            return new ProductSource(productName, productName, CreateSourceInfo(productName));
        }

        private static SoftwareSourceInfo CreateSourceInfo(string? productName)
        {
            return new SoftwareSourceInfo
            {
                Name = productName
            };
        }
    }
}
