using SetWallpaper.COM;
using SetWallpaper.Output;
using System.Collections.Generic;
using System.Management.Automation;

namespace SetWallpaper
{
    [Cmdlet(VerbsCommon.Get, "Monitors")]
    public sealed class GetMonitors : Cmdlet
    {
        protected override void ProcessRecord()
        {
            var desktopWallpaper = (IDesktopWallpaper)new DesktopWallpaper();

            try
            {
                var monitors = new List<Monitor>();

                desktopWallpaper.GetMonitorDevicePathCount(out uint monitorsCount);

                for (uint monitorIndex = 0; monitorIndex < monitorsCount; monitorIndex++)
                {
                    desktopWallpaper.GetMonitorDevicePathAt(monitorIndex, out string monitorID);

                    var monitor = new Monitor(monitorIndex, monitorID);

                    monitors.Add(monitor);
                }

                WriteObject(monitors, enumerateCollection: true);
            }
            finally
            {
                desktopWallpaper.Release();
            }
        }
    }
}
