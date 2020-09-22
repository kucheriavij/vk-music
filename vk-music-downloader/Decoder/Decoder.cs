using System;

namespace VkMusicDownloader.Decoder
{
    public class Decoder
    {
        protected int Uid;
        private string _n = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMN0PQRSTUVWXYZO123456789+/=";

        public Decoder(int uid)
        {
            Uid = uid;
        }
    }
}