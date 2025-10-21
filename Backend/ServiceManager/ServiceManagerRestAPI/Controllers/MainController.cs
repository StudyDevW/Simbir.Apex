using Contracts.BindingModels;
using Contracts.BusinessLogicContracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceManagerRestAPI.AppsettingsWork;

namespace ServiceManagerRestAPI.Controllers {

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase {
        private readonly ISimbirServiceLogic simbirServiceLogic;
        private readonly JsonSerializerSettings jsonSettings;
        private readonly AppsettingsWorkService appsettingsService;
        public MainController(ISimbirServiceLogic simbirServiceLogicImp)
        {
            simbirServiceLogic = simbirServiceLogicImp;
            jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new IPEndPointConverter());
            jsonSettings.Converters.Add(new IPAddressConverter());
            jsonSettings.Formatting = Formatting.Indented;
            appsettingsService = new();
        }

        [HttpPost]
        public IActionResult InsertService(string jsonData) 
        {
            SimbirServiceBindingModel? insertModel;
            try {
                insertModel = JsonConvert.DeserializeObject<SimbirServiceBindingModel>(jsonData, jsonSettings);
                if (insertModel == null) { return NoContent(); }
                simbirServiceLogic.InsertService(insertModel);
                appsettingsService.AddUpdateAppsettings(insertModel);
                return Ok("Данные заполнены");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateService(string jsonData)
        {
            SimbirServiceBindingModel? updateModel;
            try {
                updateModel = JsonConvert.DeserializeObject<SimbirServiceBindingModel>(jsonData, jsonSettings);
                if (updateModel == null) { return NoContent(); }
                simbirServiceLogic.UpdateService(updateModel);
                appsettingsService.AddUpdateAppsettings(updateModel);
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
        public IActionResult GetServiceInfoList()
        {
            List<SimbirServiceBindingModel> recordList;
            try { 
                simbirServiceLogic.GetServiceInfo(out recordList);
                if (recordList.Count == 0) { Results.BadRequest("Данные не найдены"); }
                return Ok(JsonConvert.SerializeObject(recordList, jsonSettings));
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetServiceInfo(int serviceId)
        {
            SimbirServiceBindingModel record;
            try { 
                simbirServiceLogic.GetServiceInfo(out record, serviceId);
                return Ok(JsonConvert.SerializeObject(record, jsonSettings));
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult RootConnect(string login, string password)
        {
            if (APIRoot.GetRootLogin() == login && APIRoot.GetRootPassword() == password) { return Ok(true); }
            return BadRequest("Данные введены не верно!");
        }
    }
}
