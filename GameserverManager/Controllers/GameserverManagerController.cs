using Microsoft.AspNetCore.Mvc;

namespace GameserverManager.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GameserverManagerController : ControllerBase
    {
        NatsMessageSender _mq;
        ILogger _logger;
        public GameserverManagerController(NatsMessageSender mq, ILogger<GameserverManagerController> logger)
        {
            this._mq = mq;
            this._logger = logger;
        }

        [HttpPost]
        public async Task<string> FindServer(string username)
        {
            var result = await _mq.FindNiceServer(username);
            _logger.LogInformation("FindServer, username:{Username}, serverNumber:{ServerNumber}", username, result);
            return result;
        }
    }
}
