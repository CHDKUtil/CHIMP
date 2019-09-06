using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Software
{
    sealed class ProductModuleProvider : DataProvider<ProductModuleProvider.ComponentsData>, IProductModuleProvider
    {
        #region Constants

        private const string DataFileName = "components.json";

        #endregion

        #region Fields

        private string ProductName { get; }

        #endregion

        #region Constructor

        public ProductModuleProvider(string productName, ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger<ProductModuleProvider>())
        {
            ProductName = productName;

            modules = new Lazy<Dictionary<string, ModuleData>>(GetModules);
            moduleNames = new Lazy<Dictionary<string, string>>(GetModuleNames);
        }

        #endregion

        #region IProductModuleProvider Members

        public string Path => Data.Modules?.Path;

        public string Extension => Data.Modules?.Extension;

        public string GetModuleName(string filePath)
        {
            ModuleNames.TryGetValue(filePath, out string moduleName);
            return moduleName;
        }

        public string GetModuleTitle(string moduleName)
        {
            if (!Modules.TryGetValue(moduleName, out ModuleData module))
                return null;
            return module.Title;
        }

        public string GetModuleId(string moduleName)
        {
            if (!Modules.TryGetValue(moduleName, out ModuleData module))
                return null;
            return module.Id;
        }

        #endregion

        #region Data

        internal sealed class ModulesData
        {
            public string Path { get; set; }
            public string Extension { get; set; }
            public IDictionary<string, ModuleData> Children { get; set; }
        }

        internal sealed class ModuleData
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public IDictionary<string, ModuleData> Children { get; set; }
            public string[] Files { get; set; }
        }

        internal sealed class ComponentsData
        {
            public ModulesData Modules { get; set; }
        }

        protected override string GetFilePath()
        {
            return System.IO.Path.Combine(Directories.Data, Directories.Product, ProductName, DataFileName);
        }

        #endregion

        #region Modules

        private readonly Lazy<Dictionary<string, ModuleData>> modules;

        private Dictionary<string, ModuleData> Modules => modules.Value;

        private Dictionary<string, ModuleData> GetModules()
        {
            Dictionary<string, ModuleData> modules = new Dictionary<string, ModuleData>();
            GetModules(Data.Modules?.Children, modules);
            return modules;
        }

        private void GetModules(IDictionary<string, ModuleData> children, Dictionary<string, ModuleData> modules)
        {
            if (children != null)
            {
                foreach (var kvp in children)
                {
                    var name = kvp.Key;
                    var module = kvp.Value;
                    if (name.Length > 0)
                        modules.Add(name, module);
                    GetModules(module.Children, modules);
                }
            }
        }

        #endregion

        #region ModuleNames

        private readonly Lazy<Dictionary<string, string>> moduleNames;

        private Dictionary<string, string> ModuleNames => moduleNames.Value;

        private Dictionary<string, string> GetModuleNames()
        {
            var moduleNames = new Dictionary<string, string>();
            GetModuleNames(Data.Modules?.Children, moduleNames);
            return moduleNames;
        }

        private static void GetModuleNames(IDictionary<string, ModuleData> modules, Dictionary<string, string> moduleNames)
        {
            if (modules != null)
            {
                foreach (var kvp in modules)
                {
                    var files = kvp.Value.Files;
                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            moduleNames.Add(file.ToLowerInvariant(), kvp.Key);
                        }
                    }
                    GetModuleNames(kvp.Value.Children, moduleNames);
                }
            }
        }

        #endregion
    }
}
