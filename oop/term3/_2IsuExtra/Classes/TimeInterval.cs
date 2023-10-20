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
        private TimeInterval(string dayOfWeek, int sHours, int sMinutes, int eHours, int eMinutes)
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

        public TimeIntervalBuilder ToBuilder()
        {
            TimeIntervalBuilder builder = new TimeIntervalBuilder()
                .WithDay(DayOfWeek)
                .WithStartHours(Start.Hour)
                .WithStartMinutes(Start.Minute)
                .WithEndHours(End.Hour)
                .WithEndMinutes(End.Minute);

            return builder;
        }

        public class TimeIntervalBuilder
        {
            private string _day;
            private int _startHours;
            private int _startMinutes;
            private int _endHours;
            private int _endMinute;

            public TimeIntervalBuilder WithDay(string day)
            {
                _day = day;
                return this;
            }

            public TimeIntervalBuilder WithStartHours(int startHours)
            {
                _startHours = startHours;
                return this;
            }

            public TimeIntervalBuilder WithStartMinutes(int startMinutes)
            {
                _startMinutes = startMinutes;
                return this;
            }

            public TimeIntervalBuilder WithEndHours(int endHours)
            {
                _endHours = endHours;
                return this;
            }

            public TimeIntervalBuilder WithEndMinutes(int endMinutes)
            {
                _endMinute = endMinutes;
                return this;
            }

            public TimeInterval Build()
            {
                var finallyVersion = new TimeInterval(
                    _day,
                    _startHours,
                    _startMinutes,
                    _endHours,
                    _endMinute);

                return finallyVersion;
            }
        }
    }
}