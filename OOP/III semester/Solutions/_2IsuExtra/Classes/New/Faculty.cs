using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Isu.Classes;

namespace IsuExtra.Classes.New
{
    public class Faculty
    {
        public Faculty(string tagFaculty)
        : this(tagFaculty, ImmutableDictionary<Group, ImmutableList<Lesson>>.Empty) { }

        public Faculty(string tagFaculty, Dictionary<Group, List<Lesson>> flow)
        {
            Tag = tagFaculty;
            Flows = ImmutableDictionary<Group, ImmutableList<Lesson>>.Empty;
            foreach ((Group grp, List<Lesson> schedule) in flow)
            {
                Flows = Flows.Add(grp, schedule.ToImmutableList());
            }
        }

        public Faculty(string tagFaculty, ImmutableDictionary<Group, ImmutableList<Lesson>> flow)
        {
            Tag = tagFaculty;
            Flows = flow;
        }

        public string Tag { get; }

        public ImmutableDictionary<Group, ImmutableList<Lesson>> Flows { get; }

        public bool CrossingSchedule(Group group, Lesson pickedCourse)
        {
            return Flows[group].Any(lesson => lesson.CrossingTime(pickedCourse));
        }

        public bool CrossingTeacher(Group group, Lesson pickedCourse)
        {
            return Flows[group].Any(lesson => lesson.CrossingTime(pickedCourse) &&
                                             lesson.CrossingTeacher(pickedCourse));
        }

        public bool CrossingAuditory(Group group, Lesson pickedCourse)
        {
            return Flows[group].Any(lesson => lesson.CrossingTime(pickedCourse) &&
                                             lesson.CrossingAuditory(pickedCourse));
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Faculty)obj;
            return other != null && Equals(Tag, other.Tag);
        }

        public override int GetHashCode() { return Tag.GetHashCode(); }

        public override string ToString()
        {
            string list = string.Empty;
            foreach ((Group group, ImmutableList<Lesson> lessons) in Flows)
            {
                list += "*" + group;
                list = lessons.Aggregate(list, (current, lesson) => current + (lesson + " "));
                list += '\n';
            }

            return Tag + "\n" + list;
        }
    }
}