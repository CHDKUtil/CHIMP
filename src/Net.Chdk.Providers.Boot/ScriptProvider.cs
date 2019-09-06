using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Providers.Boot
{
    sealed class ScriptProvider : BootProvider<ScriptProvider.ScriptData>, IScriptProvider
    {
        #region Constants

        private const string DataFileName = "script.json";

        #endregion

        #region Constructor

        public ScriptProvider(ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger<ScriptProvider>())
        {
        }

        #endregion

        #region IScriptProvider Members

        public IDictionary<string, byte[]> GetFiles()
        {
            return Files;
        }

        #endregion

        #region Data

        internal sealed class ScriptData : DataBase
        {
        }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, DataFileName);
        }

        #endregion
    }
}
