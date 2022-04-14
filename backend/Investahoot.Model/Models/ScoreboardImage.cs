namespace Investahoot.Model.Models
{
    public static class ScoreboardImage
    {
        public static Image CreateForTopThree(IEnumerable<Player> players)
        {
            var image = new Image();
            
            var topThreePlayers = players.OrderBy(p => p.Score).Take(3);
            foreach (var (player, index) in topThreePlayers.WithIndex())
            {
                var name = player.Name.ToUpper();
                var score = player.Score;
                var line = $"{score} - {name}";
                var lineLength = (Image.Width - line.Length) / 2;

                image.SetText(2, index + 2, line);
            }
            
            return image;
        }

        private static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
    }
}