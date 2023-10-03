using CSVDataImporter.Controllers;
using CSVDataImporter.Data;
using CSVDataImporter.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;

namespace CSVDataImporter.Services
{
    public class EmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly AppDbContext _context;
        public EmployeeService(ILogger<EmployeeService> logger, AppDbContext context)
        {
            _context = context;
            _logger = logger;
        }
        public List<Employee> GetAll()
        {
            List<Employee> result = new();
            try
            {
                result = _context.Employees!.OrderBy(q => q.Surname).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
            }
            return result;
        }

        public Employee? GetById(int id)
        {
            Employee? employee = null;
            try
            {
                employee = _context.Employees.First(q => q.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
            }

            return employee;
        }

        public int ImportFromCSV(IFormFile file)
        {
            var result = 0;
            try
            {
                using Stream stream = file.OpenReadStream();
                var parser = new TextFieldParser(stream)
                {
                    TextFieldType = FieldType.Delimited
                };
                parser.SetDelimiters(new string[] { "," });
                int i = 0;
                while (!parser.EndOfData)
                {
                    string[] row = parser.ReadFields()!;
                    Employee employee = new()
                    {
                        Payroll_Number = row[0],
                        Forenames = row[1],
                        Surname = row[2],
                        Date_of_Birth = row[3],
                        Telephone = row[4],
                        Mobile = row[5],
                        Address = row[6],
                        Address_2 = row[7],
                        Postcode = row[8],
                        EMail_Home = row[9],
                        Start_Date = row[10]
                    };

                    if (i > 0) // if row is header
                        _context.Employees.Add(employee);
                    i++;
                }
                result = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
            }
            return result;
        }

        public int Edit(Employee model)
        {
            int res = 0;
            try
            {
                _context.Employees.Update(model);
                res = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
            }
            return res;
        }
    }
}
