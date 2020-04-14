using SetWallpaper.Output;
using System;
using System.IO;
using System.Management.Automation;

namespace SetWallpaper.Commands
{
    [Cmdlet(VerbsCommon.Set, "Wallpaper", SupportsShouldProcess = true, DefaultParameterSetName = BY_ID_AND_LITERALPATH)]
    [OutputType(typeof(Monitor))]
    [OutputType(typeof(string))]
    public class SetWallpaperCommand : PSCmdlet
    {
        public const string BY_INPUT_OBJECT_AND_PATH = "ByInputObjectAndPath";
        public const string BY_INPUT_OBJECT_AND_LITERAL_PATH = "ByInputObjectAndLiteralPath";
        public const string BY_ID_AND_LITERALPATH = "ByIdAndLiteralPath";
        public const string BY_ID_AND_PATH = "ByIdAndPath";

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = BY_INPUT_OBJECT_AND_LITERAL_PATH)]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = BY_INPUT_OBJECT_AND_PATH)]
        public Monitor[] InputObject { get; set; }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = BY_ID_AND_LITERALPATH)]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = BY_ID_AND_PATH)]
        public string[] Id { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = BY_INPUT_OBJECT_AND_PATH)]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = BY_ID_AND_PATH)]
        [Alias("PSPath")]
        public string Path { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = BY_INPUT_OBJECT_AND_LITERAL_PATH)]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = BY_ID_AND_LITERALPATH)]
        public string LiteralPath { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter PassThrough { get; set; }

        protected override void ProcessRecord()
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

        private void ProcessByIdAndLiteralPath()
        {
            if (PassThrough) WriteObject(Id, enumerateCollection: true);
        }

        private void ProcessByIdAndPath()
        {
            var pathToWallpaper = GetPathToWallpaper(Path, isLiteral: false);

            if (PassThrough) WriteObject(Id, enumerateCollection: true);
        }

        private void ProcessByInputObjectAndLiteralPath()
        {
            if (PassThrough) WriteObject(InputObject, enumerateCollection: true);
        }

        private void ProcessByInputObjectAndPath()
        {
            if (PassThrough) WriteObject(InputObject, enumerateCollection: true);
        }

        private void SetWallpapers(string[] ids, string pathToWallpaper)
        {
            if (ids is null) throw new ArgumentNullException(nameof(ids));
            if (pathToWallpaper is null) throw new ArgumentNullException(nameof(pathToWallpaper));

            var plausibleExtension = pathToWallpaper.HasPlausibleFileExtension();

            if (!plausibleExtension) WriteWarning("You have provided a wallpaper file that hasn't a supported image file extension, this operation might fail");
        }

        private string GetPathToWallpaper(string path, bool isLiteral = true)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));

            var wallpaperFileSystemPath = isLiteral ? path : ResolvePathToWallpaper(path);

            if (File.Exists(wallpaperFileSystemPath)) return wallpaperFileSystemPath;

            throw new FileNotFoundException("Wallpaper file could not be found", wallpaperFileSystemPath);
        }

        private string ResolvePathToWallpaper(string path)
        {
            var paths = GetResolvedProviderPathFromPSPath(path, out ProviderInfo _);

            WriteVerbose($"PowerShell Path resolved to {paths.Count} file system path(s)");

            string pathToWallpaper = null;

            if (paths.Count > 0)
            {
                WriteWarning("Multiple paths were resolved from the PowerShell path provided, taking the first one");

                pathToWallpaper = paths[0];

                WriteVerbose($"Wallpaper path selected is: {pathToWallpaper}");
            }

            if (string.IsNullOrWhiteSpace(pathToWallpaper)) WriteWarning("No usable paths were resolved from the PowerShell Path provided");

            return pathToWallpaper;
        }
    }
}
