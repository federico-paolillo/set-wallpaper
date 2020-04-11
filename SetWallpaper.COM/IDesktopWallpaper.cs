using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SetWallpaper.COM
{
    [ComImport, Guid("B92B56A9-8B55-4E14-9A89-0199BBB6F93B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDesktopWallpaper
    {
        [PreserveSig]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        int SetWallpaper([MarshalAs(UnmanagedType.LPWStr), In] string monitorID, [MarshalAs(UnmanagedType.LPWStr), In] string wallpaper);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        int GetWallpaper([MarshalAs(UnmanagedType.LPWStr), In] string monitorID, [MarshalAs(UnmanagedType.LPWStr), Out] out string wallpaper);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        int GetMonitorDevicePathAt([MarshalAs(UnmanagedType.U4), In] uint monitorIndex, [MarshalAs(UnmanagedType.LPWStr), Out] out string monitorID);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        int GetMonitorDevicePathCount([MarshalAs(UnmanagedType.U4), Out] out uint count);
    }
}
