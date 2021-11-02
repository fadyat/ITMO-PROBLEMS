using System;
using IsuExtra.Exceptions;

namespace IsuExtra.Classes
{
    /// <summary> Days of the week. </summary>
    public enum Days : uint
    {
        /// <summary> Sunday </summary>
        Sunday,

        /// <summary> Monday </summary>
        Monday,

        /// <summary> Tuesday </summary>
        Tuesday,

        /// <summary> Wednesday </summary>
        Wednesday,

        /// <summary> Thursday </summary>
        Thursday,

        /// <summary> Friday </summary>
        Friday,

        /// <summary> Saturday </summary>
        Saturday,
    }

    public class TimeInterval
    {
        public TimeInterval(string dayOfWeek, int sHours, int sMinutes, int eHours, int eMinutes)
        {
            if (!Enum.IsDefined(typeof(Days), dayOfWeek))
                throw new IsuExtraException("This day of week doesn't exist!");

            DayOfWeek = dayOfWeek;
            Start = new DateTime(1, 1, 1, sHours, sMinutes, 0);
            End = new DateTime(1, 1, 1, eHours, eMinutes, 0);

            if (Start.CompareTo(End) >= 0)
                throw new IsuExtraException("The start time can't be later than the end!");
        }

        private DateTime Start { get; }

        private DateTime End { get; }

        private string DayOfWeek { get; }

        public bool CrossingIntervals(TimeInterval secondInterval)
        {
            return End >= secondInterval.Start &&
                   secondInterval.End >= Start;
        }

        public bool CrossingDates(TimeInterval secondInterval)
        {
            return Equals(DayOfWeek, secondInterval.DayOfWeek);
        }

        public override bool Equals(object obj)
        {
            if (obj is not TimeInterval item)
            {
                return false;
            }

            return Equals(Start, item.Start) &&
                   Equals(End, item.End) &&
                   Equals(DayOfWeek, item.DayOfWeek);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End, DayOfWeek);
        }

        public override string ToString()
        {
            return DayOfWeek + " " + Start.ToString("H:mm") + " " + End.ToString("H:mm");
        }
    }
}