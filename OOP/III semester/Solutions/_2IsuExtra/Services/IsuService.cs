using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Classes;
using IsuExtra.Exceptions;
using IsuExtra.Interfaces;

namespace IsuExtra.Services
{
    public class IsuService : Isu.Classes.IsuService, IIsuService
    {
        private readonly IStudentStreamService _streams;
        private readonly List<Student> _students;
        private uint _issuedStudentId;

        public IsuService(IStudentStreamService streams)
        {
            _streams = streams;
            _students = new List<Student>();
            _issuedStudentId = 100000;
        }

        public new Student AddStudent(Isu.Classes.Group group, string name)
        {
            group = GetGroup(group.Name);
            if (group.Capacity >= group.MaxCapacity)
                throw new Exception("Can't add student, full group!");

            group = new Isu.Classes.Group(group, group.Capacity + 1);
            UpdateGroup(group);

            var newStudent = new Student(name, _issuedStudentId++, group.Name);
            _students.Add(newStudent);
            return newStudent;
        }

        public new Student GetStudent(uint id)
        {
            foreach (Student student in _students
                .Where(student => Equals(student.Id, id)))
            {
                return student;
            }

            throw new IsuExtraException("Can't get student!");
        }

        public Student AddStudentToStream(Student student, StudentStream stream)
        {
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
            if (Equals(electiveCourse.FacultyTag, student.GroupName.FacultyTag))
            {
                throw new IsuExtraException("Student can't join his faculty course!");
            }

            if (!electiveCourse.StreamsIds
                .Any(streamId => student.PickedStreams.Contains(streamId)))
            {
                throw new StudentStreamException("Can't compare student and courses flows");
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
            var list = new List<Student>();
            foreach (int streamId in electiveCourse.StreamsIds.Where(streamId => Equals(streamId, stream.Id)))
            {
                list.AddRange(_students.Where(student => student.PickedCourses
                    .Contains(electiveCourse.Id) && student.PickedStreams.Contains(streamId)));
            }

            return list;
        }

        public List<Student> FindStudentsNotPickedElectiveCourse(Isu.Classes.GroupName groupName)
        {
            return _students.Where(student => Equals(student.GroupName, groupName) &&
                                              student.PickedCourses.Count != 2)
                .ToList();
        }

        private void UpdateStudent(Student student)
        {
            Student prevStatus = GetStudent(student.Id);
            _students.Remove(prevStatus);
            _students.Add(student);
        }
    }
}