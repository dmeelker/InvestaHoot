namespace Investahoot.Model.Models
{
    public static class ScoreboardImage
    {
        public static Image CreateForTopThree(IEnumerable<Player> players)
        {
            var image = new Image();
            
            image.SetText(0, 0, "oooo-=SCOREBOARD=-oooo");
            
            var topThreePlayers = players.OrderBy(p => p.Score).Take(3);
            foreach (var (player, index) in topThreePlayers.WithIndex())
            {
                var name = player.Name.ToUpper();
                var score = player.Score;
                var line = $"{score} - {name}";
                
                image.SetText(2, index + 2, line);
            }
            
            image.SetText(0, 5, "oooooooooooooooooooooo");
            
            return image;
        }

        private static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
    }
}