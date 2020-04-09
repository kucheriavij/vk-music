using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VkMusicDownloader
{
    static class Config
    {
        const string ConfigFileName = "config.json";
        public static void CreateConfig(string uid = "")
        {
            JObject config = new JObject(
                new JProperty("uid", uid)
            );

            File.WriteAllText(@ConfigFileName, config.ToString());
        }

        public static JObject ReadConfig()
        {
            return JObject.Parse(File.ReadAllText(@ConfigFileName));
        }
    }
}
