using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Classes;
using IsuExtra.Classes;
using IsuExtra.Services;
using NUnit.Framework;
using IsuService = IsuExtra.Services.IsuService;
using Student = IsuExtra.Classes.Student;

namespace IsuExtra.Tests
{
    public class IsuServiceTest
    {
        private StudentStreamService _streamService;
        private IsuService _isuService;
        private ScheduleService _scheduleService;
        private ElectiveCourseService _courseService;

        [SetUp]
        public void SetUp()
        {
            _scheduleService = new ScheduleService();
            _streamService = new StudentStreamService(_scheduleService);
            _isuService = new IsuService(_streamService);
            _courseService = new ElectiveCourseService(_streamService);

            // Correct Schedule for M3201
            _scheduleService.AddSchedule(
                new GroupName.GroupNameBuilder()
                    .WithTag("M3")
                    .WithCourseNumber(2)
                    .WithGroupNumber("01")
                    .Build(),
                new List<Lesson>
                {
                    new Lesson.LessonBuilder()
                        .WithAuditory(1)
                        .WithName("OOP")
                        .WithTeacher("Kek1")
                        .WithTime(new TimeInterval.TimeIntervalBuilder()
                            .WithDay("Monday")
                            .WithStartHours(10)
                            .WithStartMinutes(00)
                            .WithEndHours(11)
                            .WithEndMinutes(30)
                            .Build())
                        .Build()
                });
        }

        private static readonly object[] ScheduleCreationData =
        {
            new object[]
            {
                // Crossing lessons time in new schedule
                new List<Lesson>
                {
                    new Lesson.LessonBuilder()
                        .WithAuditory(2)
                        .WithName("OOP")
                        .WithTeacher("Kek2")
                        .WithTime(new TimeInterval.TimeIntervalBuilder()
                            .WithDay("Monday")
                            .WithStartHours(12)
                            .WithStartMinutes(00)
                            .WithEndHours(13)
                            .WithEndMinutes(30)
                            .Build())
                        .Build(),
                    new Lesson.LessonBuilder()
                        .WithAuditory(22)
                        .WithName("English")
                        .WithTeacher("Kek22")
                        .WithTime(new TimeInterval.TimeIntervalBuilder()
                            .WithDay("Monday")
                            .WithStartHours(12)
                            .WithStartMinutes(30)
                            .WithEndHours(13)
                            .WithEndMinutes(00)
                            .Build())
                        .Build()
                }
            },
            new object[]
            {
                // New schedule crossing M3201 schedule by time & teacher
                new List<Lesson>
                {
                    new Lesson.LessonBuilder()
                        .WithAuditory(3)
                        .WithName("OOP")
                        .WithTeacher("Kek1")
                        .WithTime(new TimeInterval.TimeIntervalBuilder()
                            .WithDay("Monday")
                            .WithStartHours(10)
                            .WithStartMinutes(10)
                            .WithEndHours(13)
                            .WithEndMinutes(30)
                            .Build())
                        .Build()
                }
            },
            new object[]
            {
                // New schedule crossing M3201 schedule by auditory & time
                new List<Lesson>
                {
                    new Lesson.LessonBuilder()
                        .WithAuditory(1)
                        .WithName("OOP")
                        .WithTeacher("Kek3")
                        .WithTime(new TimeInterval.TimeIntervalBuilder()
                            .WithDay("Monday")
                            .WithStartHours(10)
                            .WithStartMinutes(10)
                            .WithEndHours(13)
                            .WithEndMinutes(30)
                            .Build())
                        .Build()
                }
            }
        };

        [TestCaseSource(nameof(ScheduleCreationData))]
        public void ScheduleCrossingCheck_ThrowException(List<Lesson> lessons)
        {
            GroupName groupName = new GroupName.GroupNameBuilder()
                .WithTag("M3")
                .WithCourseNumber(2)
                .WithGroupNumber("02")
                .Build();

            Assert.Catch<Exception>(() =>
            {
                _scheduleService.AddSchedule(groupName, lessons);
            });
        }

