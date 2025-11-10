using Contracts.BindingModels;
using Contracts.BusinessLogicContracts;
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

        public void InsertService(in SimbirServiceBindingModel insertModel) 
        {
            SimbirServiceBindingModel? searchModel;
            simbirServiceStorage.GetServiceDbInfo(out searchModel, insertModel.ServiceName);
            if (searchModel != null) { return; }
            simbirServiceStorage.InsertDbServiceInfo(insertModel);
            simbirServiceCache.InsertCacheServiceInfo(insertModel); 
        }
        public void UpdateService(in SimbirServiceBindingModel UpdateModel) 
        {
            simbirServiceStorage.UpdateDbServiceInfo(UpdateModel);
            simbirServiceCache.UpdateCacheServiceInfo(UpdateModel);
        }
        public void DeleteService(int deleteModelId) 
        {
            simbirServiceStorage.DeleteDbServiceInfo(deleteModelId);
            simbirServiceCache.DeleteCacheServiceInfo(deleteModelId); 
        }

        public void GetServiceInfo(out List<SimbirServiceBindingModel> modelsInfo) {
            simbirServiceStorage.GetServiceDbInfo(out modelsInfo);
        }
        public void GetServiceInfo(out SimbirServiceBindingModel modelInfo, int serviceId) {
            simbirServiceCache.GetCacheServiceInfo(out modelInfo, serviceId);
        }
    }
}
