using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;
using challenge.Repositories;

namespace challenge.Controllers
{
    // URL route to access Reporting Structure info
    [Route("api/employee/report")]
    // Reporting Structure controller expands Controller
    public class ReportingStructureController : Controller
    {
        // The services and logging that the Controller uses
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        // Constructor for the controller initilizes the services and logging
        public ReportingStructureController(ILogger<ReportingStructureController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        // HTTP Get request for Reporting Structure requiring an ID provided in the URL and then calls getEmployeeReportById to handle it
        [HttpGet("{id}", Name = "getEmployeeReportById")]
        public IActionResult getEmployeeReportById(string id)
        {
            // logging the get request
            _logger.LogDebug($"Received employee report number get request for '{id}'");
            
            // Getting the Employee that matches the provided ID
            var employee = _employeeService.GetById(id);

            // If the provided ID is invalid the employee will be null and this lets the client know that the Direct Report was not found
            if (employee == null)
                return NotFound(); 

            // Creates the report structure dynamically and temporalily so it doesn't persist
            ReportingStructure reportingStructure = new ReportingStructure(employee);

            // Sends the reportStructure to the client letting them know their request worked
            return Ok(reportingStructure);
        }
    }
}
