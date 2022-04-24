using Investahoot.Model.Events;
using Investahoot.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investahoot.Model
{
    internal class EventPublisher
    {
        public void PublishLobbyEvent(IEnumerable<Player> players)
        {
            var playerNames = players.Select(p => p.Name).ToList();

            foreach(var player in players)
            {
                player.Events.PublishEvent(new LobbyEvent(playerNames));
            }
        }

        public void PublishQuestionEventToAll(IEnumerable<Player> players, Round round)
        {
            foreach(var player in players)
            {
                PublishQuestionEvent(player, round);
            }
        }

        public void PublishQuestionEvent(Player player, Round round)
        {
            player.Events.PublishEvent(new QuestionEvent(
                round.Id,
                round.Question.Answers,
                player.AnsweredQuestion(round.Question.Id),
                (int)round.TimeLeft.TotalSeconds));
        }

        public void PublishScores(IEnumerable<Player> players)
        {
            var e = new ScoreEvent(
                players.Select(player => new PlayerScore(player.Name, player.Score))
                .ToList());

            foreach(var player in players)
            {
                player.Events.PublishEvent(e);
            }
        }

        public void PublishGameClosed(IEnumerable<Player> players)
        {
            var e = new GameClosedEvent();

            foreach(var player in players)
            {
                player.Events.PublishEvent(e);
            }
        }
    }
}
