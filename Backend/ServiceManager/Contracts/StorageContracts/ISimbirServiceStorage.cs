using Contracts.BindingModels;

namespace Contracts.StorageContracts {
    public interface ISimbirServiceStorage {
        public void InsertDbServiceInfo(in SimbirServiceBindingModel insertModel);
        public void UpdateDbServiceInfo(in SimbirServiceBindingModel updateModel);
        public void DeleteDbServiceInfo(int deleteModelId);
        public void GetServiceDbInfo(out List<SimbirServiceBindingModel> recordList);
        public void GetServiceDbInfo(out SimbirServiceBindingModel record, int serviceId);
        public void GetServiceDbInfo(out SimbirServiceBindingModel record, string serviceName);
    }
}
