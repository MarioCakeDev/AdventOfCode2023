using System.Globalization;

namespace AOC;

public class Day4
{
    public static string Part1(string input)
    {
        return input.Split("\r\n")
            .Select(row =>
            {
                int startIndex = row.IndexOf(':', StringComparison.Ordinal);
                string[] parts = row[(startIndex + 2)..].ToString().Split(" | ", 2);
                int[] winningNumbers = parts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                int[] cardNumbers = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                return cardNumbers.Intersect(winningNumbers).ToArray();
            })
            .Select(numbers => Math.Floor(Math.Pow(2, numbers.Length - 1)))
            .Sum()
            .ToString(CultureInfo.InvariantCulture);
    }

    public static string Part2(string input)
    {
        Dictionary<int, int> additionalCards = new();
        int totalCards = 0;
        int[] totalNumbers = input.Split("\r\n")
            .Select(row =>
            {
                int startIndex = row.IndexOf(':', StringComparison.Ordinal);
                string[] parts = row[(startIndex + 2)..].ToString().Split(" | ", 2);
                int[] winningNumbers = parts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                int[] cardNumbers = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                return winningNumbers.Intersect(cardNumbers).ToArray().Length;
            })
            .ToArray();

        for (int index = 0; index < totalNumbers.Length; index++)
        {
            additionalCards.TryGetValue(index, out int cards);
            cards += 1;
            totalCards += cards;
            int numbers = totalNumbers[index];
            for (int card = 0; card < numbers; card++)
            {
                if (additionalCards.TryGetValue(card + index + 1, out int additional))
                {
                    additionalCards[card + index + 1] = additional + cards;
                }
                else
                {
                    additionalCards[card + index + 1] = cards;
                }
            }
        }

        return totalCards.ToString();
    }
}