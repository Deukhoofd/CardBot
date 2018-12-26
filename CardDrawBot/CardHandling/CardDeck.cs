using System;
using System.Collections.Generic;
using System.Linq;

namespace CardDrawBot.CardHandling
{
    public class CardDeck
    {
        private readonly List<PlayingCard> _allCards;
        private Stack<PlayingCard> _cards;
        public bool AutoReshuffle { get; set; }

        public CardDeck()
        {
            _allCards = new List<PlayingCard>();
            var enumIterator = Enum.GetValues(typeof(CardColor));
            foreach (CardColor color in enumIterator)
            {
                for (var i = 1; i < 14; i++)
                {
                    _allCards.Add(new PlayingCard(color, i));
                }
            }
            Shuffle();
        }

        public void Shuffle()
        {
            var random = new Random();
            var randomCards = _allCards.OrderBy(x => random.Next());
            _cards = new Stack<PlayingCard>(randomCards);
        }

        public PlayingCard GetNext()
        {
            if (_cards.Count == 0)
            {
                if (AutoReshuffle)
                    Shuffle();
                else
                    return null;
            }
            return _cards.Pop();
        }

        public int Count => _cards.Count;
    }
}