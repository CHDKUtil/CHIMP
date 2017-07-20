using System;
using System.Linq;
using System.Management;

namespace Chimp
{
    static class OperatingSystem
    {
        sealed class Data
        {
            public Data()
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                using (var query = searcher.Get())
                {
                    var os = query.Cast<ManagementObject>().SingleOrDefault();
                    if (os == null)
                        return;

                    OSType = (ushort)os["OSType"];
                    Caption = os["Caption"] as string;
                    CSDVersion = os["CSDVersion"] as string;
                    OSArchitecture = os["OSArchitecture"] as string;
                    Version = Version.Parse(os["Version"] as string);
                }
            }

            public string Caption { get; }
            public string CSDVersion { get; }
            public string OSArchitecture { get; }
            public ushort OSType { get; }
            public Version Version { get; }
        }

        private static Lazy<Data> data = new Lazy<Data>(() => new Data());

        private static Data Value => data.Value;

        public static string Caption => Value.Caption;
        public static string CSDVersion => Value.CSDVersion;
        public static string OSArchitecture => Value.OSArchitecture;
        public static ushort OSType => Value.OSType;
        public static Version Version => Value.Version;

        public static string DisplayName
        {
            get
            {
                return $"{Caption}{CSDVersion} {OSArchitecture}";
            }
        }
    }
}
