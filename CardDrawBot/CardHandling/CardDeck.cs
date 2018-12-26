using System;
using System.Collections.Generic;
using System.Linq;

namespace CardDrawBot.CardHandling
{
    public class CardDeck
    {
        /// <summary>
        /// An array containing all cards possibly available.
        /// </summary>
        private static PlayingCard[] _allCards;
        private readonly Random _random = new Random();

        private Stack<PlayingCard> _cards;
        public bool AutoReshuffle { get; set; }

        public CardDeck()
        {
            // If we don't know available cards yet.
            if (_allCards == null)
            {
                // Instantiate an array for the cards. As we know the exact size of the deck, we can just use this.
                _allCards = new PlayingCard[52];
                // Grab the different values for the colors enum, so we can iterate over them.
                var enumIterator = Enum.GetValues(typeof(CardColor));
                // Iterate over the colors.
                foreach (CardColor color in enumIterator)
                {
                    // for 1 (ace), to 14 (king), create a new card
                    for (var i = 1; i < 14; i++)
                    {
                        // Place this in the appropriate position
                        _allCards[((int) color) * 13 + (i - 1)] = new PlayingCard(color, i);
                    }
                }
            }
            // Create a shuffled deck.
            Shuffle();
        }

        public void Shuffle()
        {
            // Order the cards by a random value.
            var randomCards = _allCards.OrderBy(x => _random.Next());
            // And create a stack of cards. We use a stack as we only ever want to grab the topmost card.
            _cards = new Stack<PlayingCard>(randomCards);
        }

        public PlayingCard GetNext()
        {
            // If the deck is empty
            if (_cards.Count == 0)
            {
                // If auto reshuffling is enabled, reshuffle, otherwise return null.
                if (AutoReshuffle)
                    Shuffle();
                else
                    return null;
            }
            // return the topmost card.
            return _cards.Pop();
        }

        public int Count => _cards.Count;
    }
}