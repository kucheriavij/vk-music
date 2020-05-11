using System;
using System.Text.RegularExpressions;
using Leaf.xNet;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;

namespace VkMusicDownloader
{
    class Music
    {
        const string VkMusicPageUrl = "https://m.vk.com/audio";
        private string Cookie { get; set; }
        private string Uid { get; set; }
        private int TotalCount { get; set; }

        public Music()
        {
            JObject config = Config.ReadConfig();
            Cookie = CookiesSaveReadFile.ReadCookies();
            Uid = (string)config["uid"];

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(GetMusicPage().ToString());
            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'audioPage__count')]");
            var value = Regex.Match(node.InnerHtml, @"\d+").Value;
            TotalCount = Convert.ToInt32(value);
        }

        private HttpResponse GetMusicPage(int offset = 0)
        {
            HttpRequest request = new HttpRequest();
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("Cookie", Cookie);
            request.UserAgentRandomize();
            request.KeepAlive = false;

            RequestParams requestParams = new RequestParams
            {
                ["offset"] = offset,
                ["next_from"] = null
            };

            HttpResponse response = request.Get(VkMusicPageUrl, requestParams);

            return response;
        }

        public void GetAudioList()
        {
            for (int i = 0; i < TotalCount; i += 100)
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(GetMusicPage(i).ToString());
                var nodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'ai_label')]");

                foreach (var node in nodes)
                {
                    var children = node.ChildNodes;
                    string artistName = "";
                    string trackName = "";

                    foreach (var child in children)
                    {
                        if (child.HasClass("ai_title"))
                        {
                            trackName = child.InnerHtml.Trim();
                        }

                        if (child.HasClass("ai_artist"))
                        {
                            artistName = child.InnerHtml.Trim();
                        }
                    }

                    if (trackName.Length > 0 && artistName.Length > 0)
                    {
                        Console.Out.WriteLine(System.Net.WebUtility.HtmlDecode(artistName) + " - " + System.Net.WebUtility.HtmlDecode(trackName));
                    }
                }
            }
        }
    }
}
