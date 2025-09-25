using Contracts.BindingModels;
using Contracts.BusinessContracts;
using Contracts.CacheContracts;
using Contracts.StorageContracts;

namespace BusinessLogic.Implements {
    public class SimbirServiceLogic : ISimbirServiceLogic {
        private readonly ISimbirServiceCache   simbirServiceCache;
        private readonly ISimbirServiceStorage simbirServiceStorage;
        public SimbirServiceLogic(ISimbirServiceCache simbirServiceCacheImp, ISimbirServiceStorage simbirServiceStorageImp)
        {
            simbirServiceCache = simbirServiceCacheImp;
            simbirServiceStorage = simbirServiceStorageImp;
        }

        public void AddNewService(in SimbirServiceBindingModel insertModel)
        {
            if (simbirServiceCache.AddCacheServiceInfo(insertModel) == false) { 
                throw new Exception("Error: AddNewService is failed"); 
            }
        }
        public void UpdateService(in SimbirServiceBindingModel UpdateModel)
        {
            if (simbirServiceCache.EditCacheServiceInfo(UpdateModel) == false) {
                throw new Exception("Error: UpdateServuce is failed");
            }
        }
        public void DeleteService(int deleteModelId)
        {
            if (simbirServiceCache.DeleteCacheServiceInfo(deleteModelId) == false) {
                throw new Exception("Error: DeleteService is failed");
            }
        }

        public void GetServiceInfo(out List<SimbirServiceBindingModel?> modelsInfo)
        {
            simbirServiceStorage.GetServiceDbInfo(out modelsInfo);
            if (modelsInfo.Count == 0) {
                throw new Exception("Error: GetServiceInfo returned at empty list");
            }
        }
        public void GetServiceInfo(out SimbirServiceBindingModel? modelInfo, int serviceId)
        {
            simbirServiceCache.GetCacheServiceInfo(out modelInfo, serviceId);
            if (modelInfo == null) {
                throw new Exception("Error: GetServiceInfo returned at null");
            }
        }
    }
}
