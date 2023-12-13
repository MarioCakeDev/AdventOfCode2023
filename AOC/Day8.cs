using System.Text.RegularExpressions;

namespace AOC;

public partial class Day8
{
    public static string Part1(string input)
    {
        string[] parts = input.Split("\r\n\r\n");
        Direction[] directions = parts[0].Select(direction => direction == 'L' ? Direction.Left : Direction.Right).ToArray();

        Dictionary<string, Path> map = parts[1].Split("\r\n")
            .Select(line =>
            {
                Match match = PathRegex().Match(line);

                return new
                {
                    Destination = match.Groups["Destination"].Value,
                    Path = new Path(match.Groups["Left"].Value, match.Groups["Right"].Value)
                };
            })
            .ToDictionary(
                line => line.Destination,
                line => line.Path
            );

        string currentPosition = "AAA";

        int totalSteps = 0;
        while (currentPosition != "ZZZ")
        {
            Path path = map[currentPosition];
            currentPosition = directions[totalSteps % directions.Length] == Direction.Left
                ? path.Left
                : path.Right;

            totalSteps++;
        }

        return totalSteps.ToString();
    }

    public static string Part2(string input)
    {
        string[] parts = input.Split("\r\n\r\n");
        Direction[] directions = parts[0].Select(direction => direction == 'L' ? Direction.Left : Direction.Right).ToArray();

        Dictionary<string, Path> map = parts[1].Split("\r\n")
            .Select(line =>
            {
                Match match = PathRegex().Match(line);

                return new
                {
                    Destination = match.Groups["Destination"].Value,
                    Path = new Path(match.Groups["Left"].Value, match.Groups["Right"].Value)
                };
            })
            .ToDictionary(
                line => line.Destination,
                line => line.Path
            );

        string[] currentPositions = map.Keys.Where(key => key.EndsWith('A')).ToArray();

        return currentPositions.Select(currentPosition =>
            {
                long currentStep = 0;
                string startingPosition = currentPosition;

                do
                {
                    Path path = map[currentPosition];
                    currentPosition = directions[currentStep % directions.Length] == Direction.Left
                        ? path.Left
                        : path.Right;

                    currentStep++;
                } while (currentStep % directions.Length != 0 && currentPosition != startingPosition);

                return currentStep;
            })
            .Aggregate(1L, (total, cycleTime) => total * cycleTime)
            .ToString();
    }

    private record Path(string Left, string Right);

    private enum Direction
    {
        Left,
        Right
    }

    [GeneratedRegex(@"(?<Destination>[A-Z]{3}) = \((?<Left>[A-Z]{3}), (?<Right>[A-Z]{3})\)")]
    private static partial Regex PathRegex();
}