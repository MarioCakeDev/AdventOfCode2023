using System.Text.RegularExpressions;

namespace AOC;

public class Day5
{
    public static string Part1(string input)
    {
        long[]? seeds = null;
        string[] groups = input.Split("\r\n\r\n");

        Dictionary<string, Conversion> lookup = new();
        for (int index = 0; index < groups.Length; index++)
        {
            string group = groups[index];
            if (index == 0)
            {
                seeds = Regex.Matches(group, "(?<Seeds>\\d+)").Select(match => long.Parse(match.Groups["Seeds"].Value)).ToArray();
                continue;
            }

            List<ConversionRange> conversionRanges = new();
            string[] conversions = group.Split("\r\n");
            for (int conversionIndex = 0; conversionIndex < conversions.Length; conversionIndex++)
            {
                string conversion = conversions[conversionIndex];
                if (conversionIndex == 0)
                {
                    Match match = Regex.Match(conversion, "(?<From>.*?)-to-(?<To>.*?) ");
                    string from = match.Groups["From"].Value;
                    string to = match.Groups["To"].Value;
                    lookup[from] = new Conversion(to, conversionRanges);
                    continue;
                }

                long[] ranges = conversion.Split(" ").Select(long.Parse).ToArray();

                long destination = ranges[0];
                long source = ranges[1];
                long length = ranges[2];

                conversionRanges.Add(new ConversionRange(
                        new LongRange(source, source + length),
                        new LongRange(destination, destination + length)
                    )
                );
            }
        }

        return seeds!.Select(seed =>
        {
            string currentConversion = "seed";
            long lookupValue = seed;
            while (lookup.TryGetValue(currentConversion, out Conversion? conversion))
            {
                long innerLookupValue = lookupValue;
                ConversionRange? conversionRange = conversion.Ranges.FirstOrDefault(ranges =>
                    innerLookupValue >= ranges.SourceRange.Start &&
                    innerLookupValue <= ranges.SourceRange.End
                );

                if (conversionRange is not null)
                {
                    lookupValue = conversionRange.DestinationRange.Start + lookupValue - conversionRange.SourceRange.Start;
                }
                currentConversion = conversion.To;
            }

            return lookupValue;
        })
            .Min()
            .ToString();
    }

    public static string Part2(string input)
    {
        return Part1(input);
    }

    private record Conversion(string To, List<ConversionRange> Ranges);
    private record ConversionRange(LongRange SourceRange, LongRange DestinationRange);

    private record LongRange(long Start, long End);
}