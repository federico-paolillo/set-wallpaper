namespace SetWallpaper.Output
{
    /// <summary>
    /// Information about a Computer monitor installed in this System
    /// </summary>
    public sealed class Monitor
    {
        public uint Index { get; private set; }

        public string Id { get; private set; }

        public Monitor(uint index, string id)
        {
            Index = index;
            Id = id;
        }
    }
}
