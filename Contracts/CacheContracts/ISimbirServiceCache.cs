using Contracts.BindingModels;

namespace Contracts.CacheContracts {
    public interface ISimbirServiceCache {
        public bool AddCacheServiceInfo(in SimbirServiceBindingModel insertModel);
        public bool EditCacheServiceInfo(in SimbirServiceBindingModel updateModel);
        public bool DeleteCacheServiceInfo(int deleteModelId);
        public void GetCacheServiceInfo(out SimbirServiceBindingModel? record, int serviceId);
    }
}
