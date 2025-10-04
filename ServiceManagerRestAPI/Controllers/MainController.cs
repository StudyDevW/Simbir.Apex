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

        [HttpPost]
        public void InsertService(SimbirServiceBindingModel insertModel) 
        {
            try { simbirServiceLogic.InsertService(insertModel); }
            catch (Exception ex) {
                Results.BadRequest(ex);
                throw;
            }
        }

        [HttpPost]
        public void UpdateService(SimbirServiceBindingModel updateModel)
        {
            try { simbirServiceLogic.UpdateService(updateModel); }
            catch (Exception ex) {
                Results.BadRequest(ex);
                throw;
            }
        }

        [HttpPost]
        public void DeleteService(int deleteModelId)
        {
            try { simbirServiceLogic.DeleteService(deleteModelId); }
            catch (Exception ex){ 
                Results.BadRequest(ex);
                throw;
            }
        }

        [HttpGet]
        public List<SimbirServiceBindingModel> GetServiceInfoList()
        {
            List<SimbirServiceBindingModel> recordList = new();
            try { simbirServiceLogic.GetServiceInfo(out recordList); }
            catch (Exception ex) {
                Results.BadRequest(ex);
                throw;
            }
            return recordList;
        }

        [HttpGet]
        public SimbirServiceBindingModel GetServiceInfo(int serviceId)
        {
            SimbirServiceBindingModel record;
            try { simbirServiceLogic.GetServiceInfo(out record, serviceId); }
            catch (Exception ex) { 
                Results.BadRequest(ex);
                throw;
            }
            return record;
        }
    }
}
