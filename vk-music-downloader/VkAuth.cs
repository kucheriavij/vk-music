using Leaf.xNet;
using Newtonsoft.Json.Linq;

namespace VkMusicDownloader
{
    class VkAuth
    {
        const string VkLoginPage = "https://vk.com/login";
        const string VkMainPage = "https://vk.com";
        const string VkLoginUrl = "https://login.vk.com/?act=login";
        private string Login { get; set; }
        private string Password { get; set; }
        private string IpH { get; set; }
        private string LgH { get; set; }
        public string Cookie { get; set; }
        private string Uid { get; set; }

        public VkAuth(string login, string password)
        {
            JObject config = Config.ReadConfig();
            Login = login;
            Password = password;
            Uid = (string)config["uid"];
            ParseDataAuth();
        }

        private HttpResponse GetAuth()
        {
            HttpRequest request = new HttpRequest();
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("Host", "vk.com");
            request.UserAgentRandomize();
            request.KeepAlive = false;

            HttpResponse response = request.Get(VkLoginPage);

            return response;
        }

        private void ParseDataAuth()
        {
            HttpResponse authData = GetAuth();
            IpH = System.Text.RegularExpressions.Regex.Match(authData.ToString(), "name=\"ip_h\" value=\"(.*?)\"").Groups[1].Value;
            LgH = System.Text.RegularExpressions.Regex.Match(authData.ToString(), "name=\"lg_h\" value=\"(.*?)\"").Groups[1].Value;
            Cookie = authData.Cookies.GetCookieHeader(VkLoginPage);
        }

        private string Auth()
        {
            HttpRequest request = new HttpRequest();
            request.AddHeader("Accept","text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("Cookie", Cookie);
            request.AddHeader("Referer", VkLoginPage);
            request.UserAgentRandomize();
            request.KeepAlive = false;

            RequestParams requestParams = new RequestParams
            {
                ["act"] = "login",
                ["role"] = "al_frame",
                ["_origin"] = VkMainPage,
                ["ip_h"] = IpH,
                ["lg_h"] = LgH,
                ["email"] = Login,
                ["pass"] = Password
            };

            string response = request.Post(VkLoginUrl, requestParams).ToString();
            Cookie = request.Cookies.GetCookieHeader(VkMainPage + "/id" + Uid);

            CookiesSaveReadFile.WriteCookies(Cookie);

            return response;
        }

        public int CheckAuth()
        {
            string auths = Auth();
            int result = 0;

            if (System.Text.RegularExpressions.Regex.Match(auths, "Notifier.init((.*))").Groups[1].Value.Length > 0)
            {
                result = 1;
            }
            else if (auths.Contains("try{parent.location.href='/login?act=authcheck'}catch(e){}"))
            {
                result = 2;
            }
            else if (auths.Contains("parent.stManager.add(['notifier.js', 'notifier.css'], function() {"))
            {
                result = 0;
            }

            return result;
        }
    }
}
