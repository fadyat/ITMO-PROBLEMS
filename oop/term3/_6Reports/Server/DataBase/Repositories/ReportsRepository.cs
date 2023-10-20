using System;
using System.Collections.Generic;
using System.Linq;
using lab_6.DAL.Data.Interfaces;
using lab_6.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace lab_6.Server.DataBase.Repositories
{
    public class ReportsRepository : IReportRepository
    {
        private readonly ReportsDbContext _dbContext;

        public ReportsRepository(ReportsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Report> GetAll => _dbContext.Reports.Include(report => report.Employee);
        public Report FindById(Guid id) => _dbContext.Reports.Include(report => report.Employee)
            .FirstOrDefault(report => report.Id == id);

        public IEnumerable<Report> FindByEmployee(Employee employee) =>
            _dbContext.Reports.Where(report => report.Employee.Id == employee.Id).Include(report => report.Employee);

        public void Create(Report report)
        {
            _dbContext.Reports.Add(report);
            _dbContext.SaveChanges();
        }
        public void AddTask(Guid reportId, Task task)
        {
            _dbContext.Tasks.FirstOrDefault(x => x.Id == task.Id).ReportId = reportId;
            _dbContext.SaveChanges();
        }
    }
}