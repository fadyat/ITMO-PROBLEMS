using System;
using Isu.Services;

namespace Isu
{
    internal class Program
    {
        private static void Main()
        {
            var service = new IsuService();

            Group m3202 = service.AddGroup(new GroupName(new CourseNumber(2), "02"));

            // Group m3205 = service.AddGroup(new GroupName(new CourseNumber(2), "05"));
            service.AddStudent(new Group(new GroupName(new CourseNumber(2), "02")), "Artyom Fadeyev");

            // Student aE = service.AddStudent(m3202, "Artyom Ezichev");
            // Console.WriteLine();
            service.GroupInfo(new Group(new GroupName(new CourseNumber(2), "02")));

            // service.ChangeStudentGroup(aE, m3205);
            // service.Look(m3202);
            // service.Look(m3205);
        }
    }
}