        private static readonly object[] FlowsCreationData =
        {
            new object[]
            {
                // Crossing by auditory & time
                new Lesson.LessonBuilder()
                    .WithAuditory(1)
                    .WithName("OOP")
                    .WithTeacher("Kek2")
                    .WithTime(new TimeInterval.TimeIntervalBuilder()
                        .WithDay("Monday")
                        .WithStartHours(10)
                        .WithStartMinutes(00)
                        .WithEndHours(11)
                        .WithEndMinutes(30)
                        .Build())
                    .Build()
            },
            new object[]
            {
                // Crossing by teacher & time
                new Lesson.LessonBuilder()
                    .WithAuditory(2)
                    .WithName("OOP")
                    .WithTeacher("Kek1")
                    .WithTime(new TimeInterval.TimeIntervalBuilder()
                        .WithDay("Monday")
                        .WithStartHours(10)
                        .WithStartMinutes(00)
                        .WithEndHours(11)
                        .WithEndMinutes(30)
                        .Build())
                    .Build()
            },
            new object[]
            {
                // Crossing with another stream by time & auditory
                new Lesson.LessonBuilder()
                    .WithAuditory(3)
                    .WithName("OOP")
                    .WithTeacher("Kek4")
                    .WithTime(new TimeInterval.TimeIntervalBuilder()
                        .WithDay("Monday")
                        .WithStartHours(13)
                        .WithStartMinutes(00)
                        .WithEndHours(14)
                        .WithEndMinutes(30)
                        .Build())
                    .Build()
            },
            new object[]
            {
                // Crossing with another stream by time & teacher
                new Lesson.LessonBuilder()
                    .WithAuditory(4)
                    .WithName("OOP")
                    .WithTeacher("Kek3")
                    .WithTime(new TimeInterval.TimeIntervalBuilder()
                        .WithDay("Monday")
                        .WithStartHours(13)
                        .WithStartMinutes(00)
                        .WithEndHours(14)
                        .WithEndMinutes(30)
                        .Build())
                    .Build()
            }
        };

        [TestCaseSource(nameof(FlowsCreationData))]
        public void FlowCrossingCheck_ThrowException(Lesson flowLesson)
        {
            Lesson l = new Lesson.LessonBuilder()
                .WithAuditory(3)
                .WithName("OOP")
                .WithTeacher("Kek3")
                .WithTime(new TimeInterval.TimeIntervalBuilder()
                    .WithDay("Monday")
                    .WithStartHours(13)
                    .WithStartMinutes(00)
                    .WithEndHours(14)
                    .WithEndMinutes(30)
                    .Build())
                .Build();

            _streamService.CreateStream(l);

            Assert.Catch<Exception>(() =>
            {
                _streamService.CreateStream(flowLesson);
            });
        }

        [Test]
        public void StreamOptions()
        {
            Lesson l = new Lesson.LessonBuilder()
                .WithAuditory(3)
                .WithName("OOP")
                .WithTeacher("Kek3")
                .WithTime(new TimeInterval.TimeIntervalBuilder()
                    .WithDay("Monday")
                    .WithStartHours(13)
                    .WithStartMinutes(00)
                    .WithEndHours(14)
                    .WithEndMinutes(30)
                    .Build())
                .Build();

            GroupName m3201Name = new GroupName.GroupNameBuilder()
                .WithTag("M3")
                .WithCourseNumber(2)
                .WithGroupNumber("01")
                .Build();

            Group m3201 = _isuService.AddGroup(m3201Name);
            Student me1 = _isuService.AddStudent(m3201, "Me1");
            StudentStream stream = _streamService.CreateStream(l);
            me1 = _isuService.AddStudentToStream(me1, stream);
            Assert.True(me1.PickedStreams.Contains(stream.Id));
            stream = _streamService.GetStream(stream.Id);
            Assert.True(stream.Capacity == 1);
            me1 = _isuService.DeleteStudentFromStream(me1, stream);
            Assert.True(!me1.PickedStreams.Contains(stream.Id));
            stream = _streamService.GetStream(stream.Id);
            Assert.True(stream.Capacity == 0);
        }

        [Test]
        public void CourseOptions()
        {
            Lesson l = new Lesson.LessonBuilder()
                .WithAuditory(3)
                .WithName("OOP")
                .WithTeacher("Kek3")
                .WithTime(new TimeInterval.TimeIntervalBuilder()
                    .WithDay("Monday")
                    .WithStartHours(13)
                    .WithStartMinutes(00)
                    .WithEndHours(14)
                    .WithEndMinutes(30)
                    .Build())
                .Build();

            GroupName m3201Name = new GroupName.GroupNameBuilder()
                .WithTag("M3")
                .WithCourseNumber(2)
                .WithGroupNumber("01")
                .Build();

            Group m3201 = _isuService.AddGroup(m3201Name);
            Student me1 = _isuService.AddStudent(m3201, "Me1");
            StudentStream stream = _streamService.CreateStream(l);
            _isuService.AddStudentToStream(me1, stream);

            ElectiveCourse course = _courseService.CreateElectiveCourse("T3",
                "Course#1",
                new List<StudentStream> {stream});

            me1 = _isuService.AddStudentToElectiveCourse(me1, course);
            Assert.True(me1.PickedCourses.Contains(course.Id));
            me1 = _isuService.DeleteStudentFromCourse(me1, course);
            Assert.True(!me1.PickedCourses.Contains(course.Id));
        }
    }
}