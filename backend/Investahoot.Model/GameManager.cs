using Investahoot.Model.Models;
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

        public GameManager(IEnumerable<Question> questions)
        {
            AllQuestions = questions.ToList();
            RemainingQuestions = new Queue<Question>(questions);
            ChangeState(GameState.Lobby);
        }

        public void BeginGame()
        {
            ThrowIfNotInState(GameState.Lobby);

            ChangeState(GameState.Question);
            NextQuestion();
        }

        public void NextQuestion()
        {
            var question = RemainingQuestions.Dequeue();

            CurrentRound = new Round(question);
        }

        public void CheckIfTimeHasElapsed()
        {
            if (CurrentRound == null)
                return;

            if (CurrentRound.DurationElapsed)
            {
                CompleteRound();
            }
        }

        public void GiveAnswer(Guid playerId, int answerIndex)
        {
            var player = GetPlayer(playerId);

            if (!player.AnsweredQuestion(CurrentRound!.Question.Id))
            {
                player.AddAnswer(CurrentRound.Question.Id, answerIndex);
            }

            if (AllPlayersAnswered)
            {
                CompleteRound();
            }
        }

        private void CompleteRound()
        {
            if (RemainingQuestions.Any())
            {
                NextQuestion();
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
