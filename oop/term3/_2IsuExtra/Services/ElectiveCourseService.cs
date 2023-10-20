using System.Collections.Generic;
using System.Linq;
using IsuExtra.Classes;
using IsuExtra.Exceptions;
using IsuExtra.Interfaces;

namespace IsuExtra.Services
{
    public class ElectiveCourseService : IElectiveCourseService
    {
        private readonly List<ElectiveCourse> _courses;
        private readonly IStudentStreamService _streamService;
        private int _issuedCourseId;

        public ElectiveCourseService(IStudentStreamService streamService)
        {
            _issuedCourseId = 100000;
            _streamService = streamService;
            _courses = new List<ElectiveCourse>();
        }

        public ElectiveCourse CreateElectiveCourse(string facultyTag, string name, List<StudentStream> streams)
        {
            var electiveCourse = new ElectiveCourse(
                _issuedCourseId++,
                facultyTag,
                name,
                streams);

            _courses.Add(electiveCourse);
            return electiveCourse;
        }

        public ElectiveCourse GetElectiveCourse(int id)
        {
            foreach (ElectiveCourse course in _courses
                .Where(course => Equals(course.Id, id)))
            {
                return course;
            }

            throw new ElectiveCourseException("No such course!");
        }

        public void UpdateElectiveCourse(ElectiveCourse electiveCourse)
        {
            ElectiveCourse prevStatus = GetElectiveCourse(electiveCourse.Id);
            _courses.Remove(prevStatus);
            _courses.Add(electiveCourse);
        }

        public List<StudentStream> GetStudentStreams(ElectiveCourse electiveCourse)
        {
            electiveCourse = GetElectiveCourse(electiveCourse.Id);
            var streams = electiveCourse.StreamsIds
                .Select(streamId => _streamService.GetStream(streamId))
                .ToList();

            return streams;
        }
    }
}