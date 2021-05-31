using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System;
using System.Linq;

namespace Lab
{
	public class PokerHand
    {
        public readonly ICollection<Card> Cards;
        public static CombinationParser combinationParser = new CombinationParser();

        private PokerHand()
		{
            Cards = new HashSet<Card>();

        }
        public PokerHand(string hand)
        {
            var cards = hand.Split(' ');
            if (cards.Length != 5)
                throw new Exception("Number of cards should be 5");
            Cards = new HashSet<Card>();
            for (int i = 0; i < cards.Length; i++)
            {
                Cards.Add(new Card(cards[i]));
            }
        }

        public PokerHand GenerateOtherHand()
		{
            Random rand = new Random();
            var pokerHand = new PokerHand();
            int sameCards = rand.Next(1, 5);
            var ordered =  Cards.OrderBy(f => Guid.NewGuid());
            for (int i = 0; i < sameCards; i++)
			{
                pokerHand.Cards.Add(ordered.ElementAt(i));
			}

            while (pokerHand.Cards.Count < 5)
			{
                pokerHand.Cards.Add(new Card(rand.Next(2, 14), (Suit)rand.Next(0, 3)));
			}
            return pokerHand;
		}

        public Result CompareWith(PokerHand hand)
        {
            var combination = combinationParser.ParseCombination(Cards);
            var combination2 = combinationParser.ParseCombination(hand.Cards);
            if (combination.Item1 < combination2.Item1)
                return Result.Loss;
            else if (combination.Item1 > combination2.Item1)
                return Result.Win;
            for (int i = 0; i < combination2.Item2.Length; i++)
            {
                if (combination.Item2[i] < combination2.Item2[i])
                    return Result.Loss;
                else if (combination.Item2[i] > combination2.Item2[i])
                    return Result.Win;
            }
            return Result.Tie;
        }
    }
}
