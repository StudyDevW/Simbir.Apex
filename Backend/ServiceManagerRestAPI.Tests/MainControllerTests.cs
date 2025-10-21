using Contracts.BindingModels;
using Contracts.BusinessLogicContracts;
using Moq;
using ServiceManagerRestAPI.Controllers;
using System.Net;
using Xunit;

namespace ServiceManagerRestAPI.Tests {

    public class MainControllerTests {

        private Mock<ISimbirServiceLogic> mockServiceLogic = new Mock<ISimbirServiceLogic>();

        [Fact]
        public void DependencyServiceTest()
        {
            // Arrange
            var controller = new MainController(mockServiceLogic.Object);
            SimbirServiceBindingModel data = new SimbirServiceBindingModel {
                ServiceName = "Service",
                EndPointService = new IPEndPoint(new IPAddress([127, 0, 0, 1]), 8080),
                Id = 0
            };

            // Act
            controller.InsertService(data);

            // Assert
            mockServiceLogic.Verify(v => v.InsertService(data), Times.Once);
        }
    }
}
