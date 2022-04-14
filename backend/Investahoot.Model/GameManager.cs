using Investahoot.Model.Models;
using Investahoot.Model.Vestaboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Player> Players { get; } = new();
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

            var image = new VestaboardCharacterMessage(CurrentRound.Question.Image);
            await _vestaboardService.SendImageMessage(image);
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


        private void ThrowIfNotInState(GameState state)
        {
            if (State != state)
            {
                throw new Exception("Invalid state!");
            }
        }
    }
}
