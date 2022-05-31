using System.Reflection;
using Telegram.Bot;
using TelegramPollBot.Models.Commands;

namespace TelegramPollBot.Models
{
    public class Bot
    {
        private TelegramBotClient _botClient;

        private static List<Command> _commands;

        public static IReadOnlyList<Command> Commands { get => _commands.AsReadOnly(); }
        public Bot(IConfiguration configuration)
        {
            string token = configuration.GetSection("bot_settings")["Token"];
            string appUrl = configuration.GetSection("bot_settings")["Url"];
            string webhookUrl = $"{appUrl}/api/webhook";

            Client = new TelegramBotClient(token);
            InitCommands();
            Client.SetWebhookAsync(webhookUrl).Wait();
        }

        private void InitCommands()
        {
            _commands = new List<Command>();
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(Command));
            foreach (var type in types)
            {
                _commands.Add(Activator.CreateInstance(type) as Command);
            }
        }

        public TelegramBotClient Client { get => _botClient; set => _botClient = value; }
    }
}
