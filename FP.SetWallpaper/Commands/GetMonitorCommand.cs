using System.Collections.Generic;
using System.Management.Automation;
using System.Runtime.InteropServices;
using FP.SetWallpaper.Output;
using FP.SetWallpaper.COM;

namespace FP.SetWallpaper.Commands
{
    [Cmdlet(VerbsCommon.Get, "Monitor")]
    [OutputType(typeof(Monitor))]
    public sealed class GetMonitorCommand : Cmdlet
    {
        //Instead of using ProcessRecord we must use BeginProcessing because this Cmdlet does not accept input
        //According to guidelines: If your cmdlet does not accept input from the pipeline, processing should be implemented in the System.Management.Automation.Cmdlet.BeginProcessing method. 

        protected override void BeginProcessing()
        {
            IDesktopWallpaper desktopWallpaper = null;

            try
            {
                WriteVerbose("Getting IDesktopWallpaper COM interface");

                desktopWallpaper = (IDesktopWallpaper) new DesktopWallpaper();

                var monitors = new List<Monitor>();

                WriteVerbose("Getting monitors count");

                desktopWallpaper.GetMonitorDevicePathCount(out var monitorsCount);

                WriteVerbose($"Found {monitorsCount} monitor(s)");

                for (uint monitorIndex = 0; monitorIndex < monitorsCount; monitorIndex++)
                {
                    WriteVerbose($"Getting monitor id for monitor {monitorIndex}");

                    desktopWallpaper.GetMonitorDevicePathAt(monitorIndex, out var monitorID);

                    var monitor = new Monitor(monitorIndex, monitorID);

                    monitors.Add(monitor);
                }

                WriteObject(monitors, true);
            }
            catch (COMException comEx)
            {
                var errorRecord = comEx.ToErrorRecord();

                //This Cmdlet does not accept any input therefore any Exception that occurs results in a terminating error.
                //Accoring to guidelines: An error is a terminating error if it occurs in a cmdlet that does not accept or return an object or if it occurs in a cmdlet that accepts or returns only one object

                ThrowTerminatingError(errorRecord);
            }
            finally
            {
                desktopWallpaper.Release();
            }
        }
    }
}