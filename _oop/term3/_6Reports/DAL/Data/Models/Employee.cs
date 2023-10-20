using System;
using System.ComponentModel.DataAnnotations;

namespace lab_6.DAL.Data.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Employee Leader { get; set; }
    }
}