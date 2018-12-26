using System;
using System.IO;
using System.Threading.Tasks;
using CardDrawBot.CardHandling;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CardDrawBot
{
    internal static class Program
    {
        private static void Main()
        {
            MainAsync().Wait();
        }

        private static readonly CardDeck Deck = new CardDeck();

        private static async Task MainAsync()
        {
            var data = File.ReadAllText("secrets.json");
            var obj = (JObject) JsonConvert.DeserializeObject(data);

            var client = new DiscordSocketClient();
            client.Log += OnLog;
            client.Ready += () => Utilities.Log(LogSeverity.Info, "Ready");
            client.MessageReceived += OnMessage;

            await client.LoginAsync(TokenType.Bot, obj["client_token"].Value<string>());

            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private static async Task OnMessage(SocketMessage arg)
        {
            try
            {
                await MessageHandler.Handle(arg);
            }
            catch (Exception e)
            {
                Utilities.Log(LogSeverity.Critical, e.ToString());
            }
        }

        private static async Task OnLog(LogMessage arg)
        {
            await Utilities.Log(arg.Severity, arg.Message);
        }
    }
}