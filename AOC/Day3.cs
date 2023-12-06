using System.Text.RegularExpressions;

namespace AOC;

public class Day3
{
    public static string Part1(string input)
    {
        Regex numberRegex = new("(?<Number>\\d+)");
        Regex symbolRegex = new("(?<Symbol>[^.\\d])");
        Row[] rows = input.Split("\r\n")
            .Select(row =>
            {
                Number[] numbers = numberRegex.Matches(row).Select(match =>
                    new Number(
                        int.Parse(match.Groups["Number"].Value),
                        new Range(
                            match.Groups["Number"].Index,
                            match.Groups["Number"].Value.Length + match.Groups["Number"].Index - 1
                        )
                    )
                ).ToArray();

                Symbol[] symbols = symbolRegex.Matches(row).Select(match =>
                    new Symbol(match.Groups["Symbol"].Value, match.Groups["Symbol"].Index))
                .ToArray();

                return new Row(numbers, symbols);
            }).ToArray();

        return rows.SelectMany((row, rowIndex) =>
            {
                List<Row> adjacentRows = new() { row };
                if (rowIndex != 0)
                {
                    adjacentRows.Add(rows[rowIndex - 1]);
                }

                if (rowIndex != rows.Length - 1)
                {
                    adjacentRows.Add(rows[rowIndex + 1]);
                }

                int[] indices = adjacentRows
                    .SelectMany(adjacentRow => adjacentRow.Symbols.Select(symbol => symbol.Index))
                    .ToArray();

                return row.Numbers.Where(number =>
                    indices.Any(index => index >= number.Range.Start.Value - 1 && index <= number.Range.End.Value + 1)
                );
            })
            .Sum(number => number.Value)
            .ToString();
    }

    public static string Part2(string input)
    {
        Regex numberRegex = new("(?<Number>\\d+)");
        Regex symbolRegex = new("(?<Symbol>[^.\\d])");
        Row[] rows = input.Split("\r\n")
            .Select(row =>
            {
                Number[] numbers = numberRegex.Matches(row).Select(match =>
                    new Number(
                        int.Parse(match.Groups["Number"].Value),
                        new Range(
                            match.Groups["Number"].Index,
                            match.Groups["Number"].Value.Length + match.Groups["Number"].Index - 1
                        )
                    )
                ).ToArray();

                Symbol[] symbols = symbolRegex.Matches(row).Select(match =>
                    new Symbol(match.Groups["Symbol"].Value, match.Groups["Symbol"].Index))
                .ToArray();

                return new Row(numbers, symbols);
            }).ToArray();

        return rows.SelectMany((row, rowIndex) =>
            {
                List<Row> adjacentRows = new() { row };
                if (rowIndex != 0)
                {
                    adjacentRows.Add(rows[rowIndex - 1]);
                }

                if (rowIndex != rows.Length - 1)
                {
                    adjacentRows.Add(rows[rowIndex + 1]);
                }

                Number[] numbers = adjacentRows
                    .SelectMany(row => row.Numbers)
                    .ToArray();

                return row.Symbols
                    .Where(s => s.Value == "*")
                    .Select(symbol => numbers.Where(number =>
                        symbol.Index >= number.Range.Start.Value - 1 && symbol.Index <= number.Range.End.Value + 1
                    ).ToArray())
                    .ToArray();
            })
            .Where(numbers => numbers.Length > 1)
            .Sum(numbers => numbers.Aggregate(1, (a, b) => a*b.Value))
            .ToString();
    }

    private record Number(int Value, Range Range);
    private record Symbol(string Value, int Index);
    private record Row(Number[] Numbers, Symbol[] Symbols);
}