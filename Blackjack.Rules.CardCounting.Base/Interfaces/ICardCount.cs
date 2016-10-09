using System.Collections.Generic;

namespace Blackjack.Rules.CardCounting.Base.Interfaces
{
    public interface ICardCount
    {
        int CountUsingCardsFromThisHand(List<uint> cards);

        uint BetSizeBasedOnCount(int currentCount, uint tableMinBet, uint tableMaxBet);
    }
}
