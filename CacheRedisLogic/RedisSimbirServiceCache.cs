using Contracts.BindingModels;
using Contracts.CacheContracts;
using Contracts.StorageContracts;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CacheRedisLogic {
    public class RedisSimbirServiceCache : ISimbirServiceCache {
        private readonly ISimbirServiceStorage simbirServiceStorage;
        private readonly IDistributedCache     distributedCache;
        private readonly TimeSpan              cacheTime;
        public RedisSimbirServiceCache(ISimbirServiceStorage simbirServiceStorageImp, IDistributedCache distributedCacheImp,
                                       TimeSpan cacheTimeValue)
        {
            simbirServiceStorage = simbirServiceStorageImp;
            distributedCache = distributedCacheImp;
            cacheTime = cacheTimeValue;
        }

        public bool AddCacheServiceInfo(in SimbirServiceBindingModel insertModel)
        {
            if (simbirServiceStorage.InsertDbServiceInfo(insertModel) == false) { return false; }
            string cacheData = JsonSerializer.Serialize(insertModel);
            distributedCache.SetString(insertModel.Id.ToString(), cacheData,
                                       new DistributedCacheEntryOptions {
                                           AbsoluteExpirationRelativeToNow = cacheTime
                                       });
            return true;
        }

        public bool EditCacheServiceInfo(in SimbirServiceBindingModel updateModel)
        {
            if (simbirServiceStorage.UpdateDbServiceInfo(updateModel) == false) { return false; }
            string cacheData = JsonSerializer.Serialize(updateModel);
            distributedCache.SetString(updateModel.Id.ToString(), cacheData,
                                       new DistributedCacheEntryOptions {
                                           AbsoluteExpirationRelativeToNow = cacheTime
                                       });
            return true;
        }
        
        public bool DeleteCacheServiceInfo(int deleteModelId)
        {
            if (simbirServiceStorage.DeleteDbServiceInfo(deleteModelId) == false) {  return false; }
            distributedCache.Remove(deleteModelId.ToString());
            return true;
        }

        public void GetCacheServiceInfo(out SimbirServiceBindingModel? record, int serviceId)
        {
            string? cacheData = null;
            record = null;
            cacheData = distributedCache.GetString(serviceId.ToString());
            if (cacheData != null) {
                record = JsonSerializer.Deserialize<SimbirServiceBindingModel>(cacheData);
            }
            else if (record == null){
                simbirServiceStorage.GetServiceDbInfo(out record, serviceId);
                if (record != null) {
                    cacheData = JsonSerializer.Serialize(record);
                    distributedCache.SetString(record.Id.ToString(), cacheData,
                                               new DistributedCacheEntryOptions {
                                                   AbsoluteExpirationRelativeToNow = cacheTime
                                               });
                }
            }
        }
    }
}