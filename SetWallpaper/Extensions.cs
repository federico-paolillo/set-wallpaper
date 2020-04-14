using System.Management.Automation;
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

        public static ErrorRecord ToErrorRecord(this COMException comEx, object targetObject = null)
        {
            return new ErrorRecord(comEx, "COMInteropFailure", ErrorCategory.InvalidOperation, targetObject);
        }
    }
}
