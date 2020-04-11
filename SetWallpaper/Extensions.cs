using System.Runtime.InteropServices;

namespace SetWallpaper
{
    public static class Extensions
    {
        public static void Release(this object comObject)
        {
            if (comObject == null) return;

            while (Marshal.ReleaseComObject(comObject) > 0) { };
        }
    }
}
