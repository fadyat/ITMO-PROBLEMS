using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Isu.Classes;
using Isu.Exceptions;
using IsuExtra.Classes.New;
using IsuExtra.Exceptions;
using IsuExtra.Interfaces;

namespace IsuExtra.Classes.Inherited
{
    public class IsuService : Isu.Classes.IsuService, IIsuService
    {
        private const int MaxPickCourses = 2;
        private ImmutableHashSet<Faculty> _totalFaculties;
        private ImmutableDictionary<Course, uint> _registeredCourses;
        private ImmutableDictionary<uint, ImmutableList<Course>> _studentPick;

        public IsuService()
        {
            _totalFaculties = ImmutableHashSet<Faculty>.Empty;
            _registeredCourses = ImmutableDictionary<Course, uint>.Empty;
            _studentPick = ImmutableDictionary<uint, ImmutableList<Course>>.Empty;
        }

        public Course AddCourse(string facultyTag, Lesson courseInfo, uint maxCapacity = 30)
        {
            var newCourse = new Course(facultyTag, courseInfo, maxCapacity);
            if (_registeredCourses.ContainsKey(newCourse))
                throw new Exception("This course is already exist!");

            OtherFacultiesCrossingCheck(facultyTag, courseInfo);
            _registeredCourses = _registeredCourses.Add(newCourse, 0);
            return newCourse;
        }

        public Faculty AddFaculty(string facultyTag, Dictionary<Group, List<Lesson>> flow)
        {
            var newFaculty = new Faculty(facultyTag, flow);
            if (_totalFaculties.Contains(newFaculty))
                throw new IsuExtraException("This faculty is already exist!");

            _totalFaculties = _totalFaculties.Add(newFaculty);
            return newFaculty;
        }

        public void AddFacultyFlow(Faculty faculty, Dictionary<Group, List<Lesson>> flows)
        {
            if (!_totalFaculties.Contains(faculty))
                throw new IsuExtraException("This faculty doesn't create!");

            ImmutableDictionary<Group, ImmutableList<Lesson>> thisFlows = faculty.Flows;
            foreach ((Group group, List<Lesson> lst) in flows)
            {
                if (!faculty.Flows.ContainsKey(group))
                    thisFlows = thisFlows.Add(group, ImmutableList<Lesson>.Empty);

                ImmutableList<Lesson> groupSchedule = thisFlows[group];
                foreach (Lesson newLesson in lst)
                {
                    groupSchedule = groupSchedule.Add(newLesson);
                    YourFacultyCrossingCheck(faculty.Tag, group, newLesson);
                    OtherFacultiesCrossingCheck(faculty.Tag, newLesson);
                    thisFlows = thisFlows.Remove(group);
                    thisFlows = thisFlows.Add(group, groupSchedule);
                    _totalFaculties = _totalFaculties.Remove(faculty);
                    _totalFaculties = _totalFaculties.Add(new Faculty(faculty.Tag, thisFlows));
                }
            }
        }

        public void JoinCourse(Student student, Course subCourse)
        {
            JoinCourseCheck(student, subCourse);
            ImmutableList<Course> pickedCourses = _studentPick[student.Id].Add(subCourse);
            _studentPick = _studentPick.Remove(student.Id);
            _studentPick = _studentPick.Add(student.Id, pickedCourses);
            uint pickCapacity = _registeredCourses[subCourse] + 1;
            _registeredCourses = _registeredCourses.Remove(subCourse);
            _registeredCourses = _registeredCourses.Add(subCourse, pickCapacity);
        }

        public void LeaveCourse(Student student, Course unsubCourse)
        {
            LeaveCourseCheck(student, unsubCourse);
            ImmutableList<Course> pickedCourses = _studentPick[student.Id].Remove(unsubCourse);
            _studentPick = _studentPick.Remove(student.Id);
            _studentPick = _studentPick.Add(student.Id, pickedCourses);
            uint unsubCapacity = _registeredCourses[unsubCourse] - 1;
            _registeredCourses = _registeredCourses.Remove(unsubCourse);
            _registeredCourses = _registeredCourses.Add(unsubCourse, unsubCapacity);
        }

        public List<Group> GetFaculties(Course course)
        {
            return _studentPick.Keys
                .Select(GetStudent)
                .Select(currentStudent => currentStudent.Group).ToList();
        }

        public List<Student> StudentsOnCourse(Course course)
        {
            return (from id in _studentPick.Keys
            where _studentPick[id].Contains(course)
            select GetStudent(id)).ToList();
        }

        public List<Student> UnregisteredStudents(Group group)
        {
            return TotalStudents
                .Where(currentStudent => (!_studentPick.ContainsKey(currentStudent.Id)
                                          || _studentPick[currentStudent.Id].Count != MaxPickCourses)
                                         && Equals(currentStudent.Group, group)).ToList();
        }

