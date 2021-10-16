using System;
using IsuExtra.Exceptions;

namespace IsuExtra.Classes.Extra
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

    public class Lesson
    {
        public Lesson(string lessonName, string dayOfWeek, (DateTime sTime, DateTime eTime) time, string teacher, uint auditory)
        {
            Name = lessonName;
            if (!Enum.IsDefined(typeof(Days), dayOfWeek))
                throw new IsuExtraException("This day of week doesn't exist!");

            DayOfWeek = dayOfWeek;
            (DateTime sTime, DateTime eTime) = time;
            Time = (sTime.ToString("H:mm"), eTime.ToString("H:mm"));
            Teacher = teacher;
            Auditory = auditory;
        }

        public string Name { get; }
        public string DayOfWeek { get; }
        public (string start, string end) Time { get; }
        public string Teacher { get; }
        public uint Auditory { get; }
    }
}