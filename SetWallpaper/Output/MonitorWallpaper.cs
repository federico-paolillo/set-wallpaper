namespace SetWallpaper.Output
{
    /// <summary>
    /// A Wallpaper currently set on a Monitor
    /// </summary>
    public class MonitorWallpaper
    {
        public string Monitor { get; private set; }

        public string Wallpaper { get; private set; }

        public MonitorWallpaper(string monitor, string wallpaper)
        {
            Monitor = monitor;
            Wallpaper = wallpaper;
        }
    }
}
