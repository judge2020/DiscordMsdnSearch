using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordMsdn
{
    class Program
    {
        private string token;

        static void Main(string[] args)
        {
            new Program().Start();
        }

        private DiscordClient _client;

        private void Start()
        {
            _client = new DiscordClient(builder =>
            {
                builder.AppName = "MsdnBot";
                builder.AppUrl = "https://msdn.judge2020.com";
                builder.LogLevel = LogSeverity.Info;
                builder.LogHandler = LogHandler;
            });

            _client.UsingCommands(builder =>
            {
                builder.PrefixChar = char.Parse("+");
                builder.AllowMentionPrefix = true;
            });


            if (File.Exists("token.txt"))
            {
                token = File.ReadAllText("token.txt");
            }
            else
            {
                Console.WriteLine("Please insert token:");
                token = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Accepted token, trying to connect.");
            }
            createCommands();
            _client.ExecuteAndWait(async () =>
            {
                try
                {
                    await _client.Connect(token, TokenType.Bot);
                }
                catch (Exception e)
                {
                    
                    Console.WriteLine("Unable to connect: " + e.Message);
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                
            });

        }

        private void createCommands()
        {
            var service = _client.GetService<CommandService>();
            service.CreateCommand("ping")
                .Description("Pings")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Slaps " + e.User.Mention + " for pinging me.");
                });

            service.CreateCommand("msdn")
                .Description("Searched MSDN")
                .Parameter("arg1", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("in development: " + e.GetArg("arg1"));
                });
        }


        private void LogHandler(object sender, LogMessageEventArgs logMessageEventArgs)
        {
            Console.WriteLine(logMessageEventArgs.Message + " (" + logMessageEventArgs.Severity + ") [" + logMessageEventArgs.Source + "]" );
        }
    }
}
