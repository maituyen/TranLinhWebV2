using System;
using System.Net;
using System.Text;

namespace MyProject.Models
{
	public class LoginGGAuth
	{ 
        public const string clientId = "100199549480-8fpnsvdn92rai17tnvtljuiitjcf645o.apps.googleusercontent.com";
        public const string clientSecret = "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";
        string scopes = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";
        public static string redirectUri = "https://localhost:5000/GGOauth2Callback";
        public LoginGGAuth()
		{
		}
        public static Uri GetAutenticationURI()
        { 
            string scopes = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";

            if (string.IsNullOrEmpty(redirectUri))
            {
                redirectUri = "urn:ietf:wg:oauth:2.0:oob";
            }
            string oauth = string.Format("https://accounts.google.com/o/oauth2/auth?client_id={0}&redirect_uri={1}&scope={2}&response_type=code", clientId, redirectUri, scopes);
            return new Uri(oauth);
        }
        public static void Exchange(string authCode, string clientid, string secret, string redirectURI)
        {

            var request = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");

            string postData = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code", authCode, clientid, secret, redirectURI);
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //var x = AuthResponse.get(responseString);

            //x.clientId = clientid;
            //x.secret = secret;

            //return x;

        }

    }
}

