using Contracts.BindingModels;

namespace Contracts.StorageContracts {
    public interface ISimbirServiceStorage {
        public bool InsertDbServiceInfo(in SimbirServiceBindingModel insertModel);
        public bool UpdateDbServiceInfo(in SimbirServiceBindingModel updateModel);
        public bool DeleteDbServiceInfo(int deleteModelId);
        public void GetServiceDbInfo(out List<SimbirServiceBindingModel?> recordList);
        public void GetServiceDbInfo(out SimbirServiceBindingModel? record, int serviceId);
    }
}
