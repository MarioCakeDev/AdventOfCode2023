namespace AOC;

public class Day7
{
    public static string Part1(string input)
    {
        return input.Split("\r\n")
            .Select(gameString =>
            {
                string[] parts = gameString.Split(" ");
                char[] cards = parts[0].ToCharArray();
                string bid = parts[1];

                Dictionary<int, char[]> batching = cards.GroupBy(card => card)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Count()
                    )
                    .GroupBy(pair => pair.Value)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(g => g.Key).ToArray()
                    );

                char[][] someOfAKind = Enumerable.Range(0, cards.Length)
                    .Select(index => batching.TryGetValue(cards.Length - index, out char[]? batchCards)
                        ? batchCards
                        : [])
                    .ToArray();

                return new Game(new Score(someOfAKind, cards), int.Parse(bid));
            })
            .OrderByDescending(game => game.Score)
            .Select((game, index) => game.Bid * (index + 1))
            .Sum()
            .ToString();
    }

    public static string Part2(string input)
    {
        return Part1(input);
    }

    private record Game(Score Score, int Bid);

    private record Score(char[][] CardsCollection, char[] OriginalCards) : IComparable<Score>
    {
        public int CompareTo(Score? other)
        {
            if (other is null)
            {
                return -1;
            }

            foreach ((char[] OurCards, char[] OtherCards) kinds in CardsCollection.Zip(other.CardsCollection).Take(OriginalCards.Length - 1))
            {
                int? comparison = Compare(kinds.OurCards, kinds.OtherCards);
                if (comparison is not null)
                {
                    return comparison.Value;
                }
            }

            IEnumerable<int> otherSinglesSortedBySize = other.OriginalCards.Select(CardAsNumber);
            IEnumerable<int> ourSinglesSortedBySize = OriginalCards.Select(CardAsNumber);

            foreach ((int ourCard, int otherCard) in ourSinglesSortedBySize.Zip(otherSinglesSortedBySize))
            {
                if (ourCard > otherCard)
                {
                    return -1;
                }

                if (ourCard < otherCard)
                {
                    return 1;
                }
            }

            return 0;
        }

        private static int? Compare(IReadOnlyCollection<char> our, IReadOnlyCollection<char> other)
        {
            if (our.Count > other.Count)
            {
                return -1;
            }

            if (our.Count < other.Count)
            {
                return 1;
            }

            return null;
        }

        private static int CalculateValue(IEnumerable<char> cards)
        {
            return cards.Sum(CardAsNumber);
        }

        private static int CardAsNumber(char card)
        {
            return card switch
            {
                '2' => 1,
                '3' => 2,
                '4' => 3,
                '5' => 4,
                '6' => 5,
                '7' => 6,
                '8' => 7,
                '9' => 8,
                'T' => 9,
                'J' => 10,
                'Q' => 11,
                'K' => 12,
                'A' => 13,
                _ => 0
            };
        }
    }
}