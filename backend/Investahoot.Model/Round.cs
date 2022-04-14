using Investahoot.Model.Models;

namespace Investahoot.Model
{
    public class Round
    {
        public Question Question { get; }
        public DateTime StartTime { get; }
        public TimeSpan Duration => TimeSpan.FromSeconds(30);

        public TimeSpan TimeLeft => Duration - (DateTime.UtcNow - StartTime);
        public bool DurationElapsed => TimeLeft.TotalMilliseconds <= 0;

        public Round(Question question)
        {
            Question = question;
            StartTime = DateTime.UtcNow;
        }
    }
}
