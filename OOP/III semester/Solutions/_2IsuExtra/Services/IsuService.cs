using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Classes;
using IsuExtra.Classes;

namespace IsuExtra.Services
{
    public class IsuService : Isu.Classes.IsuService
    {
        private readonly StudentStreamService _streams;

        public IsuService(StudentStreamService streams)
        {
            _streams = streams;
        }

        public Student AddStudentToStream(Student student, StudentStream stream)
        {
            if (student.PickedStreams.Count == 2)
            {
                throw new Exception(); // already reached pick of streams
            }

            student.JoinStream(stream.Id);
            UpdateStudent(student);

            stream = stream.ToBuilder()
                .WithCapacity(stream.Capacity + 1)
                .Build();
            _streams.UpdateStream(stream);

            return student;
        }

        public Student DeleteStudentFromStream(Student student, StudentStream stream)
        {
            student.LeaveStream(stream.Id);
            UpdateStudent(student);

            stream = stream.ToBuilder()
                .WithCapacity(stream.Capacity - 1)
                .Build();
            _streams.UpdateStream(stream);

            return student;
        }

        public Student AddStudentToElectiveCourse(Student student, ElectiveCourse electiveCourse)
        {
            if (student.PickedCourses.Count == 2)
            {
                throw new Exception(); // already reached pick of courses
            }

            if (Equals(electiveCourse.FacultyTag, student.GroupName.FacultyTag))
            {
                throw new Exception(); // student can't join his faculty course
            }

            bool correct = electiveCourse.StreamsIds
                .Any(streamId => student.PickedStreams.Contains(streamId));

            if (!correct)
            {
                throw new Exception(); // can't compare student and courses flows
            }

            student.JoinCourse(electiveCourse.Id);
            UpdateStudent(student);
            return student;
        }

        public Student DeleteStudentFromCourse(Student student, ElectiveCourse electiveCourse)
        {
            student.LeaveCourse(electiveCourse.Id);

            foreach (int streamsId in electiveCourse.StreamsIds
                .Where(streamsId => student.PickedStreams.Contains(streamsId)))
            {
                student.LeaveStream(streamsId);
            }

            UpdateStudent(student);
            return student;
        }

        public List<Student> FindStudents(ElectiveCourse electiveCourse, StudentStream stream)
        {
            return (from streamId in electiveCourse.StreamsIds
                where Equals(streamId, stream.Id)
                from student in Students
                where student.PickedCourses.Contains(electiveCourse.Id) &&
                      student.PickedStreams.Contains(streamId)
                select student).ToList();
        }

        public List<Student> FindStudentsNotPickedElectiveCourse(GroupName groupName)
        {
            return Students.Where(student => Equals(student.GroupName, groupName) &&
                                             student.PickedCourses.Count != 2)
                .ToList();
        }
    }
}