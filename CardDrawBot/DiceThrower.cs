using System;

namespace CardDrawBot
{
    public static class DiceThrower
    {
        private static readonly Random Random = new Random();

        public static DiceResult Roll(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return null;
            var split = message.ToLowerInvariant().Split('d');
            switch (split.Length)
            {
                case 0:
                    return new DiceResult()
                    {
                        Results = new[] {Random.Next(0, 6) + 1},
                        MaxValue = 6,
                        NumberOfDice = 1
                    };
                case 1 when int.TryParse(split[0], out var v) && v > 0:
                    return new DiceResult()
                    {
                        Results = new[] {Random.Next(0, v) + 1},
                        MaxValue = v,
                        NumberOfDice = 1
                    };

                case 2:
                {
                    if (!int.TryParse(split[0], out var diceNum) || diceNum <= 0 || diceNum > 1000)
                        return null;
                    if (!int.TryParse(split[1], out var max) || max <= 0)
                        return null;
                    var arr = new int[diceNum];
                    for (var i = 0; i < diceNum; i++)
                    {
                        arr[i] = Random.Next(0, max) + 1;
                    }
                    return new DiceResult()
                    {
                        Results = arr,
                        MaxValue = diceNum,
                        NumberOfDice = max + 1
                    };
                }
                default:
                    return null;
            }
        }

        public class DiceResult
        {
            public int[] Results { get; set; }
            public int MaxValue { get; set; }
            public int NumberOfDice { get; set; }
        }
    }
}