using System.Collections.Generic;
using System.Linq;
using IsuExtra.Exceptions;

namespace IsuExtra.Classes
{
    public class ElectiveCourse
    {
        public ElectiveCourse(int id, string facultyTag, string name, List<StudentStream> streams)
        {
            Id = id;
            if (facultyTag.First() is < 'A' or > 'Z')
                throw new IsuExtraException("Faculty tag should begin with letter!");

            FacultyTag = facultyTag;
            Name = name;
            StreamsIds = new List<int>();
            foreach (StudentStream stream in streams)
            {
                StreamsIds.Add(stream.Id);
            }
        }

        public string FacultyTag { get; }

        public int Id { get; }

        public List<int> StreamsIds { get; }

        private string Name { get; }

        public override bool Equals(object obj)
        {
            return obj is ElectiveCourse item &&
                   Equals(Id, item.Id);
        }

        public override int GetHashCode()
        {
            return FacultyTag.GetHashCode();
        }

        public override string ToString()
        {
            return Name + " " + FacultyTag + " " + StreamsIds;
        }
    }
}