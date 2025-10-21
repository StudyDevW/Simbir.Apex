using Contracts.BindingModels;

namespace Contracts.CacheContracts {
    public interface ISimbirServiceCache {
        public void InsertCacheServiceInfo(in SimbirServiceBindingModel insertModel);
        public void UpdateCacheServiceInfo(in SimbirServiceBindingModel updateModel);
        public void DeleteCacheServiceInfo(int deleteModelId);
        public void GetCacheServiceInfo(out SimbirServiceBindingModel record, int serviceId);
    }
}
