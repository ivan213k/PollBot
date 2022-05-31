using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramPollBot.Models;
using TelegramPollBot.Models.Commands;

namespace TelegramPollBot.Controllers
{
    
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly Bot _bot;

        public MessageController(Bot bot)
        {
            _bot = bot;
        }

        [HttpPost]
        [Route("api/webhook")]
        public async Task<IActionResult> Update(Update update)
        {
            var commands = Bot.Commands;
            if (update.Type == UpdateType.Message)
            {
                long userId = update.Message.From.Id;
                string? currentCommandName = BotStateMachine.GetCurrentCommand(userId);
                Command? command;
                if (currentCommandName != null)
                {
                    command = commands.SingleOrDefault(t => String.Equals(t.Name, currentCommandName, StringComparison.CurrentCultureIgnoreCase));
                }
                else
                {
                    command = commands.SingleOrDefault(t => t.Name.ToLower() == update.Message.Text?.ToLower());
                }

                if (command != null)
                {
                    await command.Execute(update, _bot.Client);
                }
                else
                {
                    await _bot.Client.SendTextMessageAsync(update.Message.Chat.Id, $"Команду не розпізнано {new Models.Emoji(0x1F61E)}");
                }
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                var userId = update.CallbackQuery.From.Id;
                var command = commands.FirstOrDefault(r => r.Name == BotStateMachine.GetCurrentCommand(userId));
                if (command != null)
                {
                    await command.Execute(update, _bot.Client);
                }
            }
            return Ok();
        }

    }
}
