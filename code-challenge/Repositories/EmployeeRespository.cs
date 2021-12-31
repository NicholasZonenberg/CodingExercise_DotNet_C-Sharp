using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        // Get by id for Employee with enhancements to resolve pre existing bugs
        public Employee GetById(string id)
        {
            // Extract the database to a temporary dictionary with an indepth include for direct reports 
            Dictionary<String, Employee> temp = _employeeContext.Employees.Include(e => e.DirectReports).ToDictionary(x => x.EmployeeId);
            // Checks that provided id exists in the dictionary.  If it doesn't return null which means the requested compensation does not exist
            if (!temp.ContainsKey(id))
            {
                return null;
            }
            // Return the compensation with the requested ID
            return temp[id];
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
