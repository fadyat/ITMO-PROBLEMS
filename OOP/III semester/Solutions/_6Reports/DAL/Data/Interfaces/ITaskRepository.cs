using System;
using System.Collections.Generic;
using lab_6.DAL.Data.Models;

namespace lab_6.DAL.Data.Interfaces
{
    public interface ITaskRepository
    {
        Task FindById(Guid id);
        Task FindByDateTime(DateTime dateTime);
        IEnumerable<Task> GetAll { get; }
        IEnumerable<Task> FindByEmployee(Employee employee);
        void Create(Task task);
        string ChangeData(InputName inputName, string data, Guid id);
    }
}