using System;
using System.Globalization;

namespace CardDrawBot.CardHandling
{
    public static class CardEmoji
    {
        public static string GetEmoji(this PlayingCard card)
        {
            var str = "";
            // All card emoji of the same color are in the same Unicode value row, so grab that row.
            switch (card.Color)
            {
                case CardColor.Clubs:
                    str += "1F0D";
                    break;
                case CardColor.Diamonds:
                    str += "1F0C";
                    break;
                case CardColor.Hearts:
                    str += "1F0B";
                    break;
                case CardColor.Spades:
                    str += "1F0A";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Grab the number of the card.
            var cardNumber = card.Number;
            // the card at position 12 in unicode is the knight, which is not used in general games, so we handle that.
            if (cardNumber > 11)
                cardNumber++;

            // Get the hex value of the number
            var toHex = cardNumber.ToString("X");
            // And create the full character index,
            var toCompleteNum = str + toHex;
            // Parse this index as an integer.
            var parsed = int.Parse(toCompleteNum, NumberStyles.HexNumber);
            // And convert it to a unicode character.
            return char.ConvertFromUtf32(parsed);
        }
    }
}