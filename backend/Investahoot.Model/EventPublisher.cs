using Investahoot.Model.Events;
using Investahoot.Model.Models;

namespace Investahoot.Model
{
    internal class EventPublisher
    {
        public void PublishLobbyEvent(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                var otherPlayerNames = players.Except(new[] { player }).Select(p => p.Name).ToList();
                player.Events.PublishEvent(new LobbyEvent(otherPlayerNames));
            }
        }

        public void PublishQuestionEventToAll(IEnumerable<Player> players, Round round)
        {
            foreach (var player in players)
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

        public void PublishRoundFinishedEventToAll(IEnumerable<Player> players, Round round)
        {
            var index = 1;
            foreach (var player in players)
            {
                PublicRoundFinishedEvent(player, index, round);
                index++;
            }
        }

        public void PublicRoundFinishedEvent(Player player, int rankingIndex, Round round)
        {
            player.Events.PublishEvent(new RoundFinishedEvent(
                correctAnswer: player.AnsweredQuestionCorrectly(round.Question.Id, round.Question.CorrectAnswerIndex),
                points: player.GetPointsForQuestion(round.Question.Id),
                currentRanking: rankingIndex
            ));
        }

        public void PublishScores(IEnumerable<Player> players)
        {
            var e = new ScoreEvent(
                players.Select(player => new PlayerScore(player.Name, player.Score))
                .ToList());

            foreach (var player in players)
            {
                player.Events.PublishEvent(e);
            }
        }

        public void PublishGameClosed(IEnumerable<Player> players)
        {
            var e = new GameClosedEvent();

            foreach (var player in players)
            {
                player.Events.PublishEvent(e);
            }
        }
    }
}
