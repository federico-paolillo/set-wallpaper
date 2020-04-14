namespace SetWallpaper.Output
{
    /// <summary>
    /// A Wallpaper currently set on a Monitor
    /// </summary>
    public class Wallpaper
    {
        public string Monitor { get; private set; }

        public string Path { get; private set; }

        public Wallpaper(string monitor, string path)
        {
            Monitor = monitor;
            Path = path;
        }
    }
}
