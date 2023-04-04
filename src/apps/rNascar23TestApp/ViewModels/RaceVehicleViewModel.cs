using rNasar23.Common;

namespace rNascar23TestApp.ViewModels
{
    internal class RaceVehicleViewModel
    {
        public int RunningPosition { get; set; }
        public string Driver { get; set; }
        public string CarNumber { get; set; }
        public string CarManufacturer { get; set; }
        public int LapsCompleted { get; set; }
        public float DeltaLeader { get; set; }
        public float DeltaNext { get; set; }
        public bool IsOnTrack { get; set; }
        public bool IsOnDvp { get; set; }
        public float LastLap { get; set; }
        public float BestLap { get; set; }
        public int BestLapNumber { get; set; }
        public int LastPit { get; set; }
        public VehicleEventStatus VehicleStatus { get; set; }
        public bool PersonalBestLapThisLap { get; set; }
        public bool FastestLapInRace { get; set; }
        public bool FastestCarThisLap { get; set; }
        public string StatusLabel
        {
            get
            {
                string status = string.Empty;

                switch (VehicleStatus)
                {
                    case VehicleEventStatus.OnTrack:
                        break;
                    case VehicleEventStatus.InPits:
                        status = "Pits";
                        break;
                    case VehicleEventStatus.Garage:
                        status = "Garage";
                        break;
                    case VehicleEventStatus.Retired:
                        status = "Retired";
                        break;
                    case VehicleEventStatus.VehicleEventStatus0:
                    case VehicleEventStatus.VehicleEventStatus4:
                    case VehicleEventStatus.VehicleEventStatus5:
                    case VehicleEventStatus.VehicleEventStatus7:
                    case VehicleEventStatus.VehicleEventStatus8:
                    default:
                        status = VehicleStatus.ToString();
                        break;
                }

                var dvp = IsOnDvp ? "DVP-" : "";

                return $"{dvp}{status}";
            }
        }
    }
}

