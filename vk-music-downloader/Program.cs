using Microsoft.Extensions.CommandLineUtils;
using System;
using HtmlAgilityPack;
using System.Text;

namespace VkMusicDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();

            var auth = app.Command("auth", config =>
            {
                config.Description = "Login in https://vk.com";
                config.HelpOption("-? | -h | --help");

                var login = config.Option("-u", "Your email or phone number", CommandOptionType.SingleValue);
                var password = config.Option("-p", "Your password", CommandOptionType.SingleValue);
                var uid = config.Option("-uid", "Vk user id", CommandOptionType.SingleValue);

                config.OnExecute(() =>
                {
                    if (!login.HasValue() && !password.HasValue() && !uid.HasValue())
                    {
                        config.ShowHelp("auth");
                        return 0;
                    }

                    if (!login.HasValue())
                    {
                        Console.Error.WriteLine("Login is required");
                    }

                    if (!password.HasValue())
                    {
                        Console.Error.WriteLine("Password is required");
                    }
                    
                    Config.CreateConfig(uid.Value());

                    try
                    {
                        var vkAuth = new VkAuth(login.Value(), password.Value());
                        Console.Out.WriteLine(vkAuth.Cookie);

                        switch (vkAuth.CheckAuth())
                        {
                            case 1:
                                Console.Out.WriteLine("You are logged");
                                break;
                            case 2:
                                Console.Out.WriteLine("You have a two factor authorization");
                                break;
                            case 0:
                                Console.Error.WriteLine("You login or password incorrect");
                                break;
                        }

                        return 0;
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine($"Login faild. Check login or password. Err: {e}");

                        return 0;
                    }
                });
            });

            var music = app.Command("music", config =>
            {
                config.Description = "Get music from you vk";
                config.HelpOption("-? | -h | --help");

                var list = config.Option("-l | --list", "Music list", CommandOptionType.NoValue);

                config.OnExecute(() =>
                {
                    if (!list.HasValue())
                    {
                        config.ShowHelp("music");
                        return 0;
                    }
                    
                    var m = new Music();
                    m.GetAudioList();

                    return 0;
                });
            });

            app.HelpOption("-? | -h | --help");
            var result = app.Execute(args);
            Environment.Exit(result);
        }
    }
}
