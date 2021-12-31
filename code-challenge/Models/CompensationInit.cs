using System;
namespace challenge.Models
{
    // Compensation initializer stucture for ease of providing information for initilizing a new compensation
    public class CompensationInit
    {
        // Compensation uses already existing Employees so we don't want an Employee object here.
        // Instead it's an Employee ID that we check and then pull out the Employee from the Employee Database
        public string id { get; set; }
        // Salary for the Compensation object
        public int salary { get; set; }
        // EffectiveDate for the Compensation Object
        public DateTime effectiveDate { get; set; }
    }
}
