using System.Collections.Generic;
using Isu.Classes;
using IsuExtra.Classes;

namespace IsuExtra.Interfaces
{
    public interface IScheduleService
    {
        Schedule AddSchedule(GroupName groupName, List<Lesson> lessons);

        Schedule GetSchedule(GroupName groupName);

        public void UpdateSchedule(Schedule schedule);

        bool CheckCrossingStream(StudentStream newStream);
    }
}