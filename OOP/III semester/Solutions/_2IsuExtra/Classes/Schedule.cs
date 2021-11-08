using System;
using System.Collections.Generic;
using Isu.Classes;
using IsuExtra.Exceptions;

namespace IsuExtra.Classes
{
    public class Schedule
    {
        public Schedule(GroupName groupName, List<Lesson> lessons)
        {
            GroupName = groupName;
            ScheduleLessons = lessons;
            CheckSchedule();
        }

        public GroupName GroupName { get; }

        public List<Lesson> ScheduleLessons { get; }

        private void CheckSchedule()
        {
            int n = ScheduleLessons.Count;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (ScheduleLessons[i].CrossingTime(ScheduleLessons[j]))
                    {
                        throw new ScheduleException("Incorrect data!");
                    }
                }
            }
        }
    }
}