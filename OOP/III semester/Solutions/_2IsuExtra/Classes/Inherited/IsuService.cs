using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Isu.Exceptions;
using IsuExtra.Classes.New;
using IsuExtra.Exceptions;
using IsuExtra.Interfaces;

namespace IsuExtra.Classes.Inherited
{
    public class IsuService : Isu.Classes.IsuService, IIsuService /* IChecks */
    {
        private ImmutableHashSet<Faculty> _totalFaculties;
        private ImmutableDictionary<Course, uint> _registeredCourses;
        private ImmutableDictionary<Student, ImmutableList<Course>> _studentPick;

        public IsuService()
        {
            _totalFaculties = ImmutableHashSet<Faculty>.Empty;
            _registeredCourses = ImmutableDictionary<Course, uint>.Empty;
            _studentPick = ImmutableDictionary<Student, ImmutableList<Course>>.Empty;
        }

        public Course AddCourse(string facultyTag, Lesson courseInfo, uint maxCapacity = 30)
        {
            var newCourse = new Course(facultyTag, courseInfo, maxCapacity);
            if (_registeredCourses.ContainsKey(newCourse))
                throw new Exception("This course is already exist!");

            _registeredCourses = _registeredCourses.Add(newCourse, 0);
            return newCourse;
        }

        public Faculty AddFaculty(string facultyTag, IEnumerable<Lesson> schedule)
        {
            var newFaculty = new Faculty(facultyTag, schedule.ToImmutableList());
            if (_totalFaculties.Contains(newFaculty))
                throw new IsuExtraException("This faculty is already exist!");

            _totalFaculties = _totalFaculties.Add(newFaculty);

            /* Faculty schedule crossing checks! */
            /* Faculties schedule crossing checks! */

            return newFaculty;
        }

        public void AddFacultyLessons(Faculty faculty, IEnumerable<Lesson> newLessons)
        {
            if (!_totalFaculties.Contains(faculty))
                throw new IsuExtraException("This faculty doesn't create!");

            ImmutableList<Lesson> newSchedule = faculty.Schedule.AddRange(newLessons);
            _totalFaculties = _totalFaculties.Remove(faculty);
            _totalFaculties = _totalFaculties.Add(new Faculty(faculty.Tag, newSchedule));

            /* Faculty schedule crossing checks! */
            /* Faculties schedule crossing checks! */
        }

        public void SubscribeCourse(Student student, Course subCourse)
        {
            SubscribeCourseCheck(student, subCourse);
            ImmutableList<Course> pickedCourses = _studentPick[student].Add(subCourse);
            _studentPick = _studentPick.Remove(student);
            _studentPick = _studentPick.Add(student, pickedCourses);
            uint pickCapacity = _registeredCourses[subCourse] + 1;
            _registeredCourses = _registeredCourses.Remove(subCourse);
            _registeredCourses = _registeredCourses.Add(subCourse, pickCapacity);
        }

        public void UnsubscribeCourse(Student student, Course unsubCourse)
        {
            UnsubscribeCourseCheck(student, unsubCourse);
            ImmutableList<Course> prevCourses = _studentPick[student].Remove(unsubCourse);
            _studentPick = _studentPick.Remove(student);
            _studentPick = _studentPick.Add(student, prevCourses);
            uint unsubCapacity = _registeredCourses[unsubCourse] - 1;
            _registeredCourses = _registeredCourses.Remove(unsubCourse);
            _registeredCourses = _registeredCourses.Add(unsubCourse, unsubCapacity);
        }

        private void SubscribeCourseCheck(Student student, Course pickedCourse)
        {
            if (!TotalStudents.Contains(student))
                throw new IsuExtraException("This student doesn't exist!");

            if (!_registeredCourses.ContainsKey(pickedCourse))
                throw new IsuExtraException("This course doesn't exist!");

            if (!_totalFaculties.Contains(new Faculty(student.FacultyTag, ImmutableList<Lesson>.Empty)))
                throw new IsuExtraException("This student faculty doesn't exist!");

            if (!_totalFaculties.Contains(new Faculty(pickedCourse.FacultyTag, ImmutableList<Lesson>.Empty)))
                throw new IsuExtraException("This course faculty doesn't exist!");

            if (_registeredCourses[pickedCourse] >= pickedCourse.MaxCapacity)
                throw new IsuExtraException("This course is already full!");

            if (Equals(student.Group.Name.FacultyTag, pickedCourse.FacultyTag))
                throw new IsuExtraException("Student can't pick his faculty course!");

            if (!_studentPick.ContainsKey(student))
                _studentPick = _studentPick.Add(student, ImmutableList<Course>.Empty);

            if (Equals(_studentPick[student].Count, 2))
                throw new IsuExtraException("Student can pick only 2 courses!");

            /* Crossing in faculty schedule */
            if (_totalFaculties.Where(faculty =>
                Equals(faculty.Tag, student.FacultyTag)).Any(studentFaculty =>
                studentFaculty.CrossingSchedule(pickedCourse.Info)))
            {
                throw new IsuExtraException("This course crossing with faculty schedule!");
            }

            /* Crossing teacher */
            if (_totalFaculties.Where(faculty =>
                !Equals(faculty.Tag, student.FacultyTag)).Any(studentFaculty =>
                studentFaculty.CrossingTeacher(pickedCourse.Info)))
            {
                throw new IsuException("This teacher is busy in that time!");
            }

            /* Crossing auditory */
            if (_totalFaculties.Where(faculty =>
                !Equals(faculty.Tag, student.FacultyTag)).Any(studentFaculty =>
                studentFaculty.CrossingAuditory(pickedCourse.Info)))
            {
                throw new IsuException("This auditory is busy in that time!");
            }

            /* Crossing in student schedule */
            if (_studentPick[student].Any(course =>
                pickedCourse.Info.CrossingTime(course.Info)))
            {
                throw new IsuExtraException("This course crossing with previous student courses!");
            }
        }

        private void UnsubscribeCourseCheck(Student student, Course unsubscribedCourse)
        {
            if (!TotalStudents.Contains(student))
                throw new IsuExtraException("This student doesn't exist!");

            if (!_registeredCourses.ContainsKey(unsubscribedCourse))
                throw new IsuExtraException("This course doesn't exist!");

            if (!_totalFaculties.Contains(new Faculty(student.FacultyTag, ImmutableList<Lesson>.Empty)))
                throw new IsuExtraException("This student faculty doesn't exist!");

            if (!_totalFaculties.Contains(new Faculty(unsubscribedCourse.FacultyTag, ImmutableList<Lesson>.Empty)))
                throw new IsuExtraException("This course faculty doesn't exist!");

            if (!_studentPick.ContainsKey(student))
                throw new IsuExtraException("This student don't pick any courses!");

            if (!_studentPick[student].Contains(unsubscribedCourse))
                throw new IsuExtraException("This student don't pick this course!");
        }

        /* private bool FacultyChecks(Faculty faculty) */
        /* private bool FacultiesChecks(Faculty faculty) */
    }
}