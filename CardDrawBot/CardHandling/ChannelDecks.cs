using System.Collections.Generic;

namespace CardDrawBot.CardHandling
{
    public static class ChannelDecks
    {
        private static readonly Dictionary<ulong, CardDeck> Decks = new Dictionary<ulong, CardDeck>();

        public static CardDeck GetDeck(ulong channelId)
        {
            // If a deck exists for this channel, return that.
            if (Decks.TryGetValue(channelId, out var deck))
                return deck;
            // Otherwise create a new one, save it, and return it.
            deck = new CardDeck();
            Decks.Add(channelId, deck);
            return deck;
        }
    }
}