using System;
using System.ComponentModel.DataAnnotations;

namespace lab_6.DAL.Data.Models
{
    public class Task
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Comment { get; set; }
        public Employee Employee { get; set; }
    }
}