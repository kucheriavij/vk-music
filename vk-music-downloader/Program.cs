using Microsoft.Extensions.CommandLineUtils;
using System;

namespace VkMusicDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineApplication app = new CommandLineApplication();

            CommandLineApplication auth = app.Command("auth", config => {
                config.Description = "Login or logout in https://vk.com";
                config.HelpOption("-? | -h | --help");
                config.OnExecute(() => {
                    config.ShowHelp("auth");
                    return 0;
                });
            });

            auth.Command("login", config => {
                config.Description = "Login in https://vk.com";
                config.HelpOption("-? | -h | --help");

                CommandArgument login = config.Argument("login", "Your email or phone number", false);
                CommandArgument password = config.Argument("password", "Your password");

                config.OnExecute(() => {
                    string _login = login.Value;
                    string _passworg = password.Value;

                    if(string.IsNullOrWhiteSpace(_login) && string.IsNullOrWhiteSpace(_passworg))
                    {
                        Console.Out.WriteLine($"Arguments {login.Name} and {password.Name} not set. Please enter your email and passwor or run auth -h");
                        
                        return 0;
                    }

                    try
                    {
                        var auth = new VkAuth(_login, _passworg);
                        string GetInfo = auth.Auth();

                        switch (auth.CheckAuth(GetInfo))
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

            app.HelpOption("-? | -h | --help");
            var result = app.Execute(args);
            Environment.Exit(result);
        }
    }
}
