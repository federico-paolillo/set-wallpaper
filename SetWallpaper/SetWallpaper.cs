using SetWallpaper.COM;
using System.Management.Automation;

namespace SetWallpaper
{
    [Cmdlet(VerbsCommon.Set, "Wallpaper", SupportsShouldProcess = true)]
    public sealed class SetWallpaper : Cmdlet
    {
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            var shouldProcess = ShouldProcess(
                "Writing Garbage the Cmdlet will call a static function",
                "About to write 'Garbage', visible only if you added the -Debug switch",
                "Write Garbage ?",
                out ShouldProcessReason shouldProcessReason
            );

            if (shouldProcess)
            {
                var garbage = User32.Dummy();

                if (shouldProcessReason == ShouldProcessReason.WhatIf)
                {
                    WriteDebug("I would have written 'Garbage'");
                }
                else
                {
                    WriteDebug(garbage);
                }
            }
        }
    }
}
