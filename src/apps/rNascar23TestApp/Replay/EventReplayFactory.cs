using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace rNascar23.Replay
{
    internal static class EventReplayFactory
    {
        #region public

        public static IList<EventReplay> LoadEventReplays(string rootDirectory)
        {
            IList<EventReplay> replays = new List<EventReplay>();

            var replayRootDirectoryInfo = new DirectoryInfo(rootDirectory);

            foreach (DirectoryInfo replayDirectoryInfo in replayRootDirectoryInfo.EnumerateDirectories())
            {
                var replay = LoadEventReplay(replayDirectoryInfo);

                if (replay != null)
                    replays.Add(replay);
            }

            return replays;
        }

        public static EventReplay LoadEventReplay(DirectoryInfo replayDirectoryInfo)
        {
            // 2023-4-8.Bristol Motor Speedway Dirt-Cup-Toyota Owners 400-Race-9

            var replayDirectory = replayDirectoryInfo.Name;

            if (!replayDirectory.Contains("."))
                return null;

            if (!replayDirectory.Contains("-"))
                return null;

            var replayDirectoryMainSections = replayDirectory.Split('.');

            if (replayDirectoryMainSections.Length != 2)
                return null;

            var replayDirectorySubSections = replayDirectoryMainSections[1].Split('-');

            if (replayDirectorySubSections.Length != 5)
                return null;

            string dateFormat = "yyyy-M-d";
            DateTime eventDate = DateTime.MinValue;

            if (!DateTime.TryParseExact(
                replayDirectoryMainSections[0],
                dateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out eventDate))
            {
                return null;
            }

            EventReplay replay = new EventReplay()
            {
                EventDate = eventDate,
                TrackName = replayDirectorySubSections[0],
                Series = replayDirectorySubSections[1],
                EventName = replayDirectorySubSections[2],
                EventType = replayDirectorySubSections[3],
                RunId = int.Parse(replayDirectorySubSections[4]),
                Directory = replayDirectory,
                Frames = LoadEventReplayFrames(replayDirectoryInfo)
            };

            return replay;
        }

        private static IList<EventReplayFrame> LoadEventReplayFrames(DirectoryInfo replayDirectoryInfo)
        {
            // 14-3-5348-3-13-17-35-29.json.gz

            IList<EventReplayFrame> frames = new List<EventReplayFrame>();

            foreach (FileInfo replayFileInfo in replayDirectoryInfo.GetFiles("*.gz", SearchOption.TopDirectoryOnly))
            {
                var replayFileMainSections = replayFileInfo.Name.Split('.');

                var replayFileSubSections = replayFileMainSections[0].Split('-');

                if (replayFileSubSections.Length >= 8)
                {
                    var frameTimestamp = new DateTime(
                        DateTime.MinValue.Year,
                        DateTime.MinValue.Month,
                        DateTime.MinValue.Day,
                        int.Parse(replayFileSubSections[5]),
                        int.Parse(replayFileSubSections[6]),
                        int.Parse(replayFileSubSections[7]));

                    EventReplayFrame frame = new EventReplayFrame()
                    {
                        Timestamp = frameTimestamp,
                        FileName = replayFileInfo.FullName
                    };

                    frames.Add(frame);
                }

                frames = frames.OrderBy(f => f.Timestamp).ToList();

                for (int i = 0; i < frames.Count; i++)
                {
                    frames[i].Index = i;
                }
            }

            return frames;
        }
        #endregion
    }
}
