using System.Collections.Generic;
using System.Linq;
namespace Lab
{
	public class CombinationParser
    {
        
        public (Combinations, int[]) ParseCombination(IEnumerable<Card> cards)
        {
            var straight = IsInStraight(cards);
            var flush = IsInFlush(cards);
            var pairsCards = PairsCards(cards);
            var arrayValues = pairsCards.Select(f => f.Key).OrderByDescending(f => f).ToArray();
            if (pairsCards.Select(f => f.Key).OrderByDescending(f => f).First() == 15 && straight && flush)
                return (Combinations.RoyalFlush, new int[] { });
            if (straight && flush)
                return (Combinations.StraightFlush, arrayValues);
            if (pairsCards.Any(f => f.Value == 4))
                return (Combinations.Kare, arrayValues);
            if (pairsCards.Any(f => f.Value == 2) && pairsCards.Any(f => f.Value == 3))
                return (Combinations.FullHouse, new int[] { pairsCards.Where(f => f.Value == 3).Select(f => f.Key).First(), pairsCards.Where(f => f.Value == 2).Select(f => f.Key).First() });
            if (flush)
                return (Combinations.Flush, arrayValues);
            if (straight)
                return (Combinations.Straight, new int[] { arrayValues[0] });
            if (pairsCards.Any(f => f.Value == 3))
                return (Combinations.ThreeOfAKind, new int[] { pairsCards.Where(f=>f.Value == 3).Select(f=>f.Key).First(),
                      pairsCards.Select(f=>f.Key).Except(pairsCards.Where(f=>f.Value == 3).Select(f=>f.Key)).ElementAt(0),
                      pairsCards.Select(f=>f.Key).Except(pairsCards.Where(f=>f.Value == 3).Select(f=>f.Key)).ElementAt(1)});
            if (pairsCards.Where(f => f.Value == 2).Count() == 2)
                return (Combinations.TwoPairs, new int[] { pairsCards.Where(f => f.Value == 2).Select(f=>f.Key).ElementAt(0),
                  pairsCards.Where(f => f.Value == 2).Select(f=>f.Key).ElementAt(1), pairsCards.Where(f => f.Value == 1).Select(f=>f.Key).First()});


            if (pairsCards.Any(f => f.Value == 2))
            {
                int[] t = new int[4];
                t[0] = pairsCards.Where(f => f.Value == 2).Select(f => f.Key).First();
                int i = 1;
                foreach (var val in arrayValues)
                    if (val != t[0])
                    {
                        t[i] = val;
                        i++;
                    }
                return (Combinations.Pair, t);
            }
            return (Combinations.High, arrayValues);

        }
        private Dictionary<int, int> PairsCards(IEnumerable<Card> cards)
        {
            return cards.GroupBy(f => f.Value).Select(f => new { value = f.Key, count = f.Count() }).ToDictionary(f => f.value, f => f.count);
        }
        private bool IsInFlush(IEnumerable<Card> cards)
        {
            return cards.GroupBy(f => f.Suit).Any(f => f.Count() == 5);
        }
        private bool IsInStraight(IEnumerable<Card> cards)
        {
            var values = cards.Select(f => f.Value).OrderBy(f => f);
            bool isOk = true;
            for (int i = 1; i < values.Count(); i++)
            {
                if (values.ElementAt(i) - 1 != values.ElementAt(i - 1))
                    isOk = false;
            }
            return isOk ? isOk : values.SequenceEqual(new List<int>() { 2, 3, 4, 5, 15 });
        }
    }

}
