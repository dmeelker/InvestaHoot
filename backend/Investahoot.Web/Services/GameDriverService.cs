using Investahoot.Model;

namespace Investahoot.Web.Services
{
    public class GameDriverService : BackgroundService
    {
        private readonly GameManager _game;

        public GameDriverService(GameManager game)
        {
            _game = game;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                _game.CheckIfTimeHasElapsed();
                await Task.Delay(10);
            }
        }
    }
}
