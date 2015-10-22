using System.Collections.Generic;
using System.Linq;

namespace Blackjack_v1.Helpers
{
    public static class DealerHelper
    {
        public static void DealerTakesCards(List<DealtCard> dealersCards, Table table)
        {
            var listOfDealerCardValues = dealersCards.Select(card => (int)card.Value).ToList();
            var dealerHandValue = BasicStrategy.DetermineHandValue(listOfDealerCardValues);
            while (dealerHandValue < 17)
            {
                var card = table.Shoe.GiveMeSomeCards(1).First();
                table.TotalNumberOfCardsDealt += 1;
                var dealtCard = new DealtCard { IsVisibleToPlayers = true, Suit = card.Suit, Value = card.Value };
                table.CardsVisibleThisRound.Add(dealtCard);
                dealersCards.Add(dealtCard);
                dealerHandValue = BasicStrategy.DetermineHandValue(dealersCards.Select(c => (int)c.Value).ToList());
            }
        }
    }
}
