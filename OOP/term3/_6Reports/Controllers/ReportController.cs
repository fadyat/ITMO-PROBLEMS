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
    public class ReportController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IReportRepository _reportRepository;
        
        public ReportController(
            ITaskRepository taskRepository,
            IEmployeeRepository employeeRepository,
            IReportRepository reportRepository)
        {
            _taskRepository = taskRepository;
            _employeeRepository = employeeRepository;
            _reportRepository = reportRepository;
        }
        
        public ViewResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddTask()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTask(ReportsViewModel report)
        {
            if (ModelState.IsValid &&
                _taskRepository.FindById(report.NewTaskId) != null &&
                _reportRepository.FindById(report.Id) != null)
            {
                _reportRepository.AddTask(report.Id, _taskRepository.FindById(report.NewTaskId));
                TempData["SM"] = "Task successfully added!";
                return RedirectToAction("Index");
            }
            TempData["EM"] = "No such task or report";
            return View(report);
        }

        [HttpGet]
        public IActionResult GetReportId()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetReportId(Report report)
        {
            if (ModelState.IsValid && _reportRepository.FindById(report.Id) != null)
            {
                return RedirectToAction("TasksList", new {reportId = report.Id});
            }
            TempData["EM"] = "No such report";
            return View(new ReportsViewModel(report));
        }
        [HttpGet]
        public ViewResult TasksList(int? page, Guid reportId)
        {
            var a = _reportRepository.FindById(reportId);
            List<TasksViewModel> tasks = _taskRepository.GetAll
                .Where(x => x.ReportId == reportId).Select(x => new TasksViewModel(x)).ToList();
            var pageNumber = page ?? 1;
            var onePageOfTasks = tasks.ToPagedList(pageNumber, 3);
            ViewBag.onePageOfTasks = onePageOfTasks;
            return View(tasks);
        }
        
        [HttpGet]
        public ViewResult List(int? page)
        {
            List<ReportsViewModel> reports = _reportRepository.GetAll
                .Select(x => new ReportsViewModel(x)).ToList();
            var pageNumber = page ?? 1;
            var onePageOfReports = reports.ToPagedList(pageNumber, 3);
            ViewBag.onePageOfReports = onePageOfReports;
            return View(reports);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Report report)
        {
            if (ModelState.IsValid && _employeeRepository.GetById(report.Employee.Id) != null)
            {
                _reportRepository.Create(new Report()
                {
                    Id = Guid.NewGuid(),
                    Employee = _employeeRepository.GetById(report.Employee.Id)
                });
                TempData["SM"] = "Report successfully added!";
                return RedirectToAction("Index");
            }

            TempData["EM"] = "No such employee";
            return View(new ReportsViewModel(report));
        }
    }
}