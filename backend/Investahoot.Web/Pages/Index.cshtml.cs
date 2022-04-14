using Investahoot.Model.Models;
using Investahoot.Model.Vestaboard;
using Investahoot.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Investahoot.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly VestaboardService _vestaboardService;

        public IndexModel(ILogger<IndexModel> logger, VestaboardService vestaboardService)
        {
            _logger = logger;
            _vestaboardService = vestaboardService;
        }

        public async Task OnGet()
        {
        }
    }
}