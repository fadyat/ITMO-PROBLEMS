using Isu.Classes;
using Isu.Exceptions;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IsuService _isuService;

        [SetUp]
        public void Setup() { _isuService = new IsuService(); }
        
        [TestCase(1, "12", 1, "12")]
        [TestCase(0, "aa", 9, "bb")]
        [TestCase(0, "03", 5, "03")]
        [TestCase(11, "01", 11, "02")]
        [Test]
        public void AddingGroupCheckExisting_ThrowException(byte crs1, string n1, byte crs2, string n2)
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup(new GroupName(new CourseNumber(crs1), n1));
                _isuService.AddGroup(new GroupName(new CourseNumber(crs2), n2));
            });
        }
        
        [TestCase(1,"122")]
        [TestCase(123, "01")]
        [TestCase(2, "aa")]
        [Test]
        public void CreateGroupWithInvalidName_ThrowException(byte course, string groupNumber)
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup(new GroupName(new CourseNumber(course), groupNumber));
            });
        }
        
        [TestCase(31)]
        [TestCase(77)]
        [Test]
        public void ReachMaxStudentPerGroup_ThrowException(int howMuchAdd)
        {
            Assert.Catch<IsuException>(() =>
            {
                Group m3102 = _isuService.AddGroup(new GroupName(new CourseNumber(1), "02"));
                for (int i = 0; i < howMuchAdd; i++)
                {
                    _isuService.AddStudent(m3102, i.ToString() + "Student");
                }
            });
        }
        
        /* In this test i'm not process Group, GroupName, CourseNumber, Student */ 
        [Test]
        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        public void TransferStudentToAnotherGroup_GroupChanged(bool studentExist, bool newGroupExist)
        {
            Assert.Catch<IsuException>(() =>
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
            });
        }
    }
}