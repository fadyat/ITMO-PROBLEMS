using System;
using lab_6.DAL.Data.Models;

namespace lab_6.ViewModels
{
    public class ReportsViewModel
    {
        public ReportsViewModel()
        {
        }
        public ReportsViewModel(Report report)
        {
            Id = report.Id;
            Employee = report.Employee;
        }
        public Guid Id { get; set; }
        public Guid NewTaskId { get; set; }
        public Employee Employee { get; set; }
    }
}