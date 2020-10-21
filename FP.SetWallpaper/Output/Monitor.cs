namespace FP.SetWallpaper.Output
{
    /// <summary>
    ///     Information about a Computer monitor installed in this System
    /// </summary>
    public sealed class Monitor
    {
        public Monitor(uint index, string id)
        {
            Index = index;
            Id = id;
        }

        public uint Index { get; }

        public string Id { get; }
    }
}