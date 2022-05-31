using Microsoft.AspNetCore.Mvc;
using TelegramPollBot.Models;

namespace TelegramPollBot.Controllers
{

    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly Bot _bot;

        public HomeController(Bot bot)
        {
            _bot = bot;
        }

        [HttpGet]
        [Route("/index")]
        public string Index()
        {
            return "It`s my telegram bot:)";
        }
    }
}
