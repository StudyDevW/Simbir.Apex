using Contracts.BindingModels;
using Contracts.BusinessLogicContracts;
using Microsoft.AspNetCore.Mvc;

namespace ServiceManagerRestAPI.Controllers {

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController {
        private readonly ISimbirServiceLogic simbirServiceLogic;
        public MainController(ISimbirServiceLogic simbirServiceLogicImp)
        {
            simbirServiceLogic = simbirServiceLogicImp;
        }

        [HttpGet]
        public bool AddNewService(SimbirServiceBindingModel insertModel) 
        {
            try { simbirServiceLogic.AddNewService(insertModel); }
            catch { return false; }
            return true;
        }

        [HttpGet]
        public bool UpdateService(SimbirServiceBindingModel updateModel)
        {
            try { simbirServiceLogic.UpdateService(updateModel); }
            catch { return false; }
            return true;
        }

        [HttpGet]
        public bool DeleteService(int deleteModelId)
        {
            try { simbirServiceLogic.DeleteService(deleteModelId); }
            catch { return false; }
            return true;
        }

        [HttpGet]
        public List<SimbirServiceBindingModel?>? GetServiceInfoList()
        {
            List<SimbirServiceBindingModel?> recordList;
            try { simbirServiceLogic.GetServiceInfo(out recordList); }
            catch { return null; }
            return recordList;
        }

        [HttpGet]
        public SimbirServiceBindingModel? GetServiceInfo(int serviceId)
        {
            SimbirServiceBindingModel? record;
            try { simbirServiceLogic.GetServiceInfo(out record, serviceId); }
            catch { return null; }
            return record;
        }
    }
}
