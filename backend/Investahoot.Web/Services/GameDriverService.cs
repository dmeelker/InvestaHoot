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
            await _game.Reset();

            while (true)
            {
                await _game.Update();
                await Task.Delay(1000);
            }
        }
    }
}
