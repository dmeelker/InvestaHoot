using Investahoot.Model.Events;
using Investahoot.Model.Models;
using Investahoot.Model.Vestaboard;

namespace Investahoot.Model
{
    public class GameManager
    {
        public enum GameState
        {
            Lobby,
            Question,
            RoundResult,
            Score
        }

        public Guid GameId { get; private set; } = Guid.NewGuid();
        public List<Player> Players { get; } = new();
        public IEnumerable<Player> PlayersByScore => Players.OrderByDescending(player => player.Score);
        public List<Question> AllQuestions { get; } = new();
        public Queue<Question> RemainingQuestions { get; private set; } = new();
        public GameState State { get; private set; } = GameState.Lobby;

        public Round? CurrentRound { get; private set; }
        private DateTime _stateStartTime = DateTime.UtcNow;
        private readonly EventPublisher _eventPublisher = new();

        private readonly VestaboardService _vestaboardService;

        public GameManager(VestaboardService vestaboardService)
        {
            _vestaboardService = vestaboardService;
            AllQuestions = new QuestionLoader().LoadQuestions("questions.json");
        }

        public async Task Reset()
        {
            _eventPublisher.PublishGameClosed(Players);

            GameId = Guid.NewGuid();
            RemainingQuestions = new Queue<Question>(AllQuestions);
            Players.Clear();

            ChangeState(GameState.Lobby);
            await PostLobbyToVestaboard();
        }

        public async Task AddPlayer(Player player)
        {
            ThrowIfNotInState(GameState.Lobby);

            Players.Add(player);
            _eventPublisher.PublishLobbyEvent(Players);
            await PostLobbyToVestaboard();
        }

        public Task RemovePlayer(Player player)
        {
            Players.Remove(player);
            return Task.CompletedTask;
        }

        private async Task PostLobbyToVestaboard()
        {
            var image = new Image();
            image.SetCentered(0, "ooooooo PLAYERS oooooo");

            if (!Players.Any())
            {
                image.SetCentered(3, "404");
            }
            else
            {
                var y = 1;
                foreach (var player in Players)
                {
                    image.SetCentered(y, player.Name.ToUpper());
                    y++;
                }
            }

            await _vestaboardService.SendImageMessage(new VestaboardCharacterMessage(image));
        }

        public async Task BeginGame()
        {
            ThrowIfNotInState(GameState.Lobby);

            ChangeState(GameState.Question);
            await NextQuestion();
        }

        public async Task NextQuestion()
        {
            ChangeState(GameState.Question);
            var question = RemainingQuestions.Dequeue();

            CurrentRound = new Round((CurrentRound?.Id ?? -1) + 1, question);

            await _vestaboardService.SendImageMessage(new VestaboardCharacterMessage(CurrentRound.Question.Image));
            _eventPublisher.PublishQuestionEventToAll(Players, CurrentRound);
        }

        public async Task Update()
        {
            switch(State)
            {
                case GameState.Question:
                    CheckIfTimeHasElapsed();
                    break;
                case GameState.RoundResult:
                    await CheckRoundResultTimeout();
                    break;
            }
        }

        public void CheckIfTimeHasElapsed()
        {
            if (CurrentRound == null)
                return;
            Console.WriteLine($"Timeleft: {CurrentRound.TimeLeft}");
            if (CurrentRound.DurationElapsed)
            {
                ShowRoundResults();
            }
        }

        public async Task GiveAnswer(Guid playerId, int answerIndex)
        {
            var player = GetPlayer(playerId);

            if (!player.AnsweredQuestion(CurrentRound!.Question.Id))
            {
                var points = 0;

                if (answerIndex == CurrentRound!.Question.CorrectAnswerIndex)
                {
                    points = CurrentRound.CalculateScore();
                    player.GivePoints(points);
                }

                player.AddAnswer(CurrentRound.Question.Id, answerIndex, points);
                _eventPublisher.PublishQuestionEvent(player, CurrentRound);
            }

            if (AllPlayersAnswered)
            {
                ShowRoundResults();
            }
        }

        private void ShowRoundResults()
        {
            ChangeState(GameState.RoundResult);
            _eventPublisher.PublishRoundFinishedEventToAll(PlayersByScore, CurrentRound!);
        }

        private async Task CheckRoundResultTimeout()
        {
            if(StateDuration > TimeSpan.FromSeconds(3))
            {
                await CompleteRound();
            }
        }

        private async Task CompleteRound()
        {
            if (RemainingQuestions.Any())
            {
                await NextQuestion();
            }
            else
            {
                await FinishGame();
            }
        }

        private async Task FinishGame()
        {
            ChangeState(GameState.Score);
            
            await _vestaboardService.SendImageMessage(new VestaboardCharacterMessage(ScoreboardImage.CreateForTopThree(Players)));
            _eventPublisher.PublishScores(PlayersByScore);
        }

        private void ChangeState(GameState state)
        {
            State = state;
            _stateStartTime = DateTime.UtcNow;
        }

        private TimeSpan StateDuration => DateTime.UtcNow.Subtract(_stateStartTime);

        private bool AllPlayersAnswered => Players.All(p => p.AnsweredQuestion(CurrentRound!.Question.Id));

        public Player GetPlayer(Guid id) => Players.Single(p => p.Id == id);

        public bool PlayerExists(Guid id) => Players.Any(p => p.Id == id);

        private void ThrowIfNotInState(GameState state)
        {
            if (State != state)
            {
                throw new Exception("Invalid state!");
            }
        }
    }
}
