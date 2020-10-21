using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Runtime.InteropServices;

namespace FP.SetWallpaper
{
    public static class Extensions
    {
        //These are the extensions listed in the File Dialog shown when setting a wallpaper in Windows 10

        private static readonly List<string> SUPPORTED_IMAGE_EXTENSIONS =
            new List<string>
            {
                ".jpg",
                ".jpeg",
                ".bmp",
                ".dib",
                ".png",
                ".jfif",
                ".jpe",
                ".gif",
                ".tif",
                ".tiff",
                ".wdp",
                ".heic",
                ".heif",
                ".heics",
                ".hif",
                ".avci",
                ".avcs",
                ".avif",
                ".avifs"
            };

        public static void Release(this object comObject)
        {
            if (comObject == null) return;

            while (Marshal.ReleaseComObject(comObject) > 0)
            {
            }
        }

        public static ErrorRecord ToErrorRecord(this COMException comEx, object targetObject = null)
        {
            if (comEx is null) throw new ArgumentNullException(nameof(comEx));

            return new ErrorRecord(comEx, "COMInteropFailure", ErrorCategory.InvalidOperation, targetObject);
        }

        public static bool HasPlausibleFileExtension(this string pathToWallpaper)
        {
            if (pathToWallpaper is null) throw new ArgumentNullException(nameof(pathToWallpaper));

            var wallpaperFileExtension = Path.GetExtension(pathToWallpaper);

            return SUPPORTED_IMAGE_EXTENSIONS.Contains(wallpaperFileExtension);
        }
    }
}