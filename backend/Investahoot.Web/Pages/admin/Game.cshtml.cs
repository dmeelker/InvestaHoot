using Investahoot.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Investahoot.Web.Pages.admin
{
    public class GameModel : PageModel
    {
        public GameManager Game { get; }

        public GameModel(GameManager game)
        {
            Game = game;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostBeginGame()
        {
            await Game.BeginGame();
            return Redirect("/admin/game");
        }

        public async Task<IActionResult> OnPostGiveAnswer(string playerId, int answerIndex)
        {
            await Game.GiveAnswer(Guid.Parse(playerId), answerIndex);
            return Redirect("/admin/game");
        }

        public async Task<IActionResult> OnPostResetGame()
        {
            await Game.Reset();
            return Redirect("/admin/game");
        }
    }
}
