using System.Text.RegularExpressions;

namespace AOC;

public class Day1
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
        Dictionary<string, int> numbersAsText = new()
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 }
        };
        Regex numberRegex = new($"(?=({string.Join("|", numbersAsText.Keys)}|\\d))", RegexOptions.ECMAScript | RegexOptions.Multiline);
        return input.Split("\n").Select(row =>
        {
            MatchCollection matches = numberRegex.Matches(row);
            return int.Parse($"{AsNumber(matches.First().Groups[1].Value)}{AsNumber(matches.Last().Groups[1].Value)}");
        }).Sum().ToString();

        string AsNumber(string input)
        {
            if (input.Length == 1)
            {
                return input;
            }

            return numbersAsText[input].ToString();
        }
    }
}