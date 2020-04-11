using static System.Console;

namespace SetWallpaper.COM.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var desktopWallpaper = (IDesktopWallpaper)new DesktopWallpaper();

            desktopWallpaper.GetMonitorDevicePathCount(out uint monitorsCount);

            WriteLine($"There are {monitorsCount} monitor(s)");

            for (uint monitorIndex = 0; monitorIndex < monitorsCount; monitorIndex++)
            {
                desktopWallpaper.GetMonitorDevicePathAt(monitorIndex, out string monitorId);
                desktopWallpaper.GetWallpaper(monitorId, out string monitorWallpaper);

                WriteLine($"Monitor {monitorIndex} has wallpaper at {monitorWallpaper}");
            }

            ReadKey();
        }
    }
}