        private void JoinCourseCheck(Student student, Course pickedCourse)
        {
            if (!TotalStudents.Contains(student))
                throw new IsuExtraException("This student doesn't exist!");

            if (!_registeredCourses.ContainsKey(pickedCourse))
                throw new IsuExtraException("This course doesn't exist!");

            if (!_totalFaculties.Contains(new Faculty(student.FacultyTag)))
                throw new IsuExtraException("This student faculty doesn't exist!");

            if (!_totalFaculties.Contains(new Faculty(pickedCourse.FacultyTag)))
                throw new IsuExtraException("This course faculty doesn't exist!");

            if (_registeredCourses[pickedCourse] >= pickedCourse.MaxCapacity)
                throw new IsuExtraException("This course is already full!");

            if (Equals(student.Group.Name.FacultyTag, pickedCourse.FacultyTag))
                throw new IsuExtraException("Student can't pick his faculty course!");

            if (!_studentPick.ContainsKey(student.Id))
                _studentPick = _studentPick.Add(student.Id, ImmutableList<Course>.Empty);

            if (Equals(_studentPick[student.Id].Count, MaxPickCourses))
                throw new IsuExtraException($"Student can pick only {MaxPickCourses} courses!");

            YourFacultyCrossingCheck(student.FacultyTag, student.Group, pickedCourse.Info);
            OtherFacultiesCrossingCheck(student.FacultyTag, pickedCourse.Info);

            /* Crossing in student schedule */
            if (_studentPick[student.Id].Any(course => pickedCourse.Info.CrossingTime(course.Info)))
            {
                throw new IsuExtraException("This course crossing with previous student courses!");
            }
        }

        private void LeaveCourseCheck(Student student, Course unsubscribedCourse)
        {
            if (!TotalStudents.Contains(student))
                throw new IsuExtraException("This student doesn't exist!");

            if (!_registeredCourses.ContainsKey(unsubscribedCourse))
                throw new IsuExtraException("This course doesn't exist!");

            if (!_totalFaculties.Contains(new Faculty(student.FacultyTag)))
                throw new IsuExtraException("This student faculty doesn't exist!");

            if (!_totalFaculties.Contains(new Faculty(unsubscribedCourse.FacultyTag)))
                throw new IsuExtraException("This course faculty doesn't exist!");

            if (!_studentPick.ContainsKey(student.Id))
                throw new IsuExtraException("This student don't pick any courses!");

            if (!_studentPick[student.Id].Contains(unsubscribedCourse))
                throw new IsuExtraException("This student don't pick this course!");
        }

        private void YourFacultyCrossingCheck(string facultyTag, Group group, Lesson pickedLesson)
        {
            /* For group: If lessons crossing time - wrong */
            if (_totalFaculties.Where(currentFaculty => Equals(currentFaculty.Tag, facultyTag))
                .SelectMany(myFaculty => myFaculty.Flows[group])
                .Any(pickedLesson.CrossingTime))
            {
                throw new IsuExtraException("This lesson crossing with group schedule!");
            }

            /* For Flow: If lessons have Equal(Lesson.DTime) && !Equal(Lesson) || crossingTime() - wrong */
            foreach (Faculty currentFaculty in _totalFaculties
                .Where(currentFaculty => Equals(currentFaculty.Tag, facultyTag)))
            {
                foreach ((Group currentGroup, ImmutableList<Lesson> currentSchedule) in currentFaculty.Flows)
                {
                    if (Equals(currentGroup, group)) continue;
                    if (currentSchedule
                        .Any(currentLesson => (Equals(currentLesson.DTime, pickedLesson.DTime) && !Equals(currentLesson, pickedLesson))
                                              || currentLesson.CrossingTime(pickedLesson)))
                    {
                        throw new IsuExtraException("This lesson crossing with flows schedule!");
                    }
                }
            }
        }

        private void OtherFacultiesCrossingCheck(string facultyTag, Lesson pickedLesson)
        {
            /* For Faculties: If crossing teacher - wrong */
            if (_totalFaculties
                .Where(currentFaculty => !Equals(currentFaculty.Tag, facultyTag))
                .SelectMany(currentFaculty => currentFaculty.Flows.Values)
                .Any(currentSchedule => currentSchedule.Any(pickedLesson.CrossingTeacher)))
            {
                throw new IsuException("This teacher is busy in that time!");
            }

            /* For Faculties: If crossing auditory - wrong */
            if (_totalFaculties
                .Where(currentFaculty => !Equals(currentFaculty.Tag, facultyTag))
                .SelectMany(currentFaculty => currentFaculty.Flows.Values)
                .Any(currentSchedule => currentSchedule.Any(pickedLesson.CrossingAuditory)))
            {
                throw new IsuException("This auditory is busy in that time!");
            }
        }
    }
}