using rNascar23.ViewModels;
using System;

namespace rNascar23.Logic
{
    internal class DayOfWeekHelper
    {
        /// <summary>
        /// Schedule range should go from a Monday to a Sunday
        /// </summary>
        /// <param name="scheduleType">Type of schedule range to calculate</param>
        /// <returns>Start and end dates for a given range</returns>
        public static ScheduleRange GetScheduleRange(ScheduleType scheduleType)
        {
            return GetScheduleRangeByDate(scheduleType, DateTime.Today);
        }

        private static ScheduleRange GetScheduleRangeByDate(ScheduleType scheduleType, DateTime targetDate)
        {
            switch (scheduleType)
            {
                case ScheduleType.ThisWeek:
                    {
                        if (targetDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            return new ScheduleRange
                            {
                                Start = targetDate.AddDays(-6),
                                End = targetDate
                            };
                        }
                        else if (targetDate.DayOfWeek == DayOfWeek.Monday)
                        {
                            return new ScheduleRange
                            {
                                Start = targetDate,
                                End = targetDate.AddDays(6)
                            };
                        }
                        else
                        {
                            int daysAheadOffset = 7 - ((int)targetDate.DayOfWeek);
                            int daysBehindOffset = (int)targetDate.DayOfWeek - 1;

                            return new ScheduleRange
                            {
                                Start = targetDate.AddDays(-daysBehindOffset),
                                End = targetDate.AddDays(daysAheadOffset)
                            };
                        }
                    }
                case ScheduleType.NextWeek:
                    {
                        targetDate = targetDate.AddDays(7);

                        if (targetDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            return new ScheduleRange
                            {
                                Start = targetDate.AddDays(-6),
                                End = targetDate
                            };
                        }
                        else if (targetDate.DayOfWeek == DayOfWeek.Monday)
                        {
                            return new ScheduleRange
                            {
                                Start = targetDate,
                                End = targetDate.AddDays(6)
                            };
                        }
                        else
                        {
                            int daysAheadOffset = 7 - ((int)targetDate.DayOfWeek);
                            int daysBehindOffset = (int)targetDate.DayOfWeek - 1;

                            return new ScheduleRange
                            {
                                Start = targetDate.AddDays(-daysBehindOffset),
                                End = targetDate.AddDays(daysAheadOffset)
                            };
                        }
                    }
                case ScheduleType.Today:
                    {
                        return new ScheduleRange
                        {
                            Start = targetDate.Date,
                            End = targetDate.Date
                        };
                    }
                default:
                    {
                        return new ScheduleRange();
                    }
            }
        }
    }

    internal class ScheduleRange
    {
        public DateTime Start { get; set; } = DateTime.MinValue;
        public DateTime End { get; set; } = DateTime.MinValue;
    }
}
