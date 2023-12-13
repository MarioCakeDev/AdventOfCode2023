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

                Dictionary<int, int> batching = cards.GroupBy(card => card)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Count()
                    )
                    .GroupBy(pair => pair.Value)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Count()
                    );

                int[] someOfAKind = Enumerable.Range(0, cards.Length)
                    .Select(index => batching.GetValueOrDefault(cards.Length - index, 0))
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
        return input.Split("\r\n")
            .Select(gameString =>
            {
                string[] parts = gameString.Split(" ");
                char[] cards = parts[0].ToCharArray();
                string bid = parts[1];

                int jokers = cards.Count(card => card == 'J');

                Dictionary<int, int> batching = cards
                    .Where(card => card != 'J')
                    .GroupBy(card => card)
                    .Select(group => group.Count())
                    .OrderByDescending(cardCount => cardCount)
                    .Select(cardCount =>
                    {
                        int countWithJokers = Math.Min(cards.Length, cardCount + jokers);
                        jokers = cardCount + jokers - countWithJokers;
                        return countWithJokers;
                    })
                    .GroupBy(cardCount => cardCount)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Count()
                    );

                if (jokers == cards.Length)
                {
                    batching[cards.Length] = 1;
                }

                int[] someOfAKind = Enumerable.Range(0, cards.Length)
                    .Select(index => batching.GetValueOrDefault(cards.Length - index, 0))
                    .ToArray();

                return new Game(new Score(someOfAKind, cards, true), int.Parse(bid));
            })
            .OrderByDescending(game => game.Score)
            .Select((game, index) => game.Bid * (index + 1))
            .Sum()
            .ToString();
    }

    private record Game(Score Score, int Bid);

    private record Score(int[] CardGroupCounts, char[] OriginalCards, bool WithJokers = false) : IComparable<Score>
    {
        public int CompareTo(Score? other)
        {
            if (other is null)
            {
                return -1;
            }

            foreach ((int OurCards, int OtherCards) kinds in CardGroupCounts.Zip(other.CardGroupCounts).Take(OriginalCards.Length - 1))
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

        private static int? Compare(int our, int other)
        {
            if (our > other)
            {
                return -1;
            }

            if (our < other)
            {
                return 1;
            }

            return null;
        }

        private int CardAsNumber(char card)
        {
            if (WithJokers && card == 'J')
            {
                return 0;
            }

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