using System.Diagnostics;

namespace AOC;

public static class DayRunner
{
    public static void Run(int times)
    {
        RunAndCalculateAverage(Day1.Part1, File.ReadAllText("day1.txt"), times, "Day 1, Part 1");
        RunAndCalculateAverage(Day1.Part2, File.ReadAllText("day1.txt"), times, "Day 1, Part 2");
        RunAndCalculateAverage(Day2.Part1, File.ReadAllText("day2.txt"), times, "Day 2, Part 1");
        RunAndCalculateAverage(Day2.Part2, File.ReadAllText("day2.txt"), times, "Day 2, Part 2");
        RunAndCalculateAverage(Day3.Part1, File.ReadAllText("day3.txt"), times, "Day 3, Part 1");
        RunAndCalculateAverage(Day3.Part2, File.ReadAllText("day3.txt"), times, "Day 3, Part 2");
    }

    private static void RunAndCalculateAverage(Func<string, string> func, string input, int times, string label)
    {
        double totalMilliseconds = 0;

        for (int i = 0; i < times; i++)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            string result = func(input);
            stopwatch.Stop();
            totalMilliseconds += stopwatch.Elapsed.TotalMilliseconds;
        }

        double averageMilliseconds = totalMilliseconds / times;
        Console.WriteLine($"{label}: {averageMilliseconds.ToString("F2")}ms (average over {times} runs)");
    }
}