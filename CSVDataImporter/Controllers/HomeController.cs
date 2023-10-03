using CSVDataImporter.Models;
using CSVDataImporter.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CSVDataImporter.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public HomeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {
            List<Employee> employees = _employeeService.GetAll();
            return View(employees);
        }

        [HttpPost]
        public IActionResult Index(IFormFile file)
        {
            if (file == null)
                TempData["count"] = 0;
            else
            {
                int res = _employeeService.ImportFromCSV(file);
                TempData["count"] = res;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee? model = _employeeService.GetById(id);
            if (model == null)
                return NotFound();
            else
                return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Employee model)
        {
            int res = _employeeService.Edit(model);
            if (res > 0)
                return RedirectToAction(nameof(Index));
            else
                return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}