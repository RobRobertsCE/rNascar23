using rNascar23.Sdk.Common;

namespace rNascar23.ViewModels
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
        public VehicleStatusTypes VehicleStatus { get; set; }
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
                    case VehicleStatusTypes.OnTrack:
                        break;
                    case VehicleStatusTypes.InPits:
                        status = "Pits";
                        break;
                    case VehicleStatusTypes.Garage:
                        status = "Garage";
                        break;
                    case VehicleStatusTypes.Retired:
                        status = "Retired";
                        break;
                    case VehicleStatusTypes.VehicleEventStatus0:
                    case VehicleStatusTypes.VehicleEventStatus4:
                    case VehicleStatusTypes.VehicleEventStatus5:
                    case VehicleStatusTypes.VehicleEventStatus7:
                    case VehicleStatusTypes.VehicleEventStatus8:
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

