using System;
using System.Collections.Generic;
using Blackjack.Rules.CardCounting.Base.Interfaces;

namespace BlackJack.Rules.CardCounting.AceFive
{
    public class AceFiveCountingRules : ICardCount
    {
        public uint BetSizeBasedOnCount(int currentCount, uint tableMinBet, uint tableMaxBet)
        {
            var bet = (uint)Math.Pow(tableMinBet, currentCount);
            return bet <= tableMaxBet ? bet : tableMaxBet;
        }

        public int CountUsingCardsFromThisHand(List<uint> cards)
        {
            var count = 0;
            foreach (var card in cards)
            {
                if (card == 1)
                {
                    --count;
                }
                else if (card == 5)
                {
                    ++count;
                }
            }

            return count;
        }
    }
}
