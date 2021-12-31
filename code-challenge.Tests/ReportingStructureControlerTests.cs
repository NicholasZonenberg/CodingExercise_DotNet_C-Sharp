using challenge.Controllers;
using challenge.Data;
using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;

namespace code_challenge.Tests.Integration
{
    // Unit tests for the ReportingStructure
    [TestClass]
    public class ReportingStructureControlerTests
    {
        // Setup the enviroment for the unit tests
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        //Clean up the unit test enviroment
        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        // Check the ReportStructure for the employee with 4 total direct / inderict reports
        [TestMethod]
        public void GetReportingStructure_Returns_4()
        {
            // Arrange
            // provide the desired employee ID and the expected number of reports
            string id = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            int expectedNumberOfReports = 4;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/report/{id}");
            var response = getRequestTask.Result;

            // Assert
            // make sure status code is right
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // Extract the report structure and confirm the values match
            var reportStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedNumberOfReports, reportStructure.numberOfReports);
            Assert.AreEqual(id, reportStructure.employee.EmployeeId);
        }

        // Check the ReportStructure for the employee with 2 total direct / inderict reports
        [TestMethod]
        public void GetReportingStructure_Returns_2()
        {
            // Arrange
            // provide the desired employee ID and the expected number of reports
            string id = "03aa1462-ffa9-4978-901b-7c001562cf6f";
            int expectedNumberOfReports = 2;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/report/{id}");
            var response = getRequestTask.Result;

            // Assert
            // make sure status code is right
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportStructure = response.DeserializeContent<ReportingStructure>();
            // Extract the report structure and confirm the values match
            Assert.AreEqual(expectedNumberOfReports, reportStructure.numberOfReports);
            Assert.AreEqual(id, reportStructure.employee.EmployeeId);
        }

        // Check the ReportStructure for the employee with 0 total direct / inderict reports
        [TestMethod]
        public void GetReportingStructure_Returns_0()
        {
            // Arrange
            // provide the desired employee ID and the expected number of reports
            string id = "b7839309-3348-463b-a7e3-5de1c168beb3";
            int expectedNumberOfReports = 0;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/report/{id}");
            var response = getRequestTask.Result;

            // Assert
            // make sure status code is right
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // Extract the report structure and confirm the values match
            var reportStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedNumberOfReports, reportStructure.numberOfReports);
            Assert.AreEqual(id, reportStructure.employee.EmployeeId);
        }

        // Unit test for the report structure where the provided ID is invalid
        [TestMethod]
        public void GetReportingStrucutre_Returns_NotFound()
        {
            // Arrange
            // Provide invalid EmployeeId
            string id = "invalid_id";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/report/{id}");
            var response = getRequestTask.Result;

            // Assert
            // make sure status code is as expected
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
