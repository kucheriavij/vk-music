using Microsoft.Extensions.CommandLineUtils;
using System;

namespace VkMusicDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineApplication app = new CommandLineApplication();

            CommandLineApplication auth = app.Command("auth", config =>
            {
                config.Description = "Login in https://vk.com";
                config.HelpOption("-? | -h | --help");

                CommandOption login = config.Option("-u", "Your email or phone number", CommandOptionType.SingleValue);
                CommandOption password = config.Option("-p", "Your password", CommandOptionType.SingleValue);
                CommandOption uid = config.Option("-uid", "Vk user id", CommandOptionType.SingleValue);

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

                    string _login = login.Value();
                    string _password = password.Value();
                    Config.CreateConfig(uid.Value());

                    try
                    {
                        var auth = new VkAuth(_login, _password);
                        Console.Out.WriteLine(auth.Cookie);

                        switch (auth.CheckAuth())
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
                    catch (Exception E)
                    {
                        Console.Error.WriteLine($"Login faild. Check login or password. Err: {E}");

                        return 0;
                    }
                });
            });

            CommandLineApplication music = app.Command("music", config =>
            {
                config.Description = "Get music from you vk";
                config.HelpOption("-? | -h | --help");

                CommandOption list = config.Option("-l | --list", "Music list", CommandOptionType.NoValue);

                config.OnExecute(() =>
                {
                    if (!list.HasValue())
                    {
                        config.ShowHelp("music");
                        return 0;
                    }



                    return 0;
                });
            });

            app.HelpOption("-? | -h | --help");
            var result = app.Execute(args);
            Environment.Exit(result);
        }
    }
}
