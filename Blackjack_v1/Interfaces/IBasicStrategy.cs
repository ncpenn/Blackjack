using System.Collections.Generic;
using Blackjack.Models;

namespace Blackjack.Interfaces
{
    public interface IBasicStrategy
    {
        Enums.PlayAction DeterminePlayerNextPlay(List<uint> cardValues, uint dealerCardValue, bool canSplit);
        HandValue DetermineHandValue(List<uint> cards);
    }
}
