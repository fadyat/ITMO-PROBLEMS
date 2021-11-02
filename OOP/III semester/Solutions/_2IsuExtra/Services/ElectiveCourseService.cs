using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Classes;

namespace IsuExtra.Services
{
    public class ElectiveCourseService
    {
        private readonly List<ElectiveCourse> _courses;
        private readonly StudentStreamService _streamService;
        private int _issuedCourseId;

        public ElectiveCourseService(StudentStreamService streamService)
        {
            _issuedCourseId = 100000;
            _streamService = streamService;
            _courses = new List<ElectiveCourse>();
        }

        public ElectiveCourse CreateElectiveCourse(
            string facultyTag,
            string name,
            List<StudentStream> streams)
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

            throw new Exception(); // no such course!
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