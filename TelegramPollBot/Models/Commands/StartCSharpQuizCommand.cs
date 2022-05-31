using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramPollBot.Models.Commands
{
    public class StartCSharpQuizCommand : Command
    {
        public override string Name => "/startCSharpQuiz";

        public StartCSharpQuizCommand()
        {
            Questions = new List<Question>
            {
                new Question
                {
                    Id = 1,
                    Title = "C# це об'єктно орієнтована мова прогрвмування?",
                    AnswerOptions = new List<Answer>
                    {
                        new Answer
                        {
                            Name = "yes",
                            IsCorrect = true,
                        },
                        new Answer
                        {
                            Name = "no",
                            IsCorrect = false,
                        }
                    }
                },
                new Question
                {
                    Id = 2,
                    Title = "Вкажіть оператор отримання типу об'єкта: ?",
                    AnswerOptions = new List<Answer>
                    {
                        new Answer
                        {
                            Name = "typeof",
                            IsCorrect = true,
                        }
                    }
                }, new Question
                {
                    Id = 3,
                    Title = "Який модифікатор доступу робить код доступним лише в межах одного класу ?",
                    AnswerOptions = new List<Answer>
                    {
                        new Answer
                        {
                            Name = "private",
                            IsCorrect = true,
                        },
                        new Answer
                        {
                            Name = "protected",
                            IsCorrect = false,
                        },
                        new Answer
                        {
                            Name = "internal",
                            IsCorrect = false,
                        }
                    }
                }
            };
        }
        public IList<Question> Questions { get; set; }

        public Dictionary<long, int> UserQuestionDictionary = new();
        public override async Task Execute(Update update, TelegramBotClient client)
        {
            long userId = GetUserId(update);
            var chatId = GetChatId(update);

            BotStateMachine.SetCurrentCommand(userId, Name);

            if (BotStateMachine.GetCurrentStep(userId) == 0)
            {

                await client.SendTextMessageAsync(chatId, Questions[0].Title, replyMarkup: MakeYesNoKeyboard());
                BotStateMachine.NextStep(userId);
                return;
            }
            if (BotStateMachine.GetCurrentStep(userId) == 1)
            {
                if (update.Type == UpdateType.CallbackQuery)
                {
                    string? answer = update.CallbackQuery.Data;

                    Questions[0].Answer = answer;
                    await client.SendTextMessageAsync(chatId, Questions[1].Title);
                    BotStateMachine.NextStep(userId);
                }
                
                return;
            }
            if (BotStateMachine.GetCurrentStep(userId) == 2)
            {
                string? answer = update.Message.Text;
                Questions[1].Answer = answer;

                var option1Button = MakeInlineButton("private", "private");
                var option2Button = MakeInlineButton("protected", "protected");
                var option3Button = MakeInlineButton("internal", "internal");

                await client.SendTextMessageAsync(chatId, Questions[2].Title, replyMarkup: MakeInlineKeyboard(option1Button, option2Button,option3Button));

                BotStateMachine.NextStep(userId);
                return;
            }

            if (BotStateMachine.GetCurrentStep(userId) == 3)
            {
                if (update.Type == UpdateType.CallbackQuery)
                {
                    string? answer = update.CallbackQuery.Data;
                    Questions[2].Answer = answer;

                    await client.SendTextMessageAsync(chatId, "Quiz finished. Thanks!");

                    await SendQuizTotalScore(client, chatId);
                    BotStateMachine.FinishCurrentCommand(userId);
                }
            }
        }

        private async Task SendQuizTotalScore(TelegramBotClient client, long chatId)
        {
            int rightAnswersCount =
                Questions.Count(t => t.AnswerOptions.Any(o => o.Name == t.Answer && o.IsCorrect));
            int questionsCount = Questions.Count;

            await client.SendTextMessageAsync(chatId, $"Your score {rightAnswersCount} of {questionsCount}");
        }

        public static InlineKeyboardMarkup MakeYesNoKeyboard()
        {
            var yesButton = MakeInlineButton("Так", "yes");
            var noButton = MakeInlineButton("Ні", "no");
            return MakeInlineKeyboard(yesButton, noButton);
        }
        public static InlineKeyboardButton MakeInlineButton(string text, string callBackData)
        {
            return new InlineKeyboardButton(text)
            {
                CallbackData = callBackData
            };
        }

        public static InlineKeyboardMarkup MakeInlineKeyboard(params InlineKeyboardButton[] buttons)
        {
            return new InlineKeyboardMarkup(buttons);
        }
    }

    public class Question
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public IList<Answer> AnswerOptions { get; set; }

        public string Answer { get; set; }
    }

    public class Answer
    {
        public string Name { get; set; }
        public bool IsCorrect { get; set; }
    }
}
