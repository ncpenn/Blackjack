using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.Interfaces
{
    public interface ICardHelper
    {
        bool IsBlackJack(uint card1, uint card2);
        int GetCountValueForCard(uint cardValue);
    }
}
