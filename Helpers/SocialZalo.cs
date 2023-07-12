using System; 
using MyProject.Data.Entities;
using System.Text;
using Telegram.Bot;
using ZaloDotNetSDK;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes; 
using ZaloCSharpSDK;

namespace MyProject.Helpers
{
	public class SocialZalo
	{
        public async void SendMessage()
        {
            ZaloAppInfo zaloAppInfo = new ZaloAppInfo(1587087796459569914, "qH4wZs6oXFmWIQJUpTRB", "");
            Zalo3rdAppClient appClient1 = new Zalo3rdAppClient(zaloAppInfo);
            string access_token = "geiTSDvZXp6bYnH-tM67CgU757hVTlb4_lm3IiWH_W7--6OQt4kR4VVQTmd2A8j6r8HrMlKKcNcCcdLMb7ojOwtxGo-0SxOYhvWeDfONr2solpa4nbdKAkJYD1NUSSaawvrI5FDwZ1dSp6KtqIIPByUuG3FYLwml-kvDC_H4lWtin7q0nYcx4z-CKWMWEAWhcuLT3PWVnbcSy5HXk4QLBvdIHoAuHh8Xnk5B4iiEqLcouXzegNZlFAlT4mYXJzuUmFGV0xaIXqExes5GeX28IjgxUqcd38v6f-TrTO1PXZYho7O9bbZf6PVB3XchTfTJgSnAN8jVjnAMsLbRJD-gUT9wXZm";

            appClient1.getProfile(access_token, ""); 
          
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+ access_token +"");
            var content = new StringContent("{\"recipient\":{\"oid\":\"oid\"},\"message\":{\"text\":\"Hello World\"}}", Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://openapi.zalo.me/v2.0/oa/message", content);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Console.ReadLine();
        }
        public async void SendMessage1()
		{
			try
			{
                ZaloClient client = new ZaloClient("geiTSDvZXp6bYnH-tM67CgU757hVTlb4_lm3IiWH_W7--6OQt4kR4VVQTmd2A8j6r8HrMlKKcNcCcdLMb7ojOwtxGo-0SxOYhvWeDfONr2solpa4nbdKAkJYD1NUSSaawvrI5FDwZ1dSp6KtqIIPByUuG3FYLwml-kvDC_H4lWtin7q0nYcx4z-CKWMWEAWhcuLT3PWVnbcSy5HXk4QLBvdIHoAuHh8Xnk5B4iiEqLcouXzegNZlFAlT4mYXJzuUmFGV0xaIXqExes5GeX28IjgxUqcd38v6f-TrTO1PXZYho7O9bbZf6PVB3XchTfTJgSnAN8jVjnAMsLbRJD-gUT9wXZm");
                string access_token = "geiTSDvZXp6bYnH-tM67CgU757hVTlb4_lm3IiWH_W7--6OQt4kR4VVQTmd2A8j6r8HrMlKKcNcCcdLMb7ojOwtxGo-0SxOYhvWeDfONr2solpa4nbdKAkJYD1NUSSaawvrI5FDwZ1dSp6KtqIIPByUuG3FYLwml-kvDC_H4lWtin7q0nYcx4z-CKWMWEAWhcuLT3PWVnbcSy5HXk4QLBvdIHoAuHh8Xnk5B4iiEqLcouXzegNZlFAlT4mYXJzuUmFGV0xaIXqExes5GeX28IjgxUqcd38v6f-TrTO1PXZYho7O9bbZf6PVB3XchTfTJgSnAN8jVjnAMsLbRJD-gUT9wXZm";
                string phoneNumber = "0939392343";
                string message = "YOUR_MESSAGE";
                JObject jObject2 =client.getProfileOfFollower(phoneNumber);
                JObject jObject1 = client.getListFollower(10, 10);
                 

                Dictionary<String, String> headers = new Dictionary<String, String>();
                headers.Add("access_token", access_token);

                JsonObject id = new JsonObject();
                id.Add("user_id", "user_id");

                JsonObject text = new JsonObject();
                text.Add("text", "text_message");

                JsonObject body = new JsonObject();
                body.Add("recipient", id);
                body.Add("message", text); 

            }
            catch (Exception ex)
			{

			}
        }
	}
}