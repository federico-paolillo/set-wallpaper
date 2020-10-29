using System.Runtime.InteropServices;

namespace FP.SetWallpaper.COM
{
    [ComImport]
    [Guid("B92B56A9-8B55-4E14-9A89-0199BBB6F93B")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDesktopWallpaper
    {
        void SetWallpaper([MarshalAs(UnmanagedType.LPWStr)] [In] string monitorID, [MarshalAs(UnmanagedType.LPWStr)] [In] string wallpaper);

        void GetWallpaper([MarshalAs(UnmanagedType.LPWStr)] [In] string monitorID, [MarshalAs(UnmanagedType.LPWStr)] [Out] out string wallpaper);

        void GetMonitorDevicePathAt([MarshalAs(UnmanagedType.U4)] [In] uint monitorIndex, [MarshalAs(UnmanagedType.LPWStr)] [Out] out string monitorID);

        void GetMonitorDevicePathCount([MarshalAs(UnmanagedType.U4)] [Out] out uint count);
    }
}