namespace TelegramPollBot.Models
{
    public static class BotStateMachine
    {
        private static readonly Dictionary<long, CommandInfo> UserCurrentCommands = new();

        public static string? GetCurrentCommand(long userId)
        {
            return UserCurrentCommands.TryGetValue(userId, out CommandInfo? value) ? value.Name : null;
        }

        public static void SetCurrentCommand(long userId, string commandName)
        {
            if (!UserCurrentCommands.ContainsKey(userId))
            {
                UserCurrentCommands.Add(userId, new CommandInfo(commandName));
            }
        }

        public static void FinishCurrentCommand(long userId)
        {
            UserCurrentCommands.Remove(userId);
        }

        public static int GetCurrentStep(long userId)
        {
            return UserCurrentCommands[userId].CurrentStep;
        }

        public static void NextStep(long userId)
        {
            UserCurrentCommands[userId].CurrentStep += 1;
        }
        public static void PreviousStep(long userId)
        {
            UserCurrentCommands[userId].CurrentStep -= 1;
        }
    }
}
