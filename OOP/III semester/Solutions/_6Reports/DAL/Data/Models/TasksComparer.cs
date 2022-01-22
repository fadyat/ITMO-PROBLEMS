using System.Collections.Generic;
using lab_6.ViewModels;

namespace lab_6.DAL.Data.Models
{
    public class TasksComparer : IEqualityComparer<TasksViewModel>
    {
        public bool Equals(TasksViewModel x, TasksViewModel y)
        {
            if (x == null || y == null) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(TasksViewModel obj)
        {
            return obj.Id.GetHashCode() ^ obj.Description.GetHashCode();
        }
    }
}