using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Classes;
using IsuExtra.Classes;
using IsuExtra.Exceptions;
using IsuExtra.Interfaces;

namespace IsuExtra.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly List<Schedule> _schedules;

        public ScheduleService()
        {
            _schedules = new List<Schedule>();
        }

        public Schedule AddSchedule(GroupName groupName, List<Lesson> lessons)
        {
            var schedule = new Schedule(groupName, lessons);
            if (CheckCrossingSchedule(schedule))
            {
                throw new ScheduleException("Some of schedule crossing with another!");
            }

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

            throw new ScheduleException("No such schedule!");
        }

        public void UpdateSchedule(Schedule schedule)
        {
            Schedule prevStatus = GetSchedule(schedule.GroupName);
            _schedules.Remove(prevStatus);
            _schedules.Add(schedule);
        }

        public bool CheckCrossingStream(StudentStream newStream)
        {
            return _schedules.SelectMany(schedule => schedule.ScheduleLessons)
                .Any(l1 => l1.CrossingTime(newStream.Info) &&
                           (l1.CrossingTeacher(newStream.Info) || l1.CrossingAuditory(newStream.Info)));
        }

        private bool CheckCrossingSchedule(Schedule newSchedule)
        {
            return _schedules.Any(schedule => schedule.ScheduleLessons
                    .Any(l1 => newSchedule.ScheduleLessons
                        .Any(l2 => l1.CrossingTime(l2) && (l1.CrossingAuditory(l2) || l1.CrossingTeacher(l2)))));
        }
    }
}