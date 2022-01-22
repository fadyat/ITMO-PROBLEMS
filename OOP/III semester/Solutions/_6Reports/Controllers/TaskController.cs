using System;
using System.Collections.Generic;
using System.Linq;
using lab_6.DAL.Data.Interfaces;
using lab_6.DAL.Data.Models;
using lab_6.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using Task = lab_6.DAL.Data.Models.Task;
using TaskStatus = lab_6.DAL.Data.Models.TaskStatus;

namespace lab_6.Controllers
{
    public class TaskController : Controller
    {
         private readonly ITaskRepository _taskRepository;
         private readonly IEmployeeRepository _employeeRepository;
        
        public TaskController(ITaskRepository taskRepository, IEmployeeRepository employeeRepository)
        {
            _taskRepository = taskRepository;
            _employeeRepository = employeeRepository;
        }
        public ViewResult Index()
        {
            return View();
        }
        [HttpPost]
        public string ChangeData(string newName, Guid id, string inputName)
        {
            switch (inputName)
            {
                case "Id":
                    return _taskRepository.ChangeData(InputName.Id, newName, id);
                case "Description":
                    return _taskRepository.ChangeData(InputName.Description, newName, id);
                case "Status":
                    return _taskRepository.ChangeData(InputName.Status, newName, id);
                case "CreationDate":
                    return _taskRepository.ChangeData(InputName.CreationDate, newName, id);
                case "ModifiedDate":
                    return _taskRepository.ChangeData(InputName.ModifiedDate, newName, id);
                case "Comment":
                    return _taskRepository.ChangeData(InputName.Comment, newName, id);
                case "EmployeeId":
                    return _taskRepository.ChangeData(InputName.EmployeeId, newName, id);
            }

            return "Ok";
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Task task)
        {
            if (ModelState.IsValid)
            {
                task.Status = TaskStatus.Open;
                DateTime now = DateTime.Now;
                task.CreationDate = 
                    new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                task.LastModifiedDate = task.CreationDate;
                _taskRepository.Create(task);
                TempData["SM"] = "Task successfully added!";
                return RedirectToAction("Index");
            }
            
            return View(new TasksViewModel(task));
        }
        
        [HttpGet]
        public ViewResult List(int? page, int filterStatusId=0, string filterEmployee="All")
        {
            List<TasksViewModel> tasksByStatus = _taskRepository.GetAll
                .Where(x => (int)x.Status == filterStatusId || filterStatusId == 0).Select(x => new TasksViewModel(x)).ToList();
            List<TasksViewModel> tasksByEmployee = _taskRepository.GetAll
                .Where(x => x.Employee?.Name == filterEmployee || filterEmployee == "All").Select(x => new TasksViewModel(x)).ToList();
            List<TasksViewModel> resultList = tasksByStatus.Intersect(tasksByEmployee, new TasksComparer()).ToList();
            var pageNumber = page ?? 1;
            var onePageOfTasks = resultList.ToPagedList(pageNumber, 3);
            var filtersByStatus = from TaskFilter d in Enum.GetValues(typeof(TaskFilter))
                select new { Id = (int)d, Name = d.ToString() };
            var filtersByEmployee = _employeeRepository.GetAll.ToList(); 
            ViewBag.FiltersByStatus = new SelectList(filtersByStatus , "Id", "Name");
            ViewBag.FiltersByEmployee = new SelectList(filtersByEmployee, "Name", "Name");
            ViewBag.SelectedStatusFilter = filterStatusId.ToString();
            ViewBag.SelectedEmployeeFilter = filterEmployee;
            ViewBag.onePageOfTasks = onePageOfTasks;
            return View(resultList);
        }
    }
}