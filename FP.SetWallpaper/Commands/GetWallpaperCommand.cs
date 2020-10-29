using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Runtime.InteropServices;
using FP.SetWallpaper.Output;
using FP.SetWallpaper.COM;

namespace FP.SetWallpaper.Commands
{
    [Cmdlet(VerbsCommon.Get, "Wallpaper", DefaultParameterSetName = BY_ID)]
    [OutputType(typeof(Wallpaper))]
    public class GetWallpaperCommand : PSCmdlet
    {
        private const string BY_INPUT_OBJECT = "ByInputObject";
        private const string BY_ID = "ById";

        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true,
            ParameterSetName = BY_ID)]
        public string[] Id { get; set; }

        //-InputObject parameter implementation is strongly suggested
        //According to guidelines: [...] a .NET Framework object is often available that exactly matches the type the user needs to perform a particular operation. InputObject is the standard name for a parameter that takes such an object as input.

        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = BY_INPUT_OBJECT)]
        public Monitor[] InputObject { get; set; }

        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case BY_INPUT_OBJECT:
                    ProcessRecordByInputObject();
                    break;
                default:
                    ProcessRecordById();
                    break;
            }
        }

        private void ProcessRecordById()
        {
            WriteVerbose($"Processing record using {BY_ID} parameter set");

            GetWallpapers(Id);
        }

        private void ProcessRecordByInputObject()
        {
            WriteVerbose($"Processing record using {BY_INPUT_OBJECT} parameter set");

            var ids = InputObject
                .Select(monitor => monitor.Id)
                .ToList();

            GetWallpapers(ids);
        }

        private void GetWallpapers(IEnumerable<string> ids)
        {
            if (ids is null) throw new ArgumentNullException(nameof(ids));

            WriteVerbose("Getting IDesktopWallpaper COM interface");

            IDesktopWallpaper desktopWallpaper = null;

            //Failure to get the IDesktopWallpaper COM object renders impossible any further processing

            try
            {
                desktopWallpaper = (IDesktopWallpaper) new DesktopWallpaper();
            }
            catch (COMException comEx)
            {
                var errorRecord = comEx.ToErrorRecord();

                ThrowTerminatingError(errorRecord);

                //Releasing IDesktopWallpaper here is meaningless as it will be null because we had a failure during initialization
            }

            var wallpapers = new List<Wallpaper>();

            foreach (var id in ids)
                //Failure to run an IDesktopWallpaper method *might* be temporary, we can continue trying with any other Monitor id left

                try
                {
                    WriteVerbose($"Getting wallpaper for monitor {id}");

                    desktopWallpaper.GetWallpaper(id, out var path);

                    //This can occur if, for example, your wallpaper gets deleted and you install a new Windows 10 Insider Preview build.

                    if (string.IsNullOrWhiteSpace(path)) WriteVerbose($"Monitor {id} has no wallpaper set");

                    var wallpaper = new Wallpaper(id, path);

                    wallpapers.Add(wallpaper);
                }
                catch (COMException comEx)
                {
                    var errorRecord = comEx.ToErrorRecord(id);

                    WriteError(errorRecord);
                }

            //Releasing IDesktopWallpaper can be done directly here outside a finally, any failure occured before getting here did not stop the Program from executing

            desktopWallpaper.Release();

            WriteObject(wallpapers, true);
        }
    }
}