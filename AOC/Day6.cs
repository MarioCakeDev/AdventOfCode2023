using System.Text.RegularExpressions;

namespace AOC;

public class Day6
{
    public static string Part1(string input)
    {
        string[] races = input.Split("\r\n");
        long[] times = Regex.Matches(races[0], "(?<Time>\\d+)").Select(match => long.Parse(match.Groups["Time"].Value)).ToArray();
        long[] distances = Regex.Matches(races[1], "(?<Distance>\\d+)").Select(match => long.Parse(match.Groups["Distance"].Value)).ToArray();
        return times.Zip(distances, Race.FromTuple)
            .Select(race =>
            {
                long speed = race.Time / 2;
                long leftTime = race.Time - speed;

                long upperBounds = race.Time;
                long lowerBounds = leftTime;

                while (!(speed * leftTime > race.DistanceToBeat && (speed - 1) * (leftTime + 1) <= race.DistanceToBeat))
                {
                    if (speed * leftTime > race.DistanceToBeat)
                    {
                        lowerBounds = leftTime;
                        leftTime = (upperBounds + leftTime) / 2;
                        speed = race.Time - leftTime;
                    }
                    else
                    {
                        upperBounds = leftTime;
                        leftTime = (lowerBounds + leftTime) / 2;
                        speed = race.Time - leftTime;
                    }
                }

                return Math.Abs(leftTime+1 - (long)Math.Ceiling(race.Time / 2.0)) * 2 - (race.Time % 2 == 0 ? 1 : 0);
            })
            .Aggregate(1L, (totalWins, currentWins) => totalWins * currentWins)
            .ToString();
    }

    public static string Part2(string input)
    {
        return Part1(input.Replace(" ", ""));
    }

    private record Race(long Time, long DistanceToBeat)
    {
        public static Race FromTuple(long time, long distance)
        {
            return new Race(time, distance);
        }
    }
}