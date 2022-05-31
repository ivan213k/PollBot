using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramPollBot.Models.Commands
{
    public class StartCommand : Command
    {
        public override string Name { get => "/start"; }
        public override async Task Execute(Update update, TelegramBotClient client)
        {
            var chatId = GetChatId(update);
            var answer = $"Вас вітає Poll_Bot {new Emoji(0x1F60A)} \n" +
                         "Цей бот допоможе перевірити знання в сфері IT за напрямком .NET!\n" +
                         "Список команд: \n" +
                         "/startCSharpQuiz - Пройти опитування на знання мови програмування C#\n" +
                         "/help - Довідкова інформація";

            await client.SendTextMessageAsync(chatId, answer);
        }
    }
}
