using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investahoot.Model.Events
{
    public abstract class GameEvent
    {
        public abstract string State { get; }
    }

    public class LobbyEvent : GameEvent
    {
        public override string State => "Lobby";
        public IEnumerable<string> Players { get; }

        public LobbyEvent(IEnumerable<string> players)
        {
            Players = players;
        }
    }

    public class QuestionEvent : GameEvent
    {
        public override string State => "Question";
        public int RoundId { get; }
        public IEnumerable<string> Answers { get; }
        public bool Answered { get; }
        public int TimeLeft { get; }

        public QuestionEvent(int roundId, IEnumerable<string> answers, bool answered, int timeLeft)
        {
            RoundId = roundId;
            Answers = answers;
            Answered = answered;
            TimeLeft = timeLeft;
        }
    }

    public class RoundFinishedEvent : GameEvent
    {
        public override string State => "RoundFinished";

        public bool CorrectAnswer { get; }
        public int Points { get; }
        public int CurrentRanking { get; }

        public RoundFinishedEvent(bool correctAnswer, int points, int currentRanking)
        {
            CorrectAnswer = correctAnswer;
            Points = points;
            CurrentRanking = currentRanking;
        }
    }

    public class ScoreEvent : GameEvent
    {
        public override string State => "Score";
        public IEnumerable<PlayerScore> Players { get; }

        public ScoreEvent(IEnumerable<PlayerScore> players)
        {
            Players = players;
        }
    }

    public class PlayerScore
    {
        public string Name { get; }
        public int Score { get; }

        public PlayerScore(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }

    public class GameClosedEvent : GameEvent
    {
        public override string State => "Closed";
    }
}
