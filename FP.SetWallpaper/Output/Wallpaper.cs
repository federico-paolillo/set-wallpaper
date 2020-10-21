namespace FP.SetWallpaper.Output
{
    /// <summary>
    ///     A Wallpaper currently set on a Monitor
    /// </summary>
    public class Wallpaper
    {
        public Wallpaper(string monitor, string path)
        {
            Monitor = monitor;
            Path = path;
        }

        public string Monitor { get; }

        public string Path { get; }
    }
}