using Microsoft.AspNetCore.Http;
using Middleware_Components.DTO;
using Middleware_Components.DTO.Pagination;
using Middleware_Components.JWT.DTO.CheckUsers;
using Middleware_Components.Interfaces;
using ServiceUI.Interfaces;
using Middleware_Components.DTO.Enums;

namespace ServiceUI.Services
{
    public class UIService : IUIService
    {
        private readonly IDatabaseService _database;
        private readonly IJwtService _jwt;
        private readonly ICacheService _cache;
        private readonly ILogger _logger;
        private readonly IMailService _mail;

        public UIService(IDatabaseService database, IMailService mail, IJwtService jwt, ICacheService cache)
        {
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("ui-service-logger");
            _database = database;
            _jwt = jwt;
            _cache = cache;
            _mail = mail;
        }
        
        //AUTH PARTS
        private async Task<AuthTokenInfoWR?> GenerateTokens(AuthCheckInfo check)
        {
            if (check.check_success == null)
                return null;

            if (!_cache.CheckExistKeysStorage(check.check_success.Id, "accessTokens"))
            {
                var accessToken = _jwt.JwtTokenCreation(check.check_success);
                var refreshToken = _jwt.RefreshTokenCreation(check.check_success);

                if (_cache.CheckExistKeysStorage(check.check_success.Id, "accessTokens"))
                    _cache.DeleteKeyFromStorage(check.check_success.Id, "accessTokens");

                if (_cache.CheckExistKeysStorage(check.check_success.Id, "refreshTokens"))
                    _cache.DeleteKeyFromStorage(check.check_success.Id, "refreshTokens");

                _cache.WriteKeyInStorage(check.check_success.Id, "accessTokens", accessToken, DateTime.UtcNow.AddMinutes(10));
                _cache.WriteKeyInStorage(check.check_success.Id, "refreshTokens", refreshToken, DateTime.UtcNow.AddDays(7));

                var accessTokenOut = _cache.GetKeyFromStorage(check.check_success.Id, "accessTokens")!;
                var refreshTokenOut = _cache.GetKeyFromStorage(check.check_success.Id, "refreshTokens")!;

                AuthTokenInfoWR out_tokens = new AuthTokenInfoWR()
                {
                    accessToken = accessTokenOut,
                    refreshToken = refreshTokenOut,
                    expires_at = _cache.GetKeyExpirationTime($"accessTokens_storage_{check.check_success.Id}")
                };

                _logger.LogWarning($"(DEBUG) REFRESH: {_cache.GetKeyFromStorage(check.check_success.Id, "refreshTokens")!}");
                _logger.LogInformation($"Пользователь {check.check_success.Id} успешно вошел!");

                return out_tokens;
            }
            else
            {
                var validation = await _jwt.AccessTokenValidation(
                   $"Bearer {_cache.GetKeyFromStorage(check.check_success.Id, "accessTokens")}"
                );

                if (validation.TokenHasSuccess())
                {
                    var accessTokenOut = _cache.GetKeyFromStorage(check.check_success.Id, "accessTokens")!;
                    var refreshTokenOut = _cache.GetKeyFromStorage(check.check_success.Id, "refreshTokens")!;

                    AuthTokenInfoWR out_tokens = new AuthTokenInfoWR()
                    {
                        accessToken = accessTokenOut,
                        refreshToken = refreshTokenOut,
                        expires_at = _cache.GetKeyExpirationTime($"accessTokens_storage_{check.check_success.Id}")
                    };

                    _logger.LogWarning($"(DEBUG) REFRESH <-->: {_cache.GetKeyFromStorage(check.check_success.Id, "refreshTokens")!}");

                    return out_tokens;
                }
            }

            return null;
        }
        

        public async Task<string?> ClientSignOut(string bearer_key)
        {
            var validation = await _jwt.AccessTokenValidation(bearer_key);

            if (validation.TokenHasError())
            {
                return null;
            }
            else if (validation.TokenHasSuccess())
            {

                _cache.DeleteKeyFromStorage(validation.token_success.Id, "accessTokens");

                _cache.DeleteKeyFromStorage(validation.token_success.Id, "refreshTokens");

                _logger.LogInformation($"Пользователь id: {validation.token_success.Id} вышел!");

                return $"{validation.token_success.Id}_is_logout";
            }

            return null;
        }

        public async Task<AuthTokenInfoWR?> AuthPart(string username, string password)
        {
            var checkSuccess = await _database.CheckUserAuth(username, password);

            AuthCheckInfo authCheckInfo = new AuthCheckInfo()
            {
                check_success = checkSuccess,
                check_error = null
            };

            var tokensOut = await GenerateTokens(authCheckInfo);

            return tokensOut;
        }

        public async Task<AuthTokenInfoWR?> RefreshClientSession(string refreshTokenDTO)
        {
            var validation = await _jwt.RefreshTokenValidation(refreshTokenDTO);

            if (validation.TokenHasError())
            {
                return null;
            }
            else if (validation.TokenHasSuccess())
            {
                Auth_CheckSuccess authsuccess = new Auth_CheckSuccess()
                {
                    Id = validation.token_success.Id,
                    roles = validation.token_success.userRoles,
                    username = validation.token_success.userName
                };

                var accessToken = _jwt.JwtTokenCreation(authsuccess);
                var refreshToken = _jwt.RefreshTokenCreation(authsuccess);

                if (_cache.CheckExistKeysStorage(authsuccess.Id, "accessTokens"))
                    _cache.DeleteKeyFromStorage(authsuccess.Id, "accessTokens");

                if (_cache.CheckExistKeysStorage(authsuccess.Id, "refreshTokens"))
                    _cache.DeleteKeyFromStorage(authsuccess.Id, "refreshTokens");


                _cache.WriteKeyInStorage(authsuccess.Id, "accessTokens", accessToken, DateTime.UtcNow.AddMinutes(10));
                _cache.WriteKeyInStorage(authsuccess.Id, "refreshTokens", refreshToken, DateTime.UtcNow.AddDays(7));

                var accessTokenOut = _cache.GetKeyFromStorage(authsuccess.Id, "accessTokens")!;

                var refreshTokenOut = _cache.GetKeyFromStorage(authsuccess.Id, "refreshTokens")!;

                AuthTokenInfoWR out_tokens = new AuthTokenInfoWR()
                {
                    accessToken = accessTokenOut,
                    refreshToken = refreshTokenOut,
                    expires_at = _cache.GetKeyExpirationTime($"accessTokens_storage_{authsuccess.Id}")
                };

                _logger.LogInformation($"Токены для id: {validation.token_success.Id} обновлены!");

                return out_tokens;
            }

            return null;
        }

