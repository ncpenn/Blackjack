using Blackjack.Models;
using System.Collections.Generic;

namespace Blackjack.Actors.Interfaces
{
    public interface IDealer
    {
        HandInformation Hand { get; }

        void SetNewHand(uint[] cards);

        IEnumerable<uint> PlayHand(IShoe _shoe);
    }
}
