using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRMS.Admin.API.Controllers;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure;
using HRMS.Admin.Service;
using HRMS.Admin.Types;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit.Sdk;

namespace HRMS.Admin.Tests
{
    [TestClass]
    public class ClientControllerUnitTest
    {
        [TestMethod]
        public async Task TestGetClientsAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(TestGetClientsAsync));
            IClientService clientService = new ClientService(null, dbContext,null,null);
            var controller = new ClientController(clientService, null);

            // Act 
            var response = await controller.GetAll();
            var value = response.Value;

            dbContext.Dispose();

            // Assert
            Assert.IsTrue(value.ToString() != "0", "Success");
        }

        //[TestMethod]
        //public async Task TestGetClientByIdAsync()
        //{
        ////    // Arrange
        ////    var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(TestGetClientByIdAsync));
        ////    //IHttpClientFactory clientFactory = new IHttpClientFactory();

        ////    //IOptions<APIEndPoints> apiEndPoints = new Options<APIEndPoints>();
        ////    IClientService clientService = new ClientService(null, dbContext, null,null);
        ////    var controller = new ClientController(clientService, null);

        ////    // Act 
        ////    var response = await controller.GetById(20);
        ////    var value = response.Value;

        ////    dbContext.Dispose();

        ////    // Assert
        ////    Assert.IsFalse(value == null, "Success");
        ////}
    }
}
