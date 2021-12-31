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
    // Repository for Compensation expands the ICompensationRepository interface
    public class CompensationRepository : ICompensationRepository
    {
        // Logger and context that the repository uses
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        // initializes the loggers and context the repository uses
        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            _logger = logger;
            _compensationContext = compensationContext;
        }

        // Adds a compensation to the Context and returns the added compensation
        public Compensation Add(Compensation compensation)
        {
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }

        // Get by id for Compensation
        public Compensation GetById(string id)
        {
            // Convert the database to a dictionary and perform an indepth request for direct reports to avoid direct reports being empty
            Dictionary<String, Compensation> temp = _compensationContext.Compensations.Include(e => e.employee.DirectReports).ToDictionary(x => x.employee.EmployeeId);
            // Checks that provided id exists in the dictionary.  If it doesn't return null which means the requested compensation does not exist
            if (!temp.ContainsKey(id))
            {
                return null;
            }
            // Return the compensation with the requested ID
            return temp[id];
        }

        // Async save function
        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
