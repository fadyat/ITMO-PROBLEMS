using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using IsuExtra.Classes;
using IsuExtra.Exceptions;
using IsuExtra.Interfaces;

namespace IsuExtra.Services
{
    public class StudentStreamService : IStudentStreamService
    {
        private readonly IScheduleService _schedules;
        private readonly List<StudentStream> _streams;
        private int _issuedStudentStreamId;

        public StudentStreamService(IScheduleService schedules)
        {
            _streams = new List<StudentStream>();
            _schedules = schedules;
            _issuedStudentStreamId = 100000;
        }

        public StudentStream CreateStream(Lesson lesson, int maxCapacity = 30)
        {
            StudentStream newStream = new StudentStream.StudentStreamBuilder()
                .WithId(_issuedStudentStreamId++)
                .WithLesson(lesson)
                .WithMaxCapacity(maxCapacity)
                .WithCapacity(0)
                .Build();

            if (CheckCrossingStreams(newStream))
            {
                throw new StudentStreamException("Stream crossing with another stream");
            }

            if (_schedules.CheckCrossingStream(newStream))
            {
                throw new ScheduleException("Stream crossing with some of schedule");
            }

            _streams.Add(newStream);
            return newStream;
        }

        public StudentStream GetStream(int id)
        {
            StudentStream stream = _streams.SingleOrDefault(stream => Equals(stream.Id, id));

            if (Equals(stream, null))
            {
                throw new StudentStreamException("No such stream");
            }

            return stream;
        }

        public void UpdateStream(StudentStream newStream)
        {
            StudentStream prevStatus = GetStream(newStream.Id);
            _streams.Remove(prevStatus);
            _streams.Add(newStream);
        }

        private bool CheckCrossingStreams(StudentStream newStream)
        {
            return _streams.Any(stream => stream.Info.CrossingTime(newStream.Info) &&
                                          (stream.Info.CrossingTeacher(newStream.Info) ||
                                           stream.Info.CrossingAuditory(newStream.Info)));
        }
    }
}