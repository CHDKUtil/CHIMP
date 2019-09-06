using System;

namespace Net.Chdk
{
    public static class Files
    {
        public static class Metadata
        {
            public const string Software = "SOFTWARE.JSN";
            public const string Modules = "MODULES.JSN";

            [Obsolete]
            public const string Camera = "CAMERA.JSN";

            [Obsolete]
            public const string CameraModel = "MODEL.JSN";
        }
    }
}
