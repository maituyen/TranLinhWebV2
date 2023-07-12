using System;
using Telegram.Bot;

namespace MyProject.Helpers
{
	public class SocialTelegram
	{
		public static void Buzz(string message = "", string ChatID = "-643989120")
		{
			var botToken = "5252418358:AAH_7R6CxAxPuYFj3Ubm044FMNl3ZLF3Igo";
			var botClient = new TelegramBotClient(botToken);
			botClient.SendTextMessageAsync(ChatID, message);
		}
	}
}