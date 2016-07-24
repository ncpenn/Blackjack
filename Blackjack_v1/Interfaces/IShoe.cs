using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.Interfaces
{
    public interface IShoe
    {
        uint[] GiveMeSomeCards(int numberOfCardsRequested);
        void Shuffle();
    }
}
