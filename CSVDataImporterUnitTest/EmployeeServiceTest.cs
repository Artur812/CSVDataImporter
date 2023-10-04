using CSVDataImporter.Models;
using CSVDataImporter.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CSVDataImporterUnitTest
{
    public class EmployeeServiceTest
    {
        private readonly EmployeeService _employeeService;
        public EmployeeServiceTest()
        {
            _employeeService = DependencyInjection.GetRequiredService<EmployeeService>() ?? throw new ArgumentNullException(nameof(EmployeeService));
        }

        [Fact]
        public void ImportFromCSV()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "files/dataset.csv");
            using var stream = new MemoryStream(File.ReadAllBytes(path).ToArray());
            var formFile = new FormFile(stream, 0, stream.Length, "streamFile", path.Split(@"\").Last());

            int res = _employeeService.ImportFromCSV(formFile);

            Assert.True(res > 0);
        }

        [Fact]
        public void GetAll()
        {
            List<Employee> employees = _employeeService.GetAll();

            Assert.True(employees != null && employees.Count > 0);
        }

        [Fact]
        public void GetById()
        {
            Employee? employee = _employeeService.GetById(2);

            Assert.True(employee != null);
        }

        [Fact]
        public void Edit()
        {
            Employee? employee = _employeeService.GetById(2);
            int res = 0;
            if (employee!= null)
            {
                employee.Payroll_Number = "123";
                employee.Address = "TEST";
                res = _employeeService.Edit(employee);
            }

            Assert.True(employee != null && res > 0);
        }
    }
}
