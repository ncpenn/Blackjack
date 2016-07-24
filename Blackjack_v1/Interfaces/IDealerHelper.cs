using System.Collections.Generic;

namespace Blackjack.Interfaces
{
    internal interface IDealerHelper
    {
        void InitRound(ITable table);

        IEnumerable<uint> DealToDealer(List<uint> dealerHand, IShoe theshoe, ICardHelper cardHelper,
            IBasicStrategy basicStrategy);
    }
}
