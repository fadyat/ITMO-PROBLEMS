using System.Collections.Generic;

namespace IsuExtra.Classes.Extra
{
    public class Faculty
    {
        private List<Lesson> _schedule;

        public Faculty(string tagFaculty, List<Lesson> facultySchedule)
        {
            Tag = tagFaculty;
            _schedule = facultySchedule;
        }

        public string Tag { get; }
    }
}