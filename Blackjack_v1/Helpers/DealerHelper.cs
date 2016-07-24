using System.Collections.Generic;
using Blackjack.Interfaces;

namespace Blackjack.Helpers
{
    internal class DealerHelper : IDealerHelper
    {
        public void InitRound(ITable table)
        {
            table.ClearVisibleCardsOffTable();
        }

        public void DealInitialCardsToDealer(List<uint> dealerCards, IShoe theshoe, ITable table)
        {
            dealerCards.Clear();
            dealerCards.AddRange(theshoe.GiveMeSomeCards(2));
            table.AddCardToVisibleCards(dealerCards[0]);

        }

        public IEnumerable<uint> DealToDealer(List<uint> dealerHand, IShoe theshoe, ICardHelper cardHelper, IBasicStrategy basicStrategy )
        {
            var cardsAddedToDealer = new List<uint>();
            if (!cardHelper.IsBlackJack(dealerHand[0], dealerHand[1]))
            {
                var handValue = basicStrategy.DetermineHandValue(dealerHand);
                var value = handValue.Value;
                while (value <= 17)
                {
                    var card = theshoe.GiveMeSomeCards(1)[0];
                    cardsAddedToDealer.Add(card);
                    dealerHand.Add(card);
                    handValue = basicStrategy.DetermineHandValue(dealerHand);
                    value = handValue.Value;
                }
            }
            return cardsAddedToDealer;
        }
    }
}
