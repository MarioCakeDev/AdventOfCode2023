using System.Text.RegularExpressions;

namespace AOC;

public class Day2
{
    public static string Part1(string input)
    {
        const int maxRedCubes = 12;
        const int maxGreenCubes = 13;
        const int maxBlueCubes = 14;

        Regex gameRegex = new("Game (?<GameId>\\d+):");
        Regex cubesRegex = new("(((?<Cubes>\\d+) (?<CubeType>red|green|blue)))");
        return input.Split("\n")
            .Select(game => new Game(
                int.Parse(gameRegex.Match(game).Groups["GameId"].Value),
                game.Split(":")[1].Split(";")
                    .Select(round =>
                    {
                        MatchCollection cubeMatches = cubesRegex.Matches(round);
                        int redCubes = int.Parse(cubeMatches.FirstOrDefault(match => match.Groups["CubeType"].Value == "red")?.Groups["Cubes"].Value ?? "0");
                        int greenCubes = int.Parse(cubeMatches.FirstOrDefault(match => match.Groups["CubeType"].Value == "green")?.Groups["Cubes"].Value ?? "0");
                        int blueCubes = int.Parse(cubeMatches.FirstOrDefault(match => match.Groups["CubeType"].Value == "blue")?.Groups["Cubes"].Value ?? "0");
                        return new Round(redCubes, greenCubes, blueCubes);
                    }).ToArray()
            ))
            .Where(game => game.Rounds.All(round => round is { RedCubes: <= maxRedCubes, GreenCubes: <= maxGreenCubes, BlueCubes: <= maxBlueCubes }))
            .Sum(game => game.Id)
            .ToString();
    }

    public static string Part2(string input)
    {
        Regex gameRegex = new("Game (?<GameId>\\d+):");
        Regex cubesRegex = new("(((?<Cubes>\\d+) (?<CubeType>red|green|blue)))");
        return input.Split("\n")
            .Select(game => new Game(
                int.Parse(gameRegex.Match(game).Groups["GameId"].Value),
                game.Split(":")[1].Split(";")
                    .Select(round =>
                    {
                        MatchCollection cubeMatches = cubesRegex.Matches(round);
                        int redCubes = int.Parse(cubeMatches.FirstOrDefault(match => match.Groups["CubeType"].Value == "red")?.Groups["Cubes"].Value ?? "0");
                        int greenCubes = int.Parse(cubeMatches.FirstOrDefault(match => match.Groups["CubeType"].Value == "green")?.Groups["Cubes"].Value ?? "0");
                        int blueCubes = int.Parse(cubeMatches.FirstOrDefault(match => match.Groups["CubeType"].Value == "blue")?.Groups["Cubes"].Value ?? "0");
                        return new Round(redCubes, greenCubes, blueCubes);
                    }).ToArray()
            ))
            .Select(game => new Round(
                    game.Rounds.Max(round => round.RedCubes),
                    game.Rounds.Max(round => round.GreenCubes),
                    game.Rounds.Max(round => round.BlueCubes)
                )
            )
            .Sum(round => round.RedCubes * round.GreenCubes * round.BlueCubes)
            .ToString();
    }

    private record Game(int Id, Round[] Rounds);

    private record Round(int RedCubes, int GreenCubes, int BlueCubes);
}