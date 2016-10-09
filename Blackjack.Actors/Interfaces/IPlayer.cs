using Blackjack.Models;
using System.Collections.Generic;

namespace Blackjack.Actors.Interfaces
{
    public interface IPlayer
    {
        PlayersHands Hands { get; }

        void SetNewHand(uint[] cards);

        IEnumerable<uint> PlayHand(IShoe shoe, uint dealerUpCard, List<uint> visibleCards, uint tableMinBet, uint tableMaxBet);

        void Settle(HandInformation dealerHand);
    }
}