        //USERS CONTROLS PARTS
        public async Task AddNewUser(UserAddDTO dtoObj, string email, string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                if (validation.token_success.userRoles.Contains("MANAGER") || validation.token_success.userRoles.Contains("SUPER_USER"))
                {
                    var userData = await _database.AddUser(dtoObj);

                    await _mail.SendRegisterData(email, userData);
                }
                else
                    throw new Exception("role_invalid");
            }
        }

        public async Task ChangeUser(UserChangeDTO dtoObj, Guid id, string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                if (validation.token_success!.userRoles!.Equals("MANAGER") || validation.token_success!.userRoles!.Equals("SUPER_USER"))
                    await _database.ChangeUser(dtoObj, id);
                else
                    throw new Exception("role_invalid");
            }
        }

        public async Task DeleteUser(Guid idUser, string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                if (validation.token_success!.userRoles!.Equals("MANAGER") || validation.token_success!.userRoles!.Equals("SUPER_USER"))
                    await _database.DeleteUser(idUser);
                else
                    throw new Exception("role_invalid");
            }
        }

        public async Task<PaginationOut<List<UserGetDTO>>?> GetAllUsers(int from, int count, string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                return await _database.GetAllUsers(from, count);
            }

            return null;
        }

        public async Task<UserGetDTO?> GetUser(Guid idUser, string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                return await _database.GetUser(idUser);
            }

            return null;
        }

        public async Task CreateRuleTimed(AddRuleDTO dtoObj, string token)
        {
            List<TimedRuleDTO> rulesForApprove = new List<TimedRuleDTO>();

            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                try
                {
                    var rulesIfCached = _cache.GetKeyFromStorage<List<TimedRuleDTO>>($"rule_for_alert_{validation.token_success!.Id}");

                    if (rulesIfCached != null)
                        rulesForApprove = rulesIfCached;

                    TimedRuleDTO rulesTableToCache = new TimedRuleDTO()
                    {
                        name = dtoObj.name,
                        description = dtoObj.description,
                        logic = dtoObj.logic,
                        severity = dtoObj.severity,
                        status = dtoObj.status,
                        created_by = validation.token_success!.Id,
                        created_at = DateTime.UtcNow
                    };

                    rulesForApprove.Add(rulesTableToCache);

                    _logger.LogInformation($"ID TOKEN SUCCESS: {validation.token_success!.Id}");

                    _cache.WriteKeyInStorageObject<List<TimedRuleDTO>>(
                        $"rule_for_alert_{validation.token_success!.Id}",
                        rulesForApprove,
                        DateTime.UtcNow.AddDays(1)
                    );
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<List<List<TimedRuleDTO>?>> GetTimedRules(string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                var allRulesCached = new List<List<TimedRuleDTO>?>();

                foreach (var userId in await _database.CollectAllIdUsers())
                {
                    var rulesCached = _cache.GetKeyFromStorage<List<TimedRuleDTO>>($"rule_for_alert_{userId}");

                    allRulesCached.Add(rulesCached);
                }

                return allRulesCached;
            }

            return null;
        } 

        public async Task AcceptRule(string ruleName, Guid idUser, string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                var rulesIfCached = _cache.GetKeyFromStorage<List<TimedRuleDTO>>($"rule_for_alert_{idUser}");

                if (rulesIfCached != null)
                {
                    foreach (var rule in rulesIfCached)
                        if (rule.name == ruleName)
                           await _database.RuleFillUp(rule);
                }
                else
                {
                    throw new Exception("rule_not_found");
                }
            }
        }

        public async Task<GetRuleDTO?> GetRuleFromDB(Guid ruleId, string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                return await _database.GetRuleFromDB(ruleId);
            }

            return null;
        }

        public async Task<List<GetRuleDTO>?> GetRulesFromDB(string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                return await _database.GetAllRulesFromDB();
            }

            return null;
        }

        public async Task<GetAlertDTO?> GetAlertFromDB(Guid alertId, string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                return await _database.GetAlertFromDB(alertId);
            }

            return null;
        }

        public async Task<PaginationOut<List<GetAlertDTO>>?> GetAlertsFromDB(AlertsArgsDTO dtoObj, string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                return await _database.GetAllAlertsFromDB(dtoObj);
            }

            return null;
        }

        public async Task<MeDTO> GetInfoMe(string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                return await _database.GetMeInfo(validation.token_success.Id);
            }

            return null;
        }

        public async Task<GetEventDTO?> GetEventFromDB(Guid eventId, string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                return await _database.GetEventFromDB(eventId);
            }

            return null;
        }

        public async Task<PaginationOut<List<GetEventDTO>>?> GetEventsFromDB(int from, int count, string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                return await _database.GetAllEventsFromDB(from, count);
            }

            return null;
        }

        

    }
}
