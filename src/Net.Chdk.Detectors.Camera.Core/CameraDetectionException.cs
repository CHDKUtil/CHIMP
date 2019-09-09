using System;

namespace Net.Chdk.Detectors.Camera
{
    public sealed class CameraDetectionException : Exception
    {
        public CameraDetectionException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }
    }
}
