using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Classes;
using IsuExtra.Classes;

namespace IsuExtra.Services
{
    public class ScheduleService
    {
        private readonly List<Schedule> _schedules;

        public ScheduleService()
        {
            _schedules = new List<Schedule>();
        }

        public Schedule AddSchedule(GroupName groupName, List<Lesson> lessons)
        {
            var schedule = new Schedule(groupName, lessons);
            CheckCrossingSchedule(schedule);
            _schedules.Add(schedule);
            return schedule;
        }

        public Schedule GetSchedule(GroupName groupName)
        {
            foreach (Schedule schedule in _schedules
                .Where(schedule => Equals(schedule.GroupName, groupName)))
            {
                return schedule;
            }

            throw new Exception(); // No such schedule!
        }

        public void UpdateSchedule(Schedule schedule)
        {
            Schedule prevStatus = GetSchedule(schedule.GroupName);
            _schedules.Remove(prevStatus);
            _schedules.Add(schedule);
        }

        public void CheckCrossingSchedule(Schedule newSchedule)
        {
            if ((from schedule in _schedules
                from l1 in schedule.ScheduleLessons
                from l2 in newSchedule.ScheduleLessons
                where l1.CrossingTime(l2) &&
                      (l1.CrossingAuditory(l2) || l1.CrossingTeacher(l2))
                select l1).Any())
            {
                throw new Exception(); // crossing in schedules
            }
        }

        public void CheckCrossingStream(StudentStream newStream)
        {
            if (_schedules.SelectMany(schedule => schedule.ScheduleLessons)
                .Any(l1 => l1.CrossingTime(newStream.Info) &&
                           (l1.CrossingTeacher(newStream.Info) || l1.CrossingAuditory(newStream.Info))))
            {
                throw new Exception(); // some of schedules crossing with new stream
            }
        }
    }
}