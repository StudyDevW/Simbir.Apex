using Microsoft.AspNetCore.Mvc;
using Middleware_Components.DTO;
using Middleware_Components.Interfaces;
using ServiceUI.Interfaces;

namespace ServiceUI.Controllers
{
    [Route("api/Rules/")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly IJwtService _jwt;
        private readonly ILogger _logger;
        private readonly IUIService _serviceUI;

        public RulesController(IJwtService jwt, IUIService serviceUI)
        {
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("ServiceUI | controller-logger");
            _jwt = jwt;
            _serviceUI = serviceUI;
        }

        //CRUD'S EXPERT 
        [HttpPost("TimedRules/Add")]
        public async Task<IActionResult> AddTimedRule(AddRuleDTO dtoObj)
        {
            try
            {
                await _serviceUI.CreateRuleTimed(dtoObj, Request.Headers["Authorization"]);
                return Ok("rule_added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost("TimedRules/Accept/{userId}/{ruleName}")]
        public async Task<IActionResult> AcceptTimedRule(Guid userId, string ruleName) 
        {
            try
            {
                await _serviceUI.AcceptRule(ruleName, userId, Request.Headers["Authorization"]);
                return Ok("rule_added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("TimedRules/All")]
        public async Task<IActionResult> GetTimedRules()
        {
            try
            {
                var rules = await _serviceUI.GetTimedRules(Request.Headers["Authorization"]);

                if (rules != null)
                    return Ok(rules);

                return BadRequest("rules_null");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        //[HttpPost("Add")]
        //public async Task<IActionResult> AddRule()
        //{
        //    try
        //    {
        //     //   await _serviceUI.CreateRuleTimed(dtoObj, Request.Headers["Authorization"]);
        //        return Ok("rule_added");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

        //[HttpPatch("Change")]
        //public async Task<IActionResult> ChangeRule()
        //{

        //    return BadRequest();
        //}

        //[HttpDelete("Delete/{id}")]
        //public async Task<IActionResult> DeleteRule(Guid id)
        //{

        //    return BadRequest();
        //}


        [HttpGet("{id}")]
        public async Task<IActionResult> GetRule(Guid id)
        {
            try
            {
                var rules = await _serviceUI.GetRuleFromDB(id, Request.Headers["Authorization"]);
                return Ok(rules);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetRules()
        {
            try
            {
                var rules = await _serviceUI.GetRulesFromDB(Request.Headers["Authorization"]);
                return Ok(rules);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
