using System;
using System.Collections.Generic;
using System.Text;
using Leaf.xNet;

namespace vk_music_downloader
{
    class Auth
    {
        private string Login { get; set; }
        private string Password { get; set; }

        public Auth(string Login, string Password)
        {
            this.Login = Login;
            this.Password = Password;
        }

        private HttpResponse GetAurh()
        {
            HttpRequest request = new HttpRequest();
            request.UserAgentRandomize();
            request.KeepAlive = true;
        }
    }
}
