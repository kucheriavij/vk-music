using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VkMusicDownloader
{
    /// <inheritdoc />
	/// <summary>
	/// обрабатываем cookie в файле
	/// </summary>
    class CookiesSaveReadFile
    {
        /// <summary>
        /// сохраняем cookie в файл
        /// </summary>
        /// <param name="cookies">
        /// cookies
        /// </param>
        /// <param name="file">
        /// имя файла
        /// </param>
        public static void WriteCookies(string cookies, string file = "cookies.bin")
        {
            using (Stream stream = File.Create(file))
            {
                try
                {
                    Console.Out.Write("Writing cookies to disk... ");
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, cookies);
                    Console.Out.WriteLine("Done.");
                }
                catch (Exception E)
                {
                    Console.Out.WriteLine("Problem writing cookies to disk: " + E.GetType());
                }
            }
        }

        /// <summary>
        /// считываем cookie из файла
        /// </summary>
        /// <param name="file">
        /// имя файла
        /// </param>
        public static string ReadCookies(string file = "cookies.bin")
        {
            try
            {
                using (Stream stream = File.Open(file, FileMode.Open))
                {
                    Console.Out.Write("Reading cookies from disk... ");
                    BinaryFormatter formatter = new BinaryFormatter();
                    Console.Out.WriteLine("Done.");

                    return formatter.Deserialize(stream).ToString();
                }
            }
            catch (Exception E)
            {
                Console.Out.WriteLine("Problem reading cookies from disk: " + E.GetType());

                return "";
            }
        }
    }
}
