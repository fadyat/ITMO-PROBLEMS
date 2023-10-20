using Isu.Classes;
using Isu.Exceptions;
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

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            GroupName name = new GroupName.GroupNameBuilder()
                .WithTag("M3")
                .WithCourseNumber(2)
                .WithGroupNumber("02")
                .Build();

            Group group = _isuService.AddGroup(name);
            Student alex = _isuService.AddStudent(group, "alex");
            Student tmpAlex = _isuService.FindStudent("alex");
            if (tmpAlex == null || !Equals(alex.GroupName, tmpAlex.GroupName))
            {
                Assert.Fail();
            }
        }

        [TestCase(1, "12", 1, "12")]
        [TestCase(0, "aa", 9, "bb")]
        [TestCase(0, "03", 5, "03")]
        [TestCase(11, "01", 11, "02")]
        [Test]
        public void AddingGroupCheckExisting_ThrowException(byte crs1, string n1, byte crs2, string n2)
        {
            Assert.Catch<IsuException>(() =>
            {
                GroupName name1 = new GroupName.GroupNameBuilder()
                    .WithTag("M3")
                    .WithCourseNumber(crs1)
                    .WithGroupNumber(n1)
                    .Build();

                GroupName name2 = new GroupName.GroupNameBuilder()
                    .WithTag("M3")
                    .WithCourseNumber(crs2)
                    .WithGroupNumber(n2)
                    .Build();

                _isuService.AddGroup(name1);
                _isuService.AddGroup(name2);
            });
        }

        [TestCase(1, "122")]
        [TestCase(123, "01")]
        [TestCase(2, "aa")]
        [Test]
        public void CreateGroupWithInvalidName_ThrowException(byte course, string groupNumber)
        {
            Assert.Catch<IsuException>(() =>
            {
                GroupName name = new GroupName.GroupNameBuilder()
                    .WithTag("M3")
                    .WithCourseNumber(course)
                    .WithGroupNumber(groupNumber)
                    .Build();

                _isuService.AddGroup(name);
            });
        }

        [TestCase(31)]
        [TestCase(77)]
        [Test]
        public void ReachMaxStudentPerGroup_ThrowException(int howMuchAdd)
        {
            Assert.Catch<IsuException>(() =>
            {
                GroupName name = new GroupName.GroupNameBuilder()
                    .WithTag("M3")
                    .WithCourseNumber(1)
                    .WithGroupNumber("02")
                    .Build();

                Group m3102 = _isuService.AddGroup(name);
                for (int i = 0; i < howMuchAdd; i++)
                {
                    _isuService.AddStudent(m3102, i + "Student");
                    m3102 = _isuService.GetGroup(m3102.Name);
                }
            });
        }


        [Test]
        public void TransferStudentToAnotherGroup()
        {
            GroupName name1 = new GroupName.GroupNameBuilder()
                .WithTag("M3")
                .WithCourseNumber(1)
                .WithGroupNumber("02")
                .Build();

            Group group1 = _isuService.AddGroup(name1);

            GroupName name2 = new GroupName.GroupNameBuilder()
                .WithTag("M3")
                .WithCourseNumber(1)
                .WithGroupNumber("03")
                .Build();

            Group group2 = _isuService.AddGroup(name2);
            Student student = _isuService.AddStudent(group1, "student1");
            group1 = _isuService.GetGroup(group1.Name);
            
            Assert.AreEqual(1, group1.Capacity);
            Assert.AreEqual(0, group2.Capacity);
            Assert.AreEqual(group1.Name, student.GroupName);

            student = _isuService.ChangeStudentGroup(student, group2);
            group1 = _isuService.GetGroup(group1.Name);
            group2 = _isuService.GetGroup(group2.Name);

            Assert.AreEqual(0, group1.Capacity);
            Assert.AreEqual(1, group2.Capacity);
            Assert.AreEqual(group2.Name, student.GroupName);
        }
    }
}