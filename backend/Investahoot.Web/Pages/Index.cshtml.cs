﻿using Investahoot.Model.Models;
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
            var textMessage = new VestaboardTextMessage("hoi");
            await _vestaboardService.SendTextMessage(textMessage);

            var imageMessage = new VestaboardCharacterMessage(new Image());
            await _vestaboardService.SendImageMessage(imageMessage);
        }
    }
}