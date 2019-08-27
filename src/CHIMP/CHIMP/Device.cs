using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace Chimp
{
    static class Device
    {
        public static SafeFileHandle OpenRead(string deviceName)
        {
            var hDevice = CreateFile(deviceName,
                EAccessMode.Read, EShareMode.Read,
                IntPtr.Zero, ECreationDisposition.OpenExisting, 0, IntPtr.Zero);
            if (hDevice.IsInvalid)
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            return hDevice;
        }

        public static SafeFileHandle OpenReadWrite(string deviceName)
        {
            var hDevice = CreateFile(deviceName,
                EAccessMode.Read | EAccessMode.Write, EShareMode.Read | EShareMode.Write,
                IntPtr.Zero, ECreationDisposition.OpenExisting, 0, IntPtr.Zero);
            if (hDevice.IsInvalid)
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            return hDevice;
        }

        public static byte[] ReadBlock(SafeFileHandle hDevice, uint size)
        {
            var buffer = new byte[size];
            var bRead = ReadFile(hDevice, buffer, size, out int read, IntPtr.Zero);
            if (!bRead || read != size)
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            return buffer;
        }

        public static void WriteBlock(SafeFileHandle hDevice, byte[] buffer, uint size)
        {
            var bWrite = WriteFile(hDevice, buffer, size, out int written, IntPtr.Zero);
            if (!bWrite || written != size)
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        public static void MoveToStart(SafeFileHandle hDevice)
        {
            SetFilePointer(hDevice, 0, 0, EMoveMethod.Begin);
        }

        public static bool Invoke(SafeFileHandle hDevice, int controlCode)
        {
            return DeviceIoControl(hDevice, controlCode, IntPtr.Zero, 0, IntPtr.Zero, 0, out int read, IntPtr.Zero);
        }

        public static bool TryGet<T>(SafeFileHandle hDevice, int ioControlCode, out T value)
        {
            var size = Marshal.SizeOf<T>();
            var buffer = Marshal.AllocHGlobal(size);
            try
            {
                if (!DeviceIoControl(hDevice, ioControlCode, IntPtr.Zero, 0, buffer, size, out int read, IntPtr.Zero))
                {
                    value = default(T);
                    return false;
                }

                value = Marshal.PtrToStructure<T>(buffer);
                return true;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static bool Set<T>(SafeFileHandle hDevice, int ioControlCode, T value)
        {
            var size = Marshal.SizeOf<T>();
            var buffer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, buffer, false);
            try
            {
                if (!DeviceIoControl(hDevice, ioControlCode, IntPtr.Zero, 0, buffer, size, out int read, IntPtr.Zero))
                    return false;

                return true;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        [Flags]
        private enum EAccessMode : uint
        {
            None = 0,
            Read = 0x80000000,
            Write = 0x40000000,
        }

        [Flags]
        private enum EShareMode : uint
        {
            None = 0,
            Read = 0x00000001,
            Write = 0x00000002,
        }

        private enum ECreationDisposition : uint
        {
            None = 0,
            OpenExisting = 3
        }

        private enum EMoveMethod : uint
        {
            Begin = 0,
            Current = 1,
            End = 2
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DISK_GEOMETRY
        {
            public ulong Cylinders;
            public uint MediaType;
            public uint TracksPerCylinder;
            public uint SectorsPerTrack;
            public uint BytesPerSector;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DISK_GEOMETRY_EX
        {
            public DISK_GEOMETRY Geometry;
            public ulong DiskSize;
            public byte Data;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STORAGE_DEVICE_NUMBER
        {
            public uint DeviceType;
            public uint DeviceNumber;
            public uint PartitionNumber;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STORAGE_HOTPLUG_INFO
        {
            public uint Size;

            [MarshalAs(UnmanagedType.I1)]
            public bool MediaRemovable;

            [MarshalAs(UnmanagedType.I1)]
            public bool MediaHotplug;

            [MarshalAs(UnmanagedType.I1)]
            public bool DeviceHotplug;

            [MarshalAs(UnmanagedType.I1)]
            public bool WriteCacheEnableOverride;
        }

        public const int FSCTL_LOCK_VOLUME = 0x00090018;
        public const int FSCTL_DISMOUNT_VOLUME = 0x00090020;

        public const int IOCTL_DISK_GET_DRIVE_GEOMETRY = 0x70000;
        public const int IOCTL_DISK_GET_DRIVE_GEOMETRY_EX = 0x700a0;
        public const int IOCTL_VOLUME_UPDATE_PROPERTIES = 0x70140;
        public const int IOCTL_VOLUME_ONLINE = 0x0056c008;
        public const int IOCTL_VOLUME_OFFLINE = 0x0056c00c;
        public const int IOCTL_STORAGE_GET_HOTPLUG_INFO = 0x2d0c14;
        public const int IOCTL_STORAGE_GET_DEVICE_NUMBER = 0x2d1080;
        public const int IOCTL_STORAGE_MEDIA_REMOVAL = 0x2d4804;
        public const int IOCTL_STORAGE_EJECT_MEDIA = 0x2d4808;

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern SafeFileHandle CreateFile(string fileName, EAccessMode accessMode,
            EShareMode shareMode, IntPtr lpSecurityAttributes, ECreationDisposition creationDisposition,
            uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ReadFile(SafeFileHandle hDevice, byte[] lpBuffer,
           uint nNumBytesToRead, out int lpNumBytesRead, IntPtr lpOverlapped);

        [DllImport("kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WriteFile(SafeFileHandle hDevice, byte[] lpBuffer,
           uint nNumBytesToWrite, out int lpNumBytesWritten, IntPtr lpOverlapped);

        [DllImport("kernel32", SetLastError = true)]
        private static extern int SetFilePointer(SafeFileHandle hDevice, int lDistanceToMove,
            [In, Out] int lpDistanceToMoveHigh, EMoveMethod dwMoveMethod);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool DeviceIoControl(SafeFileHandle hDevice, int dwIoControlCode,
            IntPtr lpInBuffer, int nInBufferSize, IntPtr lpOutBuffer, int nOutBufferSize,
            out int lpBytesReturned, IntPtr lpOverlapped);
    }
}
