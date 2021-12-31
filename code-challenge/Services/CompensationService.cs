using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    // Service for Compensation implementing the ICompensationServices interface
    public class CompensationService : ICompensationService
    {
        // Logger and repository for the service
        private readonly ILogger _logger;
        private readonly ICompensationRepository _compensationRepository;

        // initialize the logger and repository for the service
        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _logger = logger;
            _compensationRepository = compensationRepository;
        }

        // Create the new compensation in the database
        public Compensation Create(Compensation compensation)
        {
            // Makes sure the compensation isn't null before adding it to the repository and waiting for the save to finish as the save is asynchronus
            if (compensation != null)
            {
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
            }

            // return the compensation so it can be double checked if needed
            return compensation;
            //throw new NotImplementedException();
        }

        // Get by id for compensation
        public Compensation GetById(string id)
        {
            // Make sure the id isn't null or empty before procedining
            if (!String.IsNullOrEmpty(id))
            {
                // Get the compensation from the repository
                Compensation temp = _compensationRepository.GetById(id);
                // Return the requested compensation
                return temp;
            }

            // If the request is invalid send back a null result
            return null;
        }
    }
}
