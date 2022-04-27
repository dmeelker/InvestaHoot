using Investahoot.Model.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investahoot.Model.Models
{
    public class Player
    {
        public Guid Id { get; }
        public string Name { get; }
        public Dictionary<Guid, GivenAnswer> GivenAnswersPerQuestionId { get; } = new();
        public int Score { get; private set; }
        public EventStream Events { get; } = new();

        public Player(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public bool AnsweredQuestion(Guid id) => GivenAnswersPerQuestionId.ContainsKey(id);
        public bool AnsweredQuestionCorrectly(Guid id, int correctAnswer) => AnsweredQuestion(id) && GivenAnswersPerQuestionId[id].Index == correctAnswer;
        public int GetPointsForQuestion(Guid id) => AnsweredQuestion(id) ? GivenAnswersPerQuestionId[id].Points : 0;

        public void AddAnswer(Guid questionid, int answerIndex, int points)
        {
            GivenAnswersPerQuestionId[questionid] = new(answerIndex, points);
        }

        public void GivePoints(int points)
        {
            Score += points;
        }
    }

    public class GivenAnswer
    {
        public int Index { get; }
        public int Points { get; }

        public GivenAnswer(int index, int points)
        {
            Index = index;
            Points = points;
        }
    }
}
