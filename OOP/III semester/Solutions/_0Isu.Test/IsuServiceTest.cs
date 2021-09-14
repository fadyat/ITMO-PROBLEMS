using Isu.Classes;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IsuService _isuService;

        [SetUp]
        public void Setup() { _isuService = new IsuService(); }
        
        [TestCase(1, "12", 1, "12")] // exception
        [TestCase(1, "12", 1, "13")]
        [TestCase(0, "aa", 9, "bb")] // exception
        [TestCase(0, "03", 5, "03")] // exception
        [Test]
        public void AddingGroupCheckExisting_ThrowException(byte crs1, string n1, byte crs2, string n2)
        {
            _isuService.AddGroup(new GroupName(new CourseNumber(crs1), n1));
            _isuService.AddGroup(new GroupName(new CourseNumber(crs2), n2));
        }
        
        [TestCase(4,"12")]
        [TestCase(1,"122")] // exception
        [TestCase(123, "01")] // exception
        [TestCase(2, "aa")] // exception
        [Test]
        public void CreateGroupWithInvalidName_ThrowException(byte course, string groupNumber)
        {
            _isuService.AddGroup(new GroupName(new CourseNumber(course), groupNumber));
        }
        
        [TestCase(0)]
        [TestCase(30)]
        [TestCase(31)] // exception
        [TestCase(77)] // exception
        [Test]
        public void ReachMaxStudentPerGroup_ThrowException(int howMuchAdd)
        {
            Group m3102 = _isuService.AddGroup(new GroupName(new CourseNumber(1), "02"));
            for (int i = 0; i < howMuchAdd; i++) {
                _isuService.AddStudent(m3102, i.ToString() + "Student");
            }
        }
        
        /* In this test i'm not process Group, GroupName, CourseNumber, Student */ 
        [Test]
        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void TransferStudentToAnotherGroup_GroupChanged(bool studentExist, bool newGroupExist)
        {
            /* it's impossible to create to students with equal id */
            _isuService.AddGroup(new GroupName(new CourseNumber(1), "02"));

            Student alex = _isuService.AddStudent(new Group(new GroupName(new CourseNumber(1), "02")), "Alex");
            if (!studentExist)
                alex = new Student("RandomNameOfStudent", 123123);
            
            var newGroup = new Group(new GroupName(new CourseNumber(1), "03"));
            if (newGroupExist)
                _isuService.AddGroup(new GroupName(new CourseNumber(1), "03"));
            
            _isuService.ChangeStudentGroup(alex, newGroup);
        }
    }
}