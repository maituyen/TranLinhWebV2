using System;
using Telegram.Bot;

namespace MyProject.Helpers
{
	public class Social
    {
		public static void Buzz(string Message,  string ChatID = "-643989120")
		{

			SocialTelegram.Buzz(Message, ChatID);
		}
	}
}