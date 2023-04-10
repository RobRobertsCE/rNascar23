namespace rNascar23.Patches.Models
{
    public class PatchChange
    {
        public string Name { get; set; }
        public CurrentAssemblyInfo Current { get; set; }
        public PatchFile Patch { get; set; }
        public PatchAction Action { get; set; }
    }
}
