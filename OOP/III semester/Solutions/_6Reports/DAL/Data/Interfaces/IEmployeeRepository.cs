using System;
using System.Collections.Generic;
using lab_6.DAL.Data.Models;

namespace lab_6.DAL.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll { get; }
        Employee GetById(Guid id);
        void Delete(Employee employee);
        void Create(Employee employee);
    }
}