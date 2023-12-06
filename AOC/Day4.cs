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
        return Part1(input);
    }
}