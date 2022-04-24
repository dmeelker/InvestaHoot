using Investahoot.Model;
using Investahoot.Model.Events;
using Investahoot.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Investahoot.Web.Controller
{
    [Route("api/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly GameManager _gameManager;

        public ApiController(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        [HttpGet]
        [Route("join")]
        public async Task<IActionResult> JoinGame(string name)
        {
            var player = new Player(name);
            await _gameManager.AddPlayer(player);

            return Ok(
            new
            {
                GameId = _gameManager.GameId,
                PlayerId = player.Id
            });
        }

        [HttpGet]
        [Route("state")]
        public ActionResult<GameEvent> GetState(Guid gameId, Guid playerId)
        {
            if (_gameManager.GameId != gameId || !_gameManager.PlayerExists(playerId))
            {
                return new GameClosedEvent();
            }

            var player = _gameManager.GetPlayer(playerId);
            return player.Events.LastEvent;
        }

        [HttpGet]
        [Route("events")]
        public async Task GetEvents(Guid gameId, Guid playerId, CancellationToken cancellationToken)
        {
            Response.ContentType = "text/event-stream";

            while (!cancellationToken.IsCancellationRequested)
            {
                var player = _gameManager.GetPlayer(playerId);

                if(player == null)
                {
                    break;
                }

                var e = await player.Events.WaitForEvent(cancellationToken);
                string data = $"data: {JsonSerializer.Serialize(e, e.GetType())}\n\n";

                await HttpContext.Response.WriteAsync(data);
                await HttpContext.Response.Body.FlushAsync();
            }

            Response.Body.Close();
        }

        [HttpPost]
        [Route("answer")]
        public async Task<IActionResult> JoinGame(Guid gameId, Guid playerId, int answer)
        {
            if (_gameManager.GameId != gameId)
                return BadRequest("Invalid game id");

            if (!_gameManager.PlayerExists(playerId))
                return BadRequest("Invalid player id");

            await _gameManager.GiveAnswer(playerId, answer);
            return Ok();
        }
    }
}
