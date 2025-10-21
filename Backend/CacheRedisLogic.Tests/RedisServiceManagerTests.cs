using Contracts.BindingModels;
using Contracts.StorageContracts;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Net;
using Xunit;

namespace CacheRedisLogic.Tests {
    public class RedisServiceManagerTests {

        private Mock<ISimbirServiceStorage> mockServiceDb = new Mock<ISimbirServiceStorage>();
        private Mock<IDistributedCache> mockDist = new Mock<IDistributedCache>();

        [Fact]
        public void InsertCacheTest()
        {
            // Arrange
            var cacheLogic = new RedisSimbirServiceCache(mockServiceDb.Object, mockDist.Object);
            SimbirServiceBindingModel data = new SimbirServiceBindingModel {
                ServiceName = "Service",
                EndPointService = new IPEndPoint(new IPAddress([127, 0, 0, 1]), 8080),
                Id = 0
            };

            // Act
            cacheLogic.InsertCacheServiceInfo(data);

            // Assert
        }

        [Fact]
        public void UpdateCacheTest()
        {
            // Arrange
            var cacheLogic = new RedisSimbirServiceCache(mockServiceDb.Object, mockDist.Object);
            SimbirServiceBindingModel data = new SimbirServiceBindingModel {
                ServiceName = "Service",
                EndPointService = new IPEndPoint(new IPAddress([127, 0, 0, 1]), 8000),
                Id = 2
            };
            // Act
            cacheLogic.UpdateCacheServiceInfo(data);

            // Assert
        }

        [Fact]
        public void DeleteCacheTest()
        {
            // Arrange
            var cacheLogic = new RedisSimbirServiceCache(mockServiceDb.Object, mockDist.Object);
            int deleteId = 4;
            // Act
            cacheLogic.DeleteCacheServiceInfo(deleteId);

            // Assert
        }

        [Fact]
        public void GetCacheTest()
        {
            // Arrange
            var cacheLogic = new RedisSimbirServiceCache(mockServiceDb.Object, mockDist.Object);
            SimbirServiceBindingModel recoed;
            // Act
            cacheLogic.GetCacheServiceInfo(out recoed, 2);

            // Assert
        }
    }
}
