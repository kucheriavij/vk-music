﻿using System;
using System.Collections.Generic;
using System.Text;
using Leaf.xNet;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;

namespace VkMusicDownloader
{
    class Music
    {
        const string VkMusicPageUrl = "https://vk.com/audios";
        private string Cookie { get; set; }
        private string Uid { get; set; }

        public Music()
        {
            JObject config = Config.ReadConfig();
            Cookie = CookiesSaveReadFile.ReadCookies();
            Uid = (string)config["uid"];
        }

        public HttpResponse GetMusicPage()
        {
            HttpRequest request = new HttpRequest();
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("Cookie", Cookie);
            request.UserAgentRandomize();
            request.KeepAlive = false;

            HttpResponse response = request.Get(VkMusicPageUrl + Uid);

            return response;
        }

        private HtmlDocument LoadVkMusicPage()
        {
            HtmlWeb web = new HtmlWeb();

            return web.Load(VkMusicPageUrl + Uid);
        }

        public HtmlNodeCollection GetAudioList(string pattern = "//*[@id=\"content\"]/div/div[2]/div[2]/div[2]/div/div/div[3]/div[1]/div[1]/div[2]/div[1]")
        {
            return LoadVkMusicPage().DocumentNode.SelectNodes(pattern);
        }
    }
}
