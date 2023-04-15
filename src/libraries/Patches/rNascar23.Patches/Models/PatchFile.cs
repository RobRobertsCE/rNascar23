using System;

namespace rNascar23.Patches.Models
{
    public class PatchFile
    {
        public Version FileVersion { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public string RelativeUri { get; set; }
        public long Size { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
