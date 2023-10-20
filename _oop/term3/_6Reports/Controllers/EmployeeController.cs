using System;
using System.Collections.Generic;
using System.Linq;
using lab_6.DAL.Data.Interfaces;
using lab_6.DAL.Data.Models;
using lab_6.ViewModels;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace lab_6.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Leader = employee;
                _employeeRepository.Create(employee);
                TempData["SM"] = "Employee successfully added!";
                return RedirectToAction("Index");
            }
            
            return View(new EmployeesViewModel(employee));
        }

        [HttpGet]
        public ViewResult List(int? page)
        {
            List<EmployeesViewModel> employees = _employeeRepository.GetAll
                .Select(x => new EmployeesViewModel(x)).ToList();
            var pageNumber = page ?? 1;
            var onePageOfEmployees = employees.ToPagedList(pageNumber, 3);
            ViewBag.onePageOfEmployees = onePageOfEmployees;
            return View(employees);
        }
        
        public IActionResult Delete(Guid id)
        {
            Employee employee = _employeeRepository.GetById(id);
            _employeeRepository.Delete(employee);
            List<EmployeesViewModel> employees = _employeeRepository.GetAll
                .Select(x => new EmployeesViewModel(x)).ToList();
            return RedirectToAction("List");
        }
    }
}