using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    // URL route to access the compensation information
    [Route("api/employee/compensation")]
    // Compensation controller expands Controller
    public class CompensationController : Controller
    {
        // All of the needed services and logging classes
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;
        private readonly ICompensationService _compensationService;

        // Initialize the class with all the services and logging
        public CompensationController(ILogger<CompensationController> logger, IEmployeeService employeeService, ICompensationService compensationService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _compensationService = compensationService;
        }

        // HTTP Post function for Compensation.  Handles adding new Compensation to the database
        [HttpPost]
        public IActionResult createCompensation([FromBody] CompensationInit init)
        {
            // logging the addition
            _logger.LogDebug($"Recived compensation create request for employee '{init.id}'");

            // Getting the Employee that is having Compensation added
            var employee = _employeeService.GetById(init.id);

            // If there's no employee with the provided ID it will return null and we need to tell the client they submited a bad request
            if(employee == null)
            {
                return BadRequest();
            }

            // create a new compensation
            Compensation compensation = new Compensation(employee, init.salary, init.effectiveDate);

            // Add the new compensation to the Service and database
            _compensationService.Create(compensation);

            // Let the user know the compensation was created and provide the user with the compensation so they can double check it
            return CreatedAtRoute("getEmployeeCompensationById", new { id = compensation.employee.EmployeeId }, compensation);
        }

        // HTTP Get for retriving an already existing Compensation requires an id be provided in the URL and then calls the getEmployeeCompensationById function to handle it
        [HttpGet("{id}", Name = "getEmployeeCompensationById")]
        public IActionResult getEmployeeCompensationById(string id)
        {
            // Logging the action
            _logger.LogDebug($"Received employee compensation get request for '{id}'");

            // Getting the compensation from the provided ID
            var compensation = _compensationService.GetById(id);

            // If the id has no compensation tell the client they provided a bad request
            if (compensation == null)
                return NotFound();

            // Return the requested compensation and tell the client it worked
            return Ok(compensation);
        }
    }
}
