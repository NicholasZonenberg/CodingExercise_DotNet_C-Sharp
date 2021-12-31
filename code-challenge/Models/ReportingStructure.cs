namespace challenge.Models
{
    // Reporting Structure
    public class ReportingStructure
    {
        // The Employee that the Reporting Structure is about
        public Employee employee { get; set; }
        // The number of reports this is dynamically calculated bt the numberOfReportsTotal function
        public int numberOfReports {
            get {
                return numberOfReportsTotal(employee);
            }
        }

        // Initilizes the reporting structure with the provided Employee
        public ReportingStructure(Employee employee)
        {
            this.employee = employee;
        }

        // Recursive function that extracts all direct and indirict reports
        int numberOfReportsTotal(Employee countingEmployee)
        {
            // if the Employee has no Direct Reports terminate the recursion and pass back a value of 0
            if (countingEmployee.DirectReports is null)
                return 0;

            // Subtotal for the rucursive employee Direct Reports count
            int subtotal = countingEmployee.DirectReports.Count;
            // If the employee has more than 0 direct reports (Handles cases where Employee might have an empty but not null Direct Report Field) do recursion
            if (countingEmployee.DirectReports.Count > 0)
            {
                // For loop performing recursion going through all of the employees direct reports and checking if those employees have any direct reports of their own
                for (int x = 0; x < countingEmployee.DirectReports.Count; x++)
                {
                    // Perform the recursive function and add it's return to the subtoal
                    subtotal += numberOfReportsTotal(countingEmployee.DirectReports[x]);
                }
                //subtotal += numberOfReportsTotal(countingEmployee);
            }
            // pass the subtotal back up the recursion chain
            return subtotal;
        }
    }
}
