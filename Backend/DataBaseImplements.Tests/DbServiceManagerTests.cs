using Contracts.BindingModels;
using DataBaseImplement.Implements;
using System.Net;
using Xunit;

namespace DataBaseImplements.Tests {
    public class DbServiceManagerTests {

        [Fact]
        public void InsertDbTest()
        {
            // Arrange
            var dbLogic = new DbSimbirServiceStorage();
            SimbirServiceBindingModel data = new SimbirServiceBindingModel {
                ServiceName = "Service",
                EndPointService = new IPEndPoint(new IPAddress([127, 0, 0, 1]), 8080),
                Id = 0
            };

            // Act
            dbLogic.InsertDbServiceInfo(data);

            // Assert
        }

        [Fact]
        public void UpdateDbTest()
        {
            // Arrange
            var dbLogic = new DbSimbirServiceStorage();
            SimbirServiceBindingModel data = new SimbirServiceBindingModel {
                ServiceName = "Service",
                EndPointService = new IPEndPoint(new IPAddress([127, 0, 0, 1]), 8000),
                Id = 2
            };
            // Act
            dbLogic.UpdateDbServiceInfo(data);

            // Assert
        }

        [Fact]
        public void DeleteDbTest()
        {
            // Arrange
            var dbLogic = new DbSimbirServiceStorage();
            int data = 9;

            // Act
            dbLogic.DeleteDbServiceInfo(data);

            // Assert
        }

        [Fact]
        public void GetDbTestList()
        {
            // Arrange
            var dbLogic = new DbSimbirServiceStorage();
            List<SimbirServiceBindingModel> recordsTest; 

            // Act
            dbLogic.GetServiceDbInfo(out recordsTest);

            // Assert
            Assert.NotNull(recordsTest);
        }

        [Fact]
        public void GetDbTest()
        {
            // Arrange
            var dbLogic = new DbSimbirServiceStorage();
            SimbirServiceBindingModel record;
            SimbirServiceBindingModel recordTest = new SimbirServiceBindingModel {
                ServiceName = "Service",
                EndPointService = IPEndPoint.Parse("127.0.0.1:8000"),
                Id = 2
            };

            // Act
            dbLogic.GetServiceDbInfo(out record, 2);

            // Assert
            Assert.Equal(recordTest.EndPointService, record.EndPointService);
        }
    }
}
