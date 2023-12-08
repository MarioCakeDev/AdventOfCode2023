using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AOC;

public static class DayRunner
{
    public static void Run(int times, int skip = 0)
    {
        Type[] validDayTypes = typeof(DayRunner).Assembly.GetTypes()
            .Where(type => Regex.IsMatch(type.Name, "^Day([1-9]|[1-2][0-9])$"))
            .Where(type =>
                type.GetMethod("Part1") is { IsStatic: true } part1 && part1.ReturnParameter.ParameterType == typeof(string) && part1.GetParameters() is [{ } part1Parameter] && part1Parameter.ParameterType == typeof(string) &&
                type.GetMethod("Part2") is { IsStatic: true } part2 && part2.ReturnParameter.ParameterType == typeof(string) && part2.GetParameters() is [{ } part2Parameter] && part2Parameter.ParameterType == typeof(string)
            ).OrderBy(type => int.Parse(type.Name[3..])).ToArray();

        foreach (Type dayType in validDayTypes.Skip(skip))
        {
            MethodInfo part1 = dayType.GetMethod("Part1")!;
            MethodInfo part2 = dayType.GetMethod("Part2")!;
            string input = File.ReadAllText(dayType.Name.ToLower() + ".txt");
            RunAndCalculateAverage(part1.CreateDelegate<PartDelegate>(), input, times, $"Day {dayType.Name[3..]}, Part 1");
            RunAndCalculateAverage(part2.CreateDelegate<PartDelegate>(), input, times, $"Day {dayType.Name[3..]}, Part 2");
        }
    }

    private delegate string PartDelegate(string input);

    private static void RunAndCalculateAverage(PartDelegate func, string input, int times, string label)
    {
        double totalMicroseconds = 0;

        string result = "";
        for (int i = 0; i < times; i++)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            result = func(input);
            stopwatch.Stop();
            totalMicroseconds += stopwatch.Elapsed.TotalMicroseconds;
        }

        double averageMicroseconds = totalMicroseconds / times;
        Console.WriteLine($"{label}: {averageMicroseconds:F2}µs (average over {times} runs), Result: {result}");
    }
}