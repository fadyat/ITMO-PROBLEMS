using System;
using lab_6.DAL.Data.Models;
using Task = lab_6.DAL.Data.Models.Task;

namespace lab_6.ViewModels
{
    public class TasksViewModel
    {
        public TasksViewModel()
        {
        }

        public TasksViewModel(Task task)
        {
            Id = task.Id;
            Description = task.Description;
            Status = task.Status;
            CreationDate = task.CreationDate;
            LastModifiedDate = task.LastModifiedDate;
            Comment = task.Comment;
            Employee = task.Employee;
        }
        public Guid Id { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Comment { get; set; }
        public Employee Employee { get; set; }
        
    }
}