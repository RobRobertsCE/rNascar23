using System;
using System.Collections.Generic;

namespace rNascar23.Patches.Models
{
    public class PatchSet
    {
        public string Name { get; set; }
        public Version Version { get; set; }
        public string Author { get; set; }
        public string Stage { get; set; }
        public bool IsPreRelease { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Published { get; set; }
        public string ZipUrl { get; set; }
        public IList<AssetFile> Assets { get; set; } = new List<AssetFile>();
        public IList<PatchFile> Files { get; set; } = new List<PatchFile>();
    }
}
