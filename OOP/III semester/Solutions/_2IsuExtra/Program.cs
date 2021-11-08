using System;
using Isu.Classes;
using IsuExtra.Classes;
using IsuExtra.Services;
using IsuService = IsuExtra.Services.IsuService;
using Student = IsuExtra.Classes.Student;

namespace IsuExtra
{
    internal static class Program
    {
        private static void Main()
        {
            /*var scheduleService = new ScheduleService();
            var studentStreamService = new StudentStreamService(scheduleService);
            var service = new IsuService(studentStreamService);
            Group group1 = service.AddGroup(new GroupName.GroupNameBuilder()
                .WithTag("M3")
                .WithCourseNumber(2)
                .WithGroupNumber("12")
                .Build());

            Student student = service.AddStudent(group1, "12");
            Console.WriteLine(service.FindGroup(group1.Name)); */
        }
    }
}