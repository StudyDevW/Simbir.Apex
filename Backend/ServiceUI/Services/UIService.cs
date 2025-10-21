using Microsoft.AspNetCore.Http;
using Middleware_Components.DTO;
using Middleware_Components.JWT.DTO.CheckUsers;
using Middleware_Components.Services;
using ServiceUI.Interfaces;

namespace ServiceUI.Services
{
    public class UIService : IUIService
    {
        private readonly IDatabaseService _database;
        private readonly IJwtService _jwt;
        private readonly ICacheService _cache;
        private readonly ILogger _logger;

        public UIService(IDatabaseService database, IJwtService jwt, ICacheService cache)
        {
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("ui-service-logger");
            _database = database;
            _jwt = jwt;
            _cache = cache;
        }
        
        //AUTH PARTS
        private async Task<AuthPairTokens?> GenerateTokens(AuthCheckInfo check)
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

                AuthPairTokens pair_tokens = new AuthPairTokens()
                {
                    accessToken = _cache.GetKeyFromStorage(check.check_success.Id, "accessTokens"),
                    refreshToken = _cache.GetKeyFromStorage(check.check_success.Id, "refreshTokens")
                };

                _logger.LogInformation($"Пользователь {check.check_success.Id} успешно вошел!");

                return pair_tokens;
            }
            else
            {
                var validation = await _jwt.AccessTokenValidation(
                   $"Bearer {_cache.GetKeyFromStorage(check.check_success.Id, "accessTokens")}"
               );

                if (validation.TokenHasSuccess())
                {
                    AuthPairTokens pair_tokens = new AuthPairTokens()
                    {
                        accessToken = _cache.GetKeyFromStorage(check.check_success.Id, "accessTokens"),
                        refreshToken = _cache.GetKeyFromStorage(check.check_success.Id, "refreshTokens")
                    };

                    return pair_tokens;
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

        public async Task<AuthPairTokens?> AuthPart(string username, string password)
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

        public async Task<AuthPairTokens?> RefreshClientSession(string refreshTokenDTO)
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



                AuthPairTokens pair_tokens = new AuthPairTokens()
                {
                    accessToken = _cache.GetKeyFromStorage(authsuccess.Id, "accessTokens"),
                    refreshToken = _cache.GetKeyFromStorage(authsuccess.Id, "refreshTokens")
                };


                _logger.LogInformation($"Токены для id: {validation.token_success.Id} обновлены!");

                return pair_tokens;
            }

            return null;
        }

        //USERS CONTROLS PARTS
        public async Task AddNewUser(UserAddDTO dtoObj, string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                if (validation.token_success!.userRoles!.Equals("MANAGER") || validation.token_success!.userRoles!.Equals("SUPER_USER"))
                    await _database.AddUser(dtoObj);
                else
                    throw new Exception("role_invalid");
            }
        }

        public async Task ChangeUser(UserChangeDTO dtoObj, string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                if (validation.token_success!.userRoles!.Equals("MANAGER") || validation.token_success!.userRoles!.Equals("SUPER_USER"))
                    await _database.ChangeUser(dtoObj, validation.token_success!.Id);
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

        public async Task<List<UserGetDTO>?> GetAllUsers(string token)
        {
            var validation = await _jwt.AccessTokenValidation(token);

            if (validation.TokenHasError())
            {
                throw new Exception("token_invalid");
            }
            else if (validation.TokenHasSuccess())
            {
                return await _database.GetAllUsers();
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
    }
}
