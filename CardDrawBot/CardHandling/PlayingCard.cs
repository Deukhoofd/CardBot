namespace CardDrawBot.CardHandling
{
    public class PlayingCard
    {
        public PlayingCard(CardColor color, int number)
        {
            Color = color;
            Number = number;
        }

        public CardColor Color { get; }
        public int Number { get; }

        public string GetPrettyString()
        {
            return $"{this.GetEmoji()} ({GetPrettyNumberString()} of {Color})";
        }

        public string GetPrettyNumberString()
        {
            switch (Number)
            {
                case 1:
                    return "Ace";
                case 11:
                    return "Jack";
                case 12:
                    return "Queen";
                case 13:
                    return "King";
                default:
                    return Number.ToString();
            }
        }
    }
}