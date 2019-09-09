using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Category;
using System;
using System.IO;
using System.Linq;

namespace Net.Chdk.Providers.Category
{
    sealed class CategoryProvider : DataProvider<string[]>, ICategoryProvider
    {
        #region Constants

        private const string DataFileName = "categories.json";

        #endregion
        
        #region Constructor

        public CategoryProvider(ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger<CategoryProvider>())
        {
        }

        #endregion

        #region ICategoryProvider Members

        public string[] GetCategoryNames()
        {
            return Data;
        }

        public CategoryInfo[] GetCategories()
        {
            return Data.Select(GetCategory).ToArray();
        }

        #endregion

        #region Data

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, DataFileName);
        }

        protected override LogLevel LogLevel => LogLevel.Information;

        protected override string Format => "Categories: {0}";

        #endregion

        #region Helper Methods

        private static CategoryInfo GetCategory(string name)
        {
            return new CategoryInfo
            {
                Name = name,
            };
        }

        #endregion
    }
}
