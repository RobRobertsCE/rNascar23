using System;

namespace rNascar23.Patches.Models
{
    public class AssetFile
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int Size { get; set; }
        public int DownloadCount { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
