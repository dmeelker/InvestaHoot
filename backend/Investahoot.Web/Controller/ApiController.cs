using Investahoot.Model;
using Investahoot.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        [Route("/join")]
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
        [Route("/state")]
        public IActionResult GetState(Guid gameId, Guid playerId)
        {
            if (_gameManager.GameId != gameId)
                return BadRequest("Invalid game id");

            if (!_gameManager.PlayerExists(playerId))
                return BadRequest("Invalid player id");

            switch (_gameManager.State)
            {
                case GameManager.GameState.Lobby:
                    return Ok(
                        new
                        {
                            State = "Lobby",
                            Players = _gameManager.Players.Select(player => player.Name)
                        });
                case GameManager.GameState.Question:
                    return Ok(
                        new
                        {
                            State = "Question",
                            Answers = _gameManager.CurrentRound!.Question.Answers,
                            TimeLeft = _gameManager.CurrentRound!.TimeLeft.TotalSeconds
                        });
                case GameManager.GameState.Score:
                    return Ok(
                        new
                        {
                            State = "Score",
                            Players = _gameManager.Players.Select(player => new
                            {
                                Name = player.Name,
                                Score = _gameManager.Players.Select(player => player.Score)
                            })
                        });

                default:
                    return BadRequest();
            }
        }

        [HttpPost]
        [Route("/answer")]
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
