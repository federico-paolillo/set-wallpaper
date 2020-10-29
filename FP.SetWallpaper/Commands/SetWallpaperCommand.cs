using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Runtime.InteropServices;
using FP.SetWallpaper.Output;
using FP.SetWallpaper.COM;

namespace FP.SetWallpaper.Commands
{
    [Cmdlet(VerbsCommon.Set, "Wallpaper", SupportsShouldProcess = true,
        DefaultParameterSetName = BY_ID_AND_LITERAL_PATH)]
    [OutputType(typeof(Monitor))]
    [OutputType(typeof(string))]
    public class SetWallpaperCommand : PSCmdlet
    {
        public const string BY_INPUT_OBJECT_AND_PATH = "ByInputObjectAndPath";
        public const string BY_INPUT_OBJECT_AND_LITERAL_PATH = "ByInputObjectAndLiteralPath";
        public const string BY_ID_AND_LITERAL_PATH = "ByIdAndLiteralPath";
        public const string BY_ID_AND_PATH = "ByIdAndPath";

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = BY_INPUT_OBJECT_AND_LITERAL_PATH)]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = BY_INPUT_OBJECT_AND_PATH)]
        public Monitor[] InputObject { get; set; }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = BY_ID_AND_LITERAL_PATH)]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = BY_ID_AND_PATH)]
        public string[] Id { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = BY_INPUT_OBJECT_AND_PATH)]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = BY_ID_AND_PATH)]
        [Alias("PSPath")]
        public string Path { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = BY_INPUT_OBJECT_AND_LITERAL_PATH)]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = BY_ID_AND_LITERAL_PATH)]
        public string LiteralPath { get; set; }

        [Parameter(Mandatory = false)] public SwitchParameter PassThru { get; set; }

        [Parameter(Mandatory = false)] public SwitchParameter Force { get; set; }

        protected override void ProcessRecord()
        {
            var userConfirmed = ShouldProcess("Changing desktop wallpaper", "Do you want to proceed ?", "Are you sure ?");

            if (!userConfirmed) return;

            try
            {
                switch (ParameterSetName)
                {
                    case BY_INPUT_OBJECT_AND_PATH:
                        ProcessByInputObjectAndPath();
                        break;
                    case BY_INPUT_OBJECT_AND_LITERAL_PATH:
                        ProcessByInputObjectAndLiteralPath();
                        break;
                    case BY_ID_AND_PATH:
                        ProcessByIdAndPath();
                        break;
                    default:
                        ProcessByIdAndLiteralPath();
                        break;
                }
            }
            catch (COMException comEx)
            {
                var errorRecord = comEx.ToErrorRecord();

                ThrowTerminatingError(errorRecord);
            }
        }

        private void ProcessByIdAndLiteralPath()
        {
            Process(Id, LiteralPath);

            if (PassThru) WriteObject(Id, true);
        }

        private void ProcessByIdAndPath()
        {
            Process(Id, Path);

            if (PassThru) WriteObject(Id, true);
        }

        private void ProcessByInputObjectAndLiteralPath()
        {
            Process(InputObject, LiteralPath);

            if (PassThru) WriteObject(InputObject, true);
        }

        private void ProcessByInputObjectAndPath()
        {
            Process(InputObject, Path);

            if (PassThru) WriteObject(InputObject, true);
        }

        private void Process(IEnumerable<Monitor> monitors, string wallpaperPSPath)
        {
            if (monitors == null) throw new ArgumentNullException(nameof(monitors));
            if (wallpaperPSPath == null) throw new ArgumentNullException(nameof(wallpaperPSPath));

            var monitorIds = monitors.Select(monitor => monitor.Id)
                .ToList();

            Process(monitorIds, wallpaperPSPath);
        }

        private void Process(IEnumerable<string> monitorIds, string wallpaperPSPath)
        {
            if (monitorIds == null) throw new ArgumentNullException(nameof(monitorIds));
            if (wallpaperPSPath == null) throw new ArgumentNullException(nameof(wallpaperPSPath));

            var wallpaperPath = ResolvePSPathToWallpaper(wallpaperPSPath);

            var wallpaperFileExtensionMakesSense = wallpaperPath.HasPlausibleFileExtension();
            var doesWallpaperFileExists = File.Exists(wallpaperPath);

            if (!wallpaperFileExtensionMakesSense)
            {
                if (!Force)
                {
                    if (!ShouldContinue("The wallpaper file does not have an image extension, use it anyway ?", "Wallpaper file might not be an image"))
                    {
                        return;
                    }
                }
            }

            if (!doesWallpaperFileExists)
            {
                if (!Force)
                {
                    if (!ShouldContinue("The wallpaper file does not exist, continue anyway ?", "Wallpaper file does not exist"))
                    {
                        return;
                    }
                }
            }

            SetWallpapers(monitorIds, wallpaperPath);
        }

        private void SetWallpapers(IEnumerable<string> monitorIds, string wallpaperPath)
        {
            if (monitorIds == null) throw new ArgumentNullException(nameof(monitorIds));
            if (wallpaperPath == null) throw new ArgumentNullException(nameof(wallpaperPath));

            IDesktopWallpaper desktopWallpaper = null;

            try
            {
                desktopWallpaper = (IDesktopWallpaper) new DesktopWallpaper();

                foreach (var monitorId in monitorIds)
                {
                    desktopWallpaper.SetWallpaper(monitorId, wallpaperPath);
                }
            }
            finally
            {
                desktopWallpaper.Release();
            }
        }

        private string ResolvePSPathToWallpaper(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var paths = GetResolvedProviderPathFromPSPath(path, out _);

            WriteVerbose($"PowerShell Path resolved to {paths.Count} file system path(s)");

            if (paths.Count == 0)
            {
                WriteWarning("No paths were resolved from the Path provided");

                return string.Empty;
            }

            if (paths.Count > 1)
            {
                WriteWarning("Multiple paths were resolved from the Path provided, taking the first one");
            }

            var pathToWallpaper = paths[0];

            WriteVerbose($"Wallpaper path selected is: {pathToWallpaper}");

            return pathToWallpaper;
        }
    }
}