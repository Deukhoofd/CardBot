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
                await MessageHandler(arg);
            }
            catch (Exception e)
            {
                Utilities.Log(LogSeverity.Critical, e.ToString());
            }
        }

        private static async Task MessageHandler(SocketMessage arg)
        {
                        var splitString = arg.Content.Split(" ");
            var command = splitString[0];
            if (string.Equals(command, "!shuffle", StringComparison.InvariantCultureIgnoreCase))
            {
                Deck.Shuffle();
                arg.Channel.SendEmbed("Card Handler", "Cards reshuffled");
            }
            else if (string.Equals(command, "!autoreshuffle", StringComparison.InvariantCultureIgnoreCase))
            {
                Deck.AutoReshuffle = !Deck.AutoReshuffle;
                arg.Channel.SendEmbed("Card Handler",
                    Deck.AutoReshuffle
                        ? "The deck will now be automatically reshuffled when empty"
                        : "The deck will no longer be automatically reshuffled when empty");
            }
            else if (string.Equals(command, "!draw", StringComparison.InvariantCultureIgnoreCase))
            {
                var card = Deck.GetNext();
                if (card == null)
                {
                    arg.Channel.SendEmbed("Card Handler",
                        "No cards left in the deck. Use !shuffle to reshuffle the deck.");
                }
                else
                {
                    var imageUrl =
                        $"https://raw.githubusercontent.com/hayeah/playing-cards-assets/master/png/{card.GetPrettyNumberString().ToLowerInvariant()}_of_{card.Color.ToString().ToLowerInvariant()}.png";
                    var message = $"{arg.Author.Mention} drew a card. It's a {card.GetPrettyString()}.";
                    if (Deck.Count == 0)
                    {
                        message += " *That was the last card! **!shuffle** the deck before drawing the next card!*";
                    }
                    arg.Channel.SendEmbed("Card Handler", message, imageUrl);
                }
            }
            else if (string.Equals(command, "!roll", StringComparison.CurrentCultureIgnoreCase))
            {
                var thrown = DiceThrower.Roll(splitString.Length == 1 ? null : splitString[1]);
                if (thrown != null)
                {
                    arg.Channel.SendEmbed("Dice Roll",
                        $"{arg.Author.Mention} threw {thrown.NumberOfDice} dice with a max value of {thrown.MaxValue}. The result was: {string.Join(", ", thrown.Results)}.");
                }
            }
        }

        private static async Task SendEmbed(this ISocketMessageChannel channel, string title, string message, string imageUrl = null)
        {
            var eb = new EmbedBuilder
            {
                Title = title, Color = Color.Gold, Description = message, Timestamp = DateTimeOffset.Now, ImageUrl = imageUrl
            };
            await channel.SendMessageAsync("", embed: eb.Build());
        }

        private static async Task OnLog(LogMessage arg)
        {
            await Utilities.Log(arg.Severity, arg.Message);
        }
    }
}