using System.Text.RegularExpressions;

namespace AOC;

public partial class Day1
{
    public static string Part1(string input)
    {
        Regex numberRegex = new("\\d");
        return input.Split("\n").Select(row =>
        {
            MatchCollection matches = numberRegex.Matches(row);
            return int.Parse($"{matches.First().Value}{matches.Last().Value}");
        }).Sum().ToString();
    }

    public static string Part2(string input)
    {
        return input.Split("\r\n").Select(row =>
        {
            MatchCollection matches = NumberRegex().Matches(row);
            int tens = AsNumber(matches[0].Groups[1].Value);
            int ones = AsNumber(matches[^1].Groups[1].Value);
            return tens * 10 + ones;
        }).Sum().ToString();

        int AsNumber(string input)
        {
            if (input.Length == 1)
            {
                return int.Parse(input);
            }

            char firstChar = input[0];
            char secondChar = input[1];
            switch (firstChar)
            {
                case 'o': return 1;
                case 'e': return 8;
                case 'n': return 9;
                case 't': return secondChar == 'w' ? 2 : 3;
                case 'f': return secondChar == 'o' ? 4 : 5;
                case 's': return secondChar == 'i' ? 6 : 7;
            }

            throw new Exception();
        }
    }

    [GeneratedRegex("(?=(one|two|three|four|five|six|seven|eight|nine|\\d))", RegexOptions.Compiled)]
    private static partial Regex NumberRegex();
}