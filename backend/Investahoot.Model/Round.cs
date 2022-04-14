using Investahoot.Model.Models;

namespace Investahoot.Model
{
    public class Round
    {
        public Question Question { get; }
        public DateTime StartTime { get; }
        public TimeSpan Duration => TimeSpan.FromHours(1);

        public TimeSpan TimeLeft => Duration - (DateTime.UtcNow - StartTime);
        public bool DurationElapsed => TimeLeft.TotalMilliseconds <= 0;

        public Round(Question question)
        {
            Question = question;
            StartTime = DateTime.UtcNow;
        }

        public int CalculateScore()
        {
            return (int)(1000 * (TimeLeft.TotalSeconds / Duration.TotalSeconds));
        }
    }
}
