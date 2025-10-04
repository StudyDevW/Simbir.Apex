using Contracts.BindingModels;

namespace Contracts.BusinessLogicContracts {
    public interface ISimbirServiceLogic {
        public void InsertService(in SimbirServiceBindingModel insertModel);
        public void UpdateService(in SimbirServiceBindingModel updateModel);
        public void DeleteService(int deleteModelId);
        public void GetServiceInfo(out List<SimbirServiceBindingModel> modelsInfo);
        public void GetServiceInfo(out SimbirServiceBindingModel modelInfo, int serviceId);
    }
}
