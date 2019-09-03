using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    abstract class MakefileParsingProvider<T> : ParsingProvider<T>
        where T : class
    {
        protected MakefileParsingProvider(ILogger logger)
            : base(logger)
        {
        }

        protected sealed override string FileName => "makefile.inc";

        protected sealed override string TrimComments(string line, string platform, string revision)
        {
            var index = line.IndexOf('#');
            if (index >= 0)
                line = line.Substring(0, index).TrimEnd();
            return line;
        }
    }
}
