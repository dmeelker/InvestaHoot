using Investahoot.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Investahoot.Web.Pages.Backend
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            var questions = new QuestionLoader().LoadQuestions("questions.json");
        }
    }
}
