using Investahoot.Model;
using Investahoot.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public abstract class BaseStateResult
        {
            public string State { get; set; }
            public abstract string Serialize();
        }

        public class LobbyStateResult : BaseStateResult
        {
            public List<string> Players { get; set; }

            public override string Serialize()
            {
                return JsonSerializer.Serialize(this);
            }
        }

        public class QuestionStateResult : BaseStateResult
        {
            public int RoundId { get; set; }
            public List<string> Answers { get; set; }
            public bool Answered { get; set; }
            public int TimeLeft { get; set; }

            public override string Serialize()
            {
                return JsonSerializer.Serialize(this);
            }
        }

        public class ScoreStateResult : BaseStateResult
        {
            public List<PlayerScore> Players { get; set; }

            public override string Serialize()
            {
                return JsonSerializer.Serialize(this);
            }
        }

        public class PlayerScore
        {
            public string Name { get; set; }
            public int Score { get; set; }
        }

        [HttpGet]
        [Route("state")]
        public ActionResult<BaseStateResult> GetState(Guid gameId, Guid playerId)
        {
            if (_gameManager.GameId != gameId)
                return BadRequest("Invalid game id");

            if (!_gameManager.PlayerExists(playerId))
                return BadRequest("Invalid player id");

            var player = _gameManager.GetPlayer(playerId);

            switch (_gameManager.State)
            {
                case GameManager.GameState.Lobby:
                    return
                        new LobbyStateResult
                        {
                            State = "Lobby",
                            Players = _gameManager.Players.Select(player => player.Name).ToList()
                        };
                case GameManager.GameState.Question:
                    return
                        new QuestionStateResult
                        {
                            State = "Question",
                            RoundId = _gameManager.CurrentRound!.Id,
                            Answers = _gameManager.CurrentRound!.Question.Answers,
                            TimeLeft = (int)_gameManager.CurrentRound!.TimeLeft.TotalSeconds,
                            Answered = player.AnsweredQuestion(_gameManager.CurrentRound!.Question.Id)
                        };
                case GameManager.GameState.Score:
                    return
                        new ScoreStateResult
                        {
                            State = "Score",
                            Players = _gameManager.Players.Select(player => new PlayerScore
                            {
                                Name = player.Name,
                                Score = player.Score
                            }).ToList()
                        };

                default:
                    throw new Exception();
            }
        }

        [HttpGet]
        [Route("events")]
        public async Task GetEvents(Guid gameId, Guid playerId, CancellationToken cancellationToken)
        {
            Response.ContentType = "text/event-stream";

            var lastState = "";
            while (!cancellationToken.IsCancellationRequested)
            {
                var state = GetState(gameId, playerId).Value!.Serialize();

                if (state != lastState)
                {
                    lastState = state;
                    string data = $"data: {state}\n\n";

                    await HttpContext.Response.WriteAsync(data);
                    await HttpContext.Response.Body.FlushAsync();
                }

                await Task.Delay(100);
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
