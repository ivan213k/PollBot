using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramPollBot.Models.Commands
{
    public class HelpCommand : Command
    {
        public override string Name { get => "/help"; }
        public override async Task Execute(Update update, TelegramBotClient client)
        {
            long chatId = GetChatId(update);
            var answer = $"Вас вітає Poll_Bot {new Emoji(0x1F60A)} \n" +
                         "Маркетингові комунікації\n" +
                         "Developer: Zaharuk Ivan\n" +
                         "501 група"; 
            
            await client.SendTextMessageAsync(chatId, answer);
        }
    }
}
