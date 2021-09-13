using System;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
        }
        
        [TestCase(1, "12", 1, "12")] // exception
        [TestCase(1, "12", 1, "13")]
        [Test]
        public void AddingGroup_CheckExisting_ThrowException(byte course1, string name1, byte course2, string name2)
        {
            try
            {
                _isuService.AddGroup(new GroupName(new CourseNumber(course1), name1));
                _isuService.AddGroup(new GroupName(new CourseNumber(course2), name2));
            }
            finally {}
        }
        [TestCase(0)]
        [TestCase(30)]
        [TestCase(31)] // exception
        [TestCase(100)] // exception
        [Test]
        public void ReachMaxStudentPerGroup_ThrowException(int howMuchAdd)
        {
            var m3102 = new Group(new GroupName(new CourseNumber(1), "02"));
            for (int i = 0; i < howMuchAdd; i++) {
                _isuService.AddStudent(m3102, i.ToString() + "Student");
            }

            Assert.True(m3102.Capacity < m3102.MaxCapacity);
        }
        
        [TestCase(4,"12")]
        [TestCase(1,"122")] // exception
        [TestCase(123, "01")] // exception
        [Test]
        public void CreateGroupWithInvalidName_ThrowException(byte course, string groupNumber)
        {
            try
            {
                var courseNumber = new CourseNumber(course);
                var groupName = new GroupName(courseNumber, groupNumber);
                _isuService.AddGroup(groupName);
            }
            finally {}
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Assert.Catch<IsuException>(() =>
            {

            });
        }
    }
}