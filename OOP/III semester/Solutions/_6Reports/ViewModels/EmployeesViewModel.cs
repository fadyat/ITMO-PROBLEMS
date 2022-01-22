using System;
using System.ComponentModel.DataAnnotations;
using lab_6.DAL.Data.Models;

namespace lab_6.ViewModels
{
    public class EmployeesViewModel
    {
        public EmployeesViewModel()
        {
        }
        public EmployeesViewModel(Employee employee)
        {
            Id = employee.Id;
            Name = employee.Name;
            Leader = employee.Leader;
        }
        public Guid Id { get; set; }
        [Display(Name = "Enter name")]
        [MinLength(3)]
        [Required(ErrorMessage = "Lenght should be not less than 3")]
        public string Name { get; set; }
        public Employee Leader { get; set; }
    }
}