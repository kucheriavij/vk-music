using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace VkMusicDownloader.Decoder
{
    public abstract class AlAudioBase
    {
        protected string ApiUrl = "https://vk.com/al_audio.php";
        private string Cookies { get; set; }
        private string Uid { get; set; }
        protected string PlaylistId = "-1";

        protected AlAudioBase()
        {
            var config = Config.ReadConfig();
            Cookies = CookiesSaveReadFile.ReadCookies();
            Uid = (string) config["uid"];
        }

        protected Dictionary<string, string> LoadData(string offset = "0")
        {
            var dictionary = new Dictionary<string, string>
            {
                ["access_hash"] = "",
                ["act"] = "load_section",
                ["al"] = "1",
                ["claim"] = "0",
                ["offset"] = offset,
                ["owner_id"] = Uid,
                ["playlist_id"] = PlaylistId,
                ["type"] = "playlist"
            };

            return dictionary;
        }

        protected Dictionary<string, string> ReloadData(Array ids)
        {
            var dictionary = new Dictionary<string, string>
            {
                ["act"] = "reload_audio",
                ["al"] = "1",
                ["ids"] = string.Join(",", ids)
            };

            return dictionary;
        }
    }
}