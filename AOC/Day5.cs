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
        Seed[]? seeds = null;
        string[] groups = input.Split("\r\n\r\n");

        Dictionary<string, Conversion> lookup = new();
        for (int index = 0; index < groups.Length; index++)
        {
            string group = groups[index];
            if (index == 0)
            {
                seeds = Regex.Matches(group, "(?<Seeds>\\d+)").Select(match => long.Parse(match.Groups["Seeds"].Value))
                    .Chunk(2)
                    .Select(seedInfo => new Seed(new LongRange(seedInfo[0], seedInfo[0] + seedInfo[1])))
                    .ToArray();
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

        return seeds!.SelectMany(seed =>
        {
            string currentConversion = "seed";
            LongRange[] ranges = { seed.Range };
            while (lookup.TryGetValue(currentConversion, out Conversion? conversion))
            {
                IEnumerable<LongRange> innerRanges = ranges;
                ConversionRange[] conversionRanges = conversion.Ranges.Where(range => HasOverlaps(range, ranges))
                    .ToArray();

                IEnumerable<LongRange> rangesWithoutOverlaps = innerRanges.Where(range => !conversionRanges.All(conversionRange => conversionRange.SourceRange.Overlaps(range)));
                ranges = innerRanges.SelectMany(range =>
                    {
                        List<LongRange> ranges = [range];
                        IEnumerable<ConversionRange> overlappingConversionRanges = conversion.Ranges.Where(conversionRange => conversionRange.SourceRange.Overlaps(range))
                            .OrderBy(conversionRange => conversionRange.SourceRange.Start);

                        foreach (ConversionRange overlappingConversionRange in overlappingConversionRanges)
                        {
                            List<LongRange> newRanges = [];
                            LongRange lastRange = ranges[^1];
                            if (lastRange.Start < overlappingConversionRange.SourceRange.Start)
                            {
                                newRanges.Add(new LongRange(
                                    lastRange.Start,
                                    overlappingConversionRange.SourceRange.Start - 1
                                ));
                            }

                            newRanges.Add(new LongRange(
                                Math.Max(lastRange.Start, overlappingConversionRange.SourceRange.Start),
                                Math.Min(lastRange.End, overlappingConversionRange.SourceRange.End)
                            ));

                            if (lastRange.End > overlappingConversionRange.SourceRange.End)
                            {
                                newRanges.Add(new LongRange(
                                    overlappingConversionRange.SourceRange.End + 1,
                                    lastRange.End
                                ));
                            }

                            ranges.RemoveAt(ranges.Count - 1);
                            ranges.AddRange(newRanges);
                        }

                        return ranges;
                    })
                    .Union(rangesWithoutOverlaps)
                    .OrderBy(range => range.Start)
                    .ToArray();
                currentConversion = conversion.To;
            }

            return ranges.Select(range => range.Start);
        })
            .Min()
            .ToString();
    }

    private static bool HasOverlaps(ConversionRange conversionRange, LongRange[] ranges)
    {
        return ranges.Any(range => range.Overlaps(conversionRange.SourceRange));
    }

    private record Conversion(string To, List<ConversionRange> Ranges);

    private record ConversionRange(LongRange SourceRange, LongRange DestinationRange)
    {
        public long Map(long value)
        {
            return DestinationRange.Start + value - SourceRange.Start;
        }
    }

    private record LongRange(long Start, long End)
    {
        public long Length { get; } = End - Start;
        public bool Overlaps(LongRange other)
        {
            return Start <= other.End &&
                   End >= other.Start;
        }

        public bool Inside(LongRange other)
        {
            return other.Includes(Start) && other.Includes(End);
        }

        public bool Includes(long value)
        {
            return value >= Start && value <= End;
        }
    }
    private record Seed(LongRange Range);
}