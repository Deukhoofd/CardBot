using System;
using System.Globalization;

namespace CardDrawBot.CardHandling
{
    public static class CardEmoji
    {
        public static string GetEmoji(this PlayingCard card)
        {
            var str = "";
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

            var cardNumber = card.Number;
            if (cardNumber > 11)
                cardNumber++;
            var toHex = cardNumber.ToString("X");
            var toCompleteNum = str + toHex;
            var parsed = int.Parse(toCompleteNum, NumberStyles.HexNumber);
            return char.ConvertFromUtf32(parsed);
        }
    }
}