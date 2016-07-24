using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.Interfaces
{
    public interface IBetHelper
    {
        uint CardCounterFigureBetSize(uint minimumBet, uint maxBet, int theCount);
    }
}
