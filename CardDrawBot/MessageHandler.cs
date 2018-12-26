using System;
using System.Threading.Tasks;
using CardDrawBot.CardHandling;
using Discord;
using Discord.WebSocket;

namespace CardDrawBot
{
    public static class MessageHandler
    {
        private static readonly CardDeck Deck = new CardDeck();
        private const char ControlCharacter = '!';

        public static async Task Handle(SocketMessage socketMessage)
        {
            // Split the string by white space.
            var splitString = socketMessage.Content.Split(" ");
            // Grab the first word of the line.
            var command = splitString[0];
            // If the command word does not start with the control character, return.
            if (command[0] != ControlCharacter)
                return;
            // Grab the command without the control character.
            command = command.Substring(1, command.Length - 1);

            // If the command is shuffle, regardless of case or computer culture.
            if (string.Equals(command, "shuffle", StringComparison.InvariantCultureIgnoreCase))
            {
                // shuffle the deck.
                Deck.Shuffle();
                // And show a simple notification message back. This is async, so will fire and forget.
                socketMessage.Channel.SendEmbed("Card Handler", "Cards reshuffled");
            }
            // If the command is auto reshuffle, regardless of case or computer culture.
            else if (string.Equals(command, "autoreshuffle", StringComparison.InvariantCultureIgnoreCase))
            {
                // Toggle the auto reshuffle of the deck.
                Deck.AutoReshuffle = !Deck.AutoReshuffle;
                // And notify the channel it was changed. Fire and forget.
                socketMessage.Channel.SendEmbed("Card Handler",
                    Deck.AutoReshuffle
                        ? "The deck will now be automatically reshuffled when empty"
                        : "The deck will no longer be automatically reshuffled when empty");
            }
            // If the user wants to draw a card, regardless of computer culture or string case.
            else if (string.Equals(command, "draw", StringComparison.InvariantCultureIgnoreCase))
            {
                // Grab the next card.
                var card = Deck.GetNext();
                // If the card is null there are no cards left, so notify the channel about this.
                if (card == null)
                {
                    socketMessage.Channel.SendEmbed("Card Handler",
                        $"{socketMessage.Author.Mention} tried to draw a card, but no cards were left in the deck. Use !shuffle to reshuffle the deck.");
                }
                // Otherwise we have a valid card.
                else
                {
                    // We grab a cool image for this card from a Github repo I found.
                    var imageUrl =
                        $"https://raw.githubusercontent.com/hayeah/playing-cards-assets/master/png/{card.GetPrettyNumberString().ToLowerInvariant()}_of_{card.Color.ToString().ToLowerInvariant()}.png";
                    // We set up the basic message
                    var message = $"{socketMessage.Author.Mention} drew a card. It's a {card.GetPrettyString()}.";
                    // If the deck is empty after this card, and auto reshuffle is not enabled, also append a warning about this.
                    if (Deck.Count == 0 && !Deck.AutoReshuffle)
                    {
                        message += " *That was the last card! **!shuffle** the deck before drawing the next card!*";
                    }
                    // Send the message.
                    socketMessage.Channel.SendEmbed("Card Handler", message, imageUrl);
                }
            }
            // If the user wants to roll, regardless of case or computer culture
            else if (string.Equals(command, "roll", StringComparison.CurrentCultureIgnoreCase))
            {
                // Use the dice thrower to throw dice. If no additional argument is specified, pass null, otherwise pass the second argument.
                var thrown = DiceThrower.Roll(splitString.Length == 1 ? null : splitString[1]);
                // If the dice thrower succeeds
                if (thrown != null)
                {
                    // Return a message. We mention the user in this, along with the number of dices thrown, the max value of the dice used, and the results.
                    socketMessage.Channel.SendEmbed("Dice Roll",
                        $"{socketMessage.Author.Mention} threw {thrown.NumberOfDice} dice with a max value of {thrown.MaxValue}." +
                        $" The result was: {string.Join(", ", thrown.Results)}.");
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

    }
}