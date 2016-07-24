using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackjack.Actors;

namespace Blackjack.Interfaces
{
    public interface IShoe
    {
        uint[] GiveMeSomeCards(int numberOfCardsRequested);
        void Shuffle();
        event Shoe.TimeToShuffle AnnounceTimeToShuffle;
    }
}
