namespace rNascar23.Service.Points.Data.Models
{
    internal class StageModel
    {
        public int race_id { get; set; }
        public int run_id { get; set; }
        public int stage_number { get; set; }
        public StagePointsModel[] results { get; set; }
    }
}
