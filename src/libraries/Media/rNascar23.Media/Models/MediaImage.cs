namespace rNascar23.Media.Models
{
    public class MediaImage
    {
        public MediaTypes MediaType { get; set; }
        public int SeriesId { get; set; }
        public int CarNumber { get; set; }
        public byte[] Image { get; set; }
    }
}
