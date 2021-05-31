using Parcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab
{
    public class MonteCarloModule : IModule
    {
        public void Run(ModuleInfo info, CancellationToken token = default)
        {
            string hand = info.Parent.ReadString();
            int numberOfRuns = info.Parent.ReadInt();
            PokerHand pokerHand = new PokerHand(hand);
            info.Parent.WriteData(MonteCarlo.Calculate(pokerHand, numberOfRuns));
        }
    }

    public class MonteCarlo
	{
        public static double Calculate(PokerHand hand, int runs)
		{
            int success = 0;
            for (int i = 0; i < runs; ++i)
			{
                success += hand.CompareWith(hand.GenerateOtherHand()) == Result.Loss
                    ? 0 : 1;
			}

            return (double)((double)success/(double)runs);
		}
	}

}
