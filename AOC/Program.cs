using System.Diagnostics;
using AOC;

string day1Input = File.ReadAllText("day1.txt");
string day2Input = File.ReadAllText("day2.txt");
string day3Input = File.ReadAllText("day3.txt");

Stopwatch stopwatch = Stopwatch.StartNew();
Console.WriteLine("Day 1, Part 1: {0}, {1}ms", Day1.Part1(day1Input), stopwatch.ElapsedMilliseconds);
stopwatch.Restart();
Console.WriteLine("Day 1, Part 2: {0}, {1}ms", Day1.Part2(day1Input), stopwatch.ElapsedMilliseconds);
stopwatch.Restart();
Console.WriteLine("Day 2, Part 1: {0}, {1}ms", Day2.Part1(day2Input), stopwatch.ElapsedMilliseconds);
stopwatch.Restart();
Console.WriteLine("Day 2, Part 2: {0}, {1}ms", Day2.Part2(day2Input), stopwatch.ElapsedMilliseconds);
stopwatch.Restart();
Console.WriteLine("Day 3, Part 1: {0}, {1}ms", Day3.Part1(day3Input), stopwatch.ElapsedMilliseconds);
stopwatch.Restart();
Console.WriteLine("Day 3, Part 2: {0}, {1}ms", Day3.Part2(day3Input), stopwatch.ElapsedMilliseconds);
stopwatch.Restart();
