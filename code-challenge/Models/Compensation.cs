using System;
namespace challenge.Models
{
    // Compensation class
    public class Compensation
    {
        // Employee the compensation is for
        public Employee employee { get; set; }
        // Extracted Employee ID for primary Key reasons
        public string Id { get; set; }
        // Compensation salary
        public int salary { get; set; }
        // Compensation effective date
        public DateTime effectiveDate { get; set; }

        // Initalize compensation and set the values to the provided values
        public Compensation(Employee employee, int salary, DateTime effectiveDate)
        {
            this.employee = employee;
            this.salary = salary;
            this.effectiveDate = effectiveDate;
            Id = this.employee.EmployeeId;
        }

        // Pramaterless constructor
        public Compensation()
        {
            employee = null;
            salary = 0;
            effectiveDate = DateTime.MinValue;
            Id = null;
        }
    }
}
