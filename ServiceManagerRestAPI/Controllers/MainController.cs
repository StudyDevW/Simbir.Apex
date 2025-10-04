using Contracts.BindingModels;
using Contracts.BusinessLogicContracts;
using Microsoft.AspNetCore.Mvc;

namespace ServiceManagerRestAPI.Controllers {

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase {
        private readonly ISimbirServiceLogic simbirServiceLogic;
        public MainController(ISimbirServiceLogic simbirServiceLogicImp)
        {
            simbirServiceLogic = simbirServiceLogicImp;
        }

        [HttpPost]
        public IActionResult InsertService(SimbirServiceBindingModel insertModel) 
        {
            try { 
                simbirServiceLogic.InsertService(insertModel);
                return Ok("Данные заполнены");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateService(SimbirServiceBindingModel updateModel)
        {
            try { 
                simbirServiceLogic.UpdateService(updateModel);
                return Ok("Данные обновлены");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult DeleteService(int deleteModelId)
        {
            try { 
                simbirServiceLogic.DeleteService(deleteModelId);
                return Ok("Данные удалены");
            }
            catch (Exception ex){ 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public List<SimbirServiceBindingModel> GetServiceInfoList()
        {
            List<SimbirServiceBindingModel> recordList = new();
            try { 
                simbirServiceLogic.GetServiceInfo(out recordList);
                if (recordList.Count == 0) { Results.BadRequest("Данные не найдены"); }
                Results.Ok($"{recordList.Count} записей");
            }
            catch (Exception ex) {
                Results.BadRequest(ex.Message);
                throw;
            }
            return recordList;
        }

        [HttpGet]
        public SimbirServiceBindingModel GetServiceInfo(int serviceId)
        {
            SimbirServiceBindingModel record;
            try { 
                simbirServiceLogic.GetServiceInfo(out record, serviceId);
                Results.Ok($"{record.ServiceName} найден");
            }
            catch (Exception ex) { 
                Results.BadRequest(ex.Message);
                throw;
            }
            return record;
        }
    }
}
