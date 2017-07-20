using Net.Chdk.Model.Card;
using Net.Chdk.Providers.Boot;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chimp.Services
{
    sealed class ScriptService : BootServiceBase, IScriptService
    {
        private IScriptProvider ScriptProvider { get; }

        public ScriptService(IVolumeContainer volumeContainer, IScriptProvider scriptProvider)
            : base(volumeContainer)
        {
            ScriptProvider = scriptProvider;
        }

        public bool TestScriptable(CardInfo cardInfo, string fileSystem)
        {
            if (fileSystem == null)
                return false;

            var files = ScriptProvider.GetFiles();
            if (!TestScriptable(cardInfo, files))
                return false;

            var blockSize = ScriptProvider.GetBlockSize(fileSystem);
            var bytes = ScriptProvider.GetBytes(fileSystem);
            if (!Test(cardInfo, blockSize, bytes))
                return false;

            return true;
        }

        public bool SetScriptable(CardInfo cardInfo, string fileSystem, bool value)
        {
            if (fileSystem == null)
                return false;

            var files = ScriptProvider.GetFiles();
            if (TestScriptable(cardInfo, files) != value)
                SetScriptable(cardInfo, files, value);

            var blockSize = ScriptProvider.GetBlockSize(fileSystem);
            var bytes = ScriptProvider.GetBytes(fileSystem);
            return Set(cardInfo, blockSize, bytes, value);
        }

        private bool TestScriptable(CardInfo cardInfo, IDictionary<string, byte[]> files)
        {
            if (files == null)
                return false;
            return files.All(kvp => TestScriptable(cardInfo, kvp));
        }

        private static bool TestScriptable(CardInfo cardInfo, KeyValuePair<string, byte[]> kvp)
        {
            var filePath = GetPath(cardInfo, kvp.Key);
            return File.Exists(filePath);
        }

        private static void SetScriptable(CardInfo cardInfo, IDictionary<string, byte[]> files, bool value)
        {
            if (value)
                SetScriptable(cardInfo, files);
            else
                ClearScriptable(cardInfo, files);
        }

        private static void SetScriptable(CardInfo cardInfo, IDictionary<string, byte[]> files)
        {
            foreach (var kvp in files)
                SetScriptable(cardInfo, kvp.Key, kvp.Value);
        }

        private static void ClearScriptable(CardInfo cardInfo, IDictionary<string, byte[]> files)
        {
            foreach (var kvp in files)
                ClearScriptable(cardInfo, kvp.Key);
        }

        private static void SetScriptable(CardInfo cardInfo, string fileName, byte[] bytes)
        {
            var path = GetPath(cardInfo, fileName);
            File.WriteAllBytes(path, bytes);
        }

        private static void ClearScriptable(CardInfo cardInfo, string fileName)
        {
            var path = GetPath(cardInfo, fileName);
            File.Delete(path);
        }
    }
}
