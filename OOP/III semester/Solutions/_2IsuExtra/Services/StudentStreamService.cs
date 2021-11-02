using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Classes;

namespace IsuExtra.Services
{
    public class StudentStreamService
    {
        private readonly ScheduleService _schedules;
        private readonly List<StudentStream> _streams;
        private int _issuedStudentStreamId;

        public StudentStreamService(ScheduleService schedules)
        {
            _streams = new List<StudentStream>();
            _schedules = schedules;
            _issuedStudentStreamId = 100000;
        }

        public StudentStream CreateStream(Lesson lesson, int maxCapacity, List<Student> students)
        {
            var studentsId = new HashSet<uint>();
            foreach (Student student in students)
            {
                studentsId.Add(student.Id);
            }

            StudentStream newStream = new StudentStream.StudentStreamBuilder()
                .WithId(_issuedStudentStreamId++)
                .WithLesson(lesson)
                .WithMaxCapacity(maxCapacity)
                .WithCapacity(0)
                .Build();

            CheckCrossingStreams(newStream);
            _schedules.CheckCrossingStream(newStream);

            _streams.Add(newStream);
            return newStream;
        }

        public StudentStream GetStream(int id)
        {
            foreach (StudentStream stream in _streams
                .Where(stream => Equals(stream.Id, id)))
            {
                return stream;
            }

            throw new Exception(); // no such stream
        }

        public void UpdateStream(StudentStream newStream)
        {
            StudentStream prevStatus = GetStream(newStream.Id);
            _streams.Remove(prevStatus);
            _streams.Add(newStream);
        }

        public void CheckCrossingStreams(StudentStream newStream)
        {
            if (_streams.Any(stream => stream.Info.CrossingTime(newStream.Info) &&
                                       (stream.Info.CrossingTeacher(newStream.Info) ||
                                        stream.Info.CrossingAuditory(newStream.Info))))
            {
                throw new Exception(); // crossing
            }
        }
    }
}