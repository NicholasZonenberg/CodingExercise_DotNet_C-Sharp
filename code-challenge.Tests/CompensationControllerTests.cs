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
    // Unit tests for the Compensation class
    [TestClass]
    public class CompensationControllerTests
    {
        // Setup the test enviroment
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

        // clean up the test enviroment
        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_BadRequest()
        {
            // Arrange
            // Creates an invalid CompensationInit for initilizing a new Compensation Object
            CompensationInit init = new CompensationInit()
            {
                id = "invalid_id",
                salary = 1,
                effectiveDate = new DateTime(2021, 12, 31)
            };

            // Seralize the invalid CompesnationInit to json for passing to the body of the HTTP Post
            var requestContent = new JsonSerialization().ToJson(init);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/employee/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            // Make sure the status is correct
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        // Test creating a new compensation object
        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            // Create a CompensationInit which has all the data for initializing a new compensation object
            CompensationInit init = new CompensationInit()
            {
                id = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                salary = 12000,
                effectiveDate = new DateTime(2021, 12, 31)
            };

            // Seralize the CompensationInit to json for passing in the body of the HTTP Post
            var requestContent = new JsonSerialization().ToJson(init);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/employee/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            // Make sure the status is correct
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            // Extract the returned Compensation object and then check to make sure it's values match the provided values
            var compensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(compensation.employee.EmployeeId);
            Assert.AreEqual(init.id, compensation.employee.EmployeeId);
            Assert.AreEqual(init.salary, compensation.salary);
            Assert.AreEqual(init.effectiveDate, compensation.effectiveDate);
        }

        // Test method for checking if the HTTP Get request works properly
        // ====MUST BE RUN WITH THE CreateCompensation_Returns_Created TEST ABOVE AS THAT TEST POPULATES THIS ENTRY====
        [TestMethod]
        public void GetCompensationById_Returns_Ok()
        {
            // Arrange
            // Expected values for 
            string employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            int expectedSalary = 12000;
            DateTime expectedEffectiveDate = new DateTime(2021, 12, 31);

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            // Makes sure the status code matches expectations
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // Extract the returned Compensation object and confirm it's values match the expected values
            var compensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(employeeId, compensation.employee.EmployeeId);
            Assert.AreEqual(expectedSalary, compensation.salary);
            Assert.AreEqual(expectedEffectiveDate, compensation.effectiveDate);
        }

        // Test method for trying to use HTTP Get on a non existant Compensation
        [TestMethod]
        public void GetCompensationById_Returns_NotFound()
        {
            // Arrange
            string id = "invalid_id";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/compensation/{id}");
            var response = getRequestTask.Result;

            // Assert
            // Makes sure status code is as expected
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
