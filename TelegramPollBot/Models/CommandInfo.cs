namespace TelegramPollBot.Models
{
    public class CommandInfo
    {
        public string Name { get; set; }

        public int CurrentStep { get; set; }

        public CommandInfo(string name)
        {
            Name = name;
            CurrentStep = 0;
        }
        public CommandInfo(string name, int currentStep)
        {
            Name = name;
            CurrentStep = currentStep;
        }
    }
}
