using System;
using System.Collections.Generic;

namespace rNascar23.Replay
{
    public class EventReplay
    {
        public DateTime EventDate { get; set; }
        public string TrackName { get; set; }
        public string EventName { get; set; }
        public string Series { get; set; }
        public string EventType { get; set; }
        public int RunId { get; set; }
        public string Directory { get; set; }
        public IList<EventReplayFrame> Frames { get; set; } = new List<EventReplayFrame>();
    }
}
