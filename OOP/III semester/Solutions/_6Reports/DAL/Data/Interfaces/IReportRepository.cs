using System;
using System.Collections.Generic;
using lab_6.DAL.Data.Models;

namespace lab_6.DAL.Data.Interfaces
{
    public interface IReportRepository
    {
        Report FindById(Guid id);
        IEnumerable<Report> GetAll { get; }
        IEnumerable<Report> FindByEmployee(Employee employee);
        void Create(Report report);
        void AddTask(Guid reportId, Task task);
    }
}