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
            Score
        }

        public Guid GameId { get; } = Guid.NewGuid();
        public List<Player> Players { get; } = new();
        public IEnumerable<Player> PlayersByScore => Players.OrderByDescending(player => player.Score);
        public List<Question> AllQuestions { get; } = new();
        public Queue<Question> RemainingQuestions { get; } = new();
        public GameState State { get; private set; } = GameState.Lobby;

        public Round? CurrentRound { get; private set; }

        private readonly VestaboardService _vestaboardService;

        public GameManager(VestaboardService vestaboardService)
        {
            _vestaboardService = vestaboardService;
            AllQuestions = new QuestionLoader().LoadQuestions("questions.json");
            RemainingQuestions = new Queue<Question>(AllQuestions);
            ChangeState(GameState.Lobby);

            AddPlayer(new Player("Testman"));
        }

        public Task AddPlayer(Player player)
        {
            ThrowIfNotInState(GameState.Lobby);

            Players.Add(player);
            return Task.CompletedTask;
        }

        public async Task BeginGame()
        {
            ThrowIfNotInState(GameState.Lobby);

            ChangeState(GameState.Question);
            await NextQuestion();
        }

        public async Task NextQuestion()
        {
            var question = RemainingQuestions.Dequeue();

            CurrentRound = new Round(question);

            await _vestaboardService.SendImageMessage(new VestaboardCharacterMessage(CurrentRound.Question.Image));
        }

        public async Task CheckIfTimeHasElapsed()
        {
            if (CurrentRound == null)
                return;

            if (CurrentRound.DurationElapsed)
            {
                await CompleteRound();
            }
        }

        public async Task GiveAnswer(Guid playerId, int answerIndex)
        {
            var player = GetPlayer(playerId);

            if (!player.AnsweredQuestion(CurrentRound!.Question.Id))
            {
                player.AddAnswer(CurrentRound.Question.Id, answerIndex);

                if (answerIndex == CurrentRound!.Question.CorrectAnswerIndex)
                {
                    var points = CurrentRound.CalculateScore();
                    player.GivePoints(points);
                }
            }

            if (AllPlayersAnswered)
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
                FinishGame();
            }
        }

        private void FinishGame()
        {
            ChangeState(GameState.Score);
        }

        private void ChangeState(GameState state)
        {
            State = state;
        }

        private bool AllPlayersAnswered => Players.All(p => p.AnsweredQuestion(CurrentRound!.Question.Id));

        private Player GetPlayer(Guid id) => Players.Single(p => p.Id == id);

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
