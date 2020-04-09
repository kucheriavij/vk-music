﻿using System;
using System.Collections.Generic;
using System.Text;
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
        private string Ip_H { get; set; }
        private string Lg_H { get; set; }
        public string Cookie { get; set; }
        private string UID { get; set; }

        public VkAuth(string Login, string Password)
        {
            JObject config = Config.ReadConfig();
            this.Login = Login;
            this.Password = Password;
            UID = (string)config["uid"];
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
            HttpResponse AuthData = GetAuth();
            Ip_H = System.Text.RegularExpressions.Regex.Match(AuthData.ToString(), "name=\"ip_h\" value=\"(.*?)\"").Groups[1].Value;
            Lg_H = System.Text.RegularExpressions.Regex.Match(AuthData.ToString(), "name=\"lg_h\" value=\"(.*?)\"").Groups[1].Value;
            Cookie = AuthData.Cookies.GetCookieHeader(VkLoginPage);
        }

        private string Auth()
        {
            HttpRequest request = new HttpRequest();
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("Cookie", Cookie);
            request.AddHeader("Referer", VkLoginPage);
            request.UserAgentRandomize();
            request.KeepAlive = false;

            RequestParams Params = new RequestParams();
            Params["act"] = "login";
            Params["role"] = "al_frame";
            Params["_origin"] = VkMainPage;
            Params["ip_h"] = Ip_H;
            Params["lg_h"] = Lg_H;
            Params["email"] = Login;
            Params["pass"] = Password;

            string response = request.Post(VkLoginUrl, Params).ToString();
            Cookie = request.Cookies.GetCookieHeader(VkMainPage + "/id" + UID);

            CookiesSaveReadFile.WriteCookies(Cookie);

            return response;
        }

        public int CheckAuth()
        {
            string Auths = Auth();
            int result = 0;

            if (System.Text.RegularExpressions.Regex.Match(Auths, "Notifier.init((.*))").Groups[1].Value.Length > 0)
            {
                result = 1;
            }
            else if (Auths.Contains("try{parent.location.href='/login?act=authcheck'}catch(e){}"))
            {
                result = 2;
            }
            else if (Auths.Contains("parent.stManager.add(['notifier.js', 'notifier.css'], function() {"))
            {
                result = 0;
            }

            return result;
        }
    }
}
