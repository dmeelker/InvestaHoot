using Investahoot.Model.Models;

namespace Investahoot.Model
{
    public class Round
    {
        public int Id { get; }
        public Question Question { get; }
        public DateTime StartTime { get; }
        public TimeSpan Duration => TimeSpan.FromSeconds(10);

        public TimeSpan TimeLeft => Duration - (DateTime.UtcNow - StartTime);
        public bool DurationElapsed => TimeLeft.TotalMilliseconds <= 0;

        public Round(int id, Question question)
        {
            Id = id;
            Question = question;
            StartTime = DateTime.UtcNow;
        }

        public int CalculateScore()
        {
            return (int)(1000 * (TimeLeft.TotalSeconds / Duration.TotalSeconds));
        }
    }
}
