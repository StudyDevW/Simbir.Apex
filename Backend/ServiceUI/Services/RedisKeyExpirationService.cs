
using Middleware_Components.Interfaces;
using ServiceUI.Interfaces;

namespace ServiceUI.Services
{
    public class RedisKeyExpirationService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly ICacheService _cache;
        private readonly IDatabaseService _db;

        public RedisKeyExpirationService(ICacheService cache, IDatabaseService db)
        {
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("redis-listner-logger");
            _cache = cache;
            _db = db;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _cache.RedisSubscriber().Subscribe("__keyevent@0__:expired").OnMessage(async channelMessage =>
            {
                var expKey = channelMessage.Message.ToString();

                await HandleExpiredKey(expKey);
            });

            _logger.LogInformation("[Redis listener]: Запущен и ждет событий");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task HandleExpiredKey(string expKey)
        {
            try
            {
                _logger.LogInformation("[Redis listener]: Истек ключ {Key}", expKey);
                //TODO: Вид ключа (accessTokens_storage_34e833c4-f431-41ad-b202-adc5ceb8eecd), достать userId и записать как offline в бд

                if (expKey.StartsWith("accessTokens_storage_"))
                {
                    var uid = expKey.Substring("accessTokens_storage_".Length);

                    await _db.SetOfflineStatus(Guid.Parse(uid));
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Redis listener]: Ошибка при обработке {Key}", expKey);
            }
        }
    }
}
