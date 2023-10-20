using System;
using System.Collections.Generic;
using System.Linq;
using lab_6.DAL.Data.Interfaces;
using lab_6.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace lab_6.Server.DataBase.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ReportsDbContext _dbContext;

        public EmployeeRepository(ReportsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Employee> GetAll => _dbContext.Employees.Include(employee => employee.Leader);

        public Employee GetById(Guid id) => _dbContext.Employees.Include(employee => employee.Leader)
            .FirstOrDefault(employee => employee.Id == id);
        public void Delete(Employee employee)
        {
            _dbContext.Employees.Remove(employee);
            _dbContext.SaveChanges();
        }

        public void Create(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();
        }
    }
}