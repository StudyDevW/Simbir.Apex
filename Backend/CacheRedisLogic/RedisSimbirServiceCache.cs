using Contracts.BindingModels;
using Contracts.CacheContracts;
using Contracts.StorageContracts;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CacheRedisLogic {
    public class RedisSimbirServiceCache : ISimbirServiceCache {
        private readonly ISimbirServiceStorage simbirServiceStorage;
        private readonly IDistributedCache     distributedCache;

        private readonly JsonSerializerSettings jsonSettings;
        public RedisSimbirServiceCache(ISimbirServiceStorage simbirServiceStorageImp, IDistributedCache distributedCacheImp)
        {
            simbirServiceStorage = simbirServiceStorageImp;
            distributedCache = distributedCacheImp;

            jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new IPEndPointConverter());
            jsonSettings.Converters.Add(new IPAddressConverter());
            jsonSettings.Formatting = Formatting.Indented;
        }

        public void InsertCacheServiceInfo(in SimbirServiceBindingModel insertModel)
        {   
            if (insertModel == null) { throw new ArgumentNullException(nameof(insertModel)); }
            string cacheData = JsonConvert.SerializeObject(insertModel, jsonSettings);
            distributedCache.SetString(insertModel.Id.ToString(), cacheData,
                                       new DistributedCacheEntryOptions {
                                           AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(43800)
                                       });
        }

        public void UpdateCacheServiceInfo(in SimbirServiceBindingModel updateModel)
        {
            if (updateModel == null) { throw new ArgumentNullException(nameof(updateModel)); }
            simbirServiceStorage.UpdateDbServiceInfo(updateModel);
            string cacheData = JsonConvert.SerializeObject(updateModel, jsonSettings);
            distributedCache.SetString(updateModel.Id.ToString(), cacheData,
                                       new DistributedCacheEntryOptions {
                                           AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(43800)
                                       });
        }
        
        public void DeleteCacheServiceInfo(int deleteModelId)
        {
            simbirServiceStorage.DeleteDbServiceInfo(deleteModelId);
            distributedCache.Remove(deleteModelId.ToString());
        }

        public void GetCacheServiceInfo(out SimbirServiceBindingModel record, int serviceId)
        {
            string? cacheData = null;
            SimbirServiceBindingModel? temp_rec = null;
            cacheData = distributedCache.GetString(serviceId.ToString());
            if (cacheData != null) { 
                temp_rec = JsonConvert.DeserializeObject<SimbirServiceBindingModel>(cacheData, jsonSettings); 
            }
            if (temp_rec == null){
                simbirServiceStorage.GetServiceDbInfo(out record, serviceId);
                InsertCacheServiceInfo(record);
                return;
            }
            record = temp_rec;
        }
    }
}