using System;
using System.Collections.Generic;
using System.Linq;
using lab_6.DAL.Data.Interfaces;
using lab_6.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace lab_6.Server.DataBase.Repositories
{
    public class TasksRepository : ITaskRepository
    {
        private readonly ReportsDbContext _dbContext;

        public TasksRepository(ReportsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task FindById(Guid id) => _dbContext.Tasks.Include(task => task.Employee)
                .FirstOrDefault(task => task.Id == id); 

        public Task FindByDateTime(DateTime dateTime) => _dbContext.Tasks.Include(task => task.Employee)
            .FirstOrDefault(task => task.CreationDate == dateTime);
        public IEnumerable<Task> GetAll => _dbContext.Tasks.Include(task => task.Employee);

        public IEnumerable<Task> FindByEmployee(Employee employee) =>
            _dbContext.Tasks.Where(task => task.Employee == employee).Include(task => task.Employee);
        
        public void Create(Task task)
        {
            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();
        }

        public string ChangeData(InputName inputName, string data, Guid Id)
        {
            var task = FindById(Id);
            switch (inputName)
            {
                case InputName.Id:
                    if (Guid.TryParse(data, out _))
                    {
                        task.Id = Guid.Parse(data);
                    }
                    else return "invalidData";
                    break;
                case InputName.Description:
                    task.Description = data;
                    break;
                case InputName.Status:
                {
                    if (Enum.TryParse(data, out TaskStatus result))
                    {
                        task.Status = result;
                    }
                    else return "invalidData";
                    break;
                }
                case InputName.CreationDate:
                {
                    if (DateTime.TryParse(data, out DateTime result))
                    {
                        task.CreationDate = result;
                    }
                    else return "invalidData";
                    break;
                }
                case InputName.ModifiedDate:
                {
                    if (DateTime.TryParse(data, out DateTime result))
                    {
                        task.CreationDate = result;
                    }
                    else return "invalidData";

                    break;
                }
                case InputName.Comment:
                    task.Comment = data;
                    break;
                case InputName.EmployeeId:
                    if (Guid.TryParse(data, out _))
                    {
                        Employee employee = _dbContext.Employees.FirstOrDefault(employee => employee.Id == Guid.Parse(data));
                        if (employee != null)
                        {
                            task.Employee = employee;
                        }
                        else return "invalidData";
                    }
                    else return "invalidData";
                    break;
            }
            _dbContext.SaveChanges();
            return "Ok";
        }
    }
}