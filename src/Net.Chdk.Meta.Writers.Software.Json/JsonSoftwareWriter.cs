using Net.Chdk.Meta.Writers.Json;
using Net.Chdk.Model.Software;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Software.Json
{
    sealed class JsonSoftwareWriter : JsonMetaWriter, ISoftwareWriter
    {
        #region ISoftwareWriter Members

        public void WriteSoftware(string path, IDictionary<string, SoftwareInfo> hash2sw)
        {
            WriteJson(path, hash2sw);
        }

        #endregion

        #region JsonMetaWriter Overrides

        protected override PreserveReferencesHandling PreserveReferencesHandling => PreserveReferencesHandling.All;

        protected override IEnumerable<JsonConverter> GetConverters()
        {
            yield return new VersionConverter();
        }

        #endregion
    }
}
