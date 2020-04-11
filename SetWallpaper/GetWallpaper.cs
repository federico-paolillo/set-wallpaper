using SetWallpaper.COM;
using SetWallpaper.Output;
using System.Collections.Generic;
using System.Management.Automation;

namespace SetWallpaper
{
    [Cmdlet(VerbsCommon.Get, "Wallpaper")]
    public class GetWallpaper : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        protected override void ProcessRecord()
        {
            var desktopWallpaper = (IDesktopWallpaper)new DesktopWallpaper();

            try
            {
                var wallpapers = new List<MonitorWallpaper>(Id.Length);

                foreach (var id in Id)
                {
                    desktopWallpaper.GetWallpaper(id, out string wallpaper);

                    var monitorWallpaper = new MonitorWallpaper(id, wallpaper);

                    wallpapers.Add(monitorWallpaper);
                }

                WriteObject(wallpapers, enumerateCollection: true);
            }
            finally
            {
                desktopWallpaper.Release();
            }
        }
    }
}
