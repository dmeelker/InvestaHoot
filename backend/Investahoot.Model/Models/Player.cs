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
        public Dictionary<Guid, int> GivenAnswersPerQuestionId { get; } = new();
        public int Score { get; private set; }
        public EventStream Events { get; } = new();

        public Player(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public bool AnsweredQuestion(Guid id) => GivenAnswersPerQuestionId.ContainsKey(id);
        public void AddAnswer(Guid questionid, int answerIndex)
        {
            GivenAnswersPerQuestionId[questionid] = answerIndex;
        }

        public void GivePoints(int points)
        {
            Score += points;
        }
    }
}
