using System;

namespace CardDrawBot
{
    public static class DiceThrower
    {
        private static readonly Random Random = new Random();

        public static DiceResult Roll(string message)
        {
            // If the argument passed is insensible, return a random number between 1 and 6.
            if (string.IsNullOrWhiteSpace(message))
            {
                return new DiceResult()
                {
                    Results      = new[] {Random.Next(0, 6) + 1},
                    MaxValue     = 6,
                    NumberOfDice = 1
                };
            }
            // Otherwise split the argument by the character 'd'.
            var split = message.ToLowerInvariant().Split('d');
            switch (split.Length)
            {
                // If we can't split the string, return a random number between 1 and 6.
                case 0:
                    return new DiceResult()
                    {
                        Results = new[] {Random.Next(0, 6) + 1},
                        MaxValue = 6,
                        NumberOfDice = 1
                    };
                // If the splitting by 'd' only results in a single result, and that result is a number bigger than 0.
                case 1 when int.TryParse(split[0], out var v) && v > 0:
                    // Return a random number between 1 and the value passed.
                    return new DiceResult()
                    {
                        Results = new[] {Random.Next(0, v) + 1},
                        MaxValue = v,
                        NumberOfDice = 1
                    };
                // If splitting by 'd' results in two values.
                case 2:
                {
                    // If the first result is a number, we consider this the number of dice that need to be thrown. This
                    // needs to be bigger than 0, and smaller or equal to 1000. If this is not the case, return null.
                    if (!int.TryParse(split[0], out var diceNum) || diceNum <= 0 || diceNum > 1000)
                        return null;
                    // The second result is the max value a die can have. Check if this is a number and bigger than 0, otherwise
                    // return null.
                    if (!int.TryParse(split[1], out var max) || max <= 0)
                        return null;
                    // Instantiate an array with the required size.
                    var arr = new int[diceNum];
                    // Generate random values for each die we need to throw.
                    for (var i = 0; i < diceNum; i++)
                    {
                        arr[i] = Random.Next(0, max) + 1;
                    }
                    // and return the result.
                    return new DiceResult()
                    {
                        Results = arr,
                        MaxValue = max,
                        NumberOfDice = diceNum
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