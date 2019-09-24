using Net.Chdk;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Chimp.Providers
{
    sealed class ChdkResourceProvider : IResourceProvider
    {
        #region Constants

        private const string LanguagePath = "LANG";

        private const string ProductName = "CHDK";

        #endregion

        #region Constructor

        public ChdkResourceProvider()
        {
            strings = new Lazy<Dictionary<string, string>>(GetStrings);
        }

        #endregion

        #region IResourceProvider Members

        public string GetString(string id)
        {
            string value;
            Strings.TryGetValue(id, out value);
            return value;
        }

        #endregion

        #region Strings

        private readonly Lazy<Dictionary<string, string>> strings;

        private Dictionary<string, string> Strings => strings.Value;

        private Dictionary<string, string> GetStrings()
        {
            var strings = new Dictionary<string, string>();
            var culture = Thread.CurrentThread.CurrentUICulture;
            while (culture.Parent != CultureInfo.InvariantCulture)
                culture = culture.Parent;
            var langName = culture.EnglishName;
            var fileName = $"{langName}.lng";
            var filePath = Path.Combine(Directories.Data, Directories.Product, ProductName, LanguagePath, fileName);
            if (File.Exists(filePath))
            {
                using var reader = File.OpenText(filePath);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length > 0)
                    {
                        line = line.TrimStart();
                        if (!line.StartsWith("//"))
                        {
                            var index = line.IndexOf(' ');
                            if (index > 0)
                            {
                                var key = line.Substring(0, index);
                                var value = GetValue(line, index);
                                strings.Add(key, value);
                            }
                        }
                    }
                }
            }
            return strings;
        }

        private static string GetValue(string line, int index)
        {
            var str = line.Substring(index + 1).Trim();
            str = str.Substring(1, str.Length - 2);
            return str.Replace("\\\"", "\"");
        }

        #endregion
    }
}
